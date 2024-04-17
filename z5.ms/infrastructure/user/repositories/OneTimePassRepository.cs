using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.db;
using z5.ms.common.infrastructure.id;
using z5.ms.domain.user.user;

namespace z5.ms.infrastructure.user.repositories
{
    /// <summary>Description used to create a one time pass for one particular purpose</summary>
    public class OtpDescriptor
    {
        /// <summary>All one time passes in each group are constrained to be unique</summary>
        public OtpGroup OtpGroup { get; set; }

        /// <summary>Expiry duration of the one time pass. Default 1 day</summary>
        public TimeSpan Duration { get; set; } = TimeSpan.FromDays(1);

        /// <summary>A strategy used to generate the random code</summary>
        /// <remarks>Default AlphaNumeric</remarks>
        public OtpCreationStrategy OtpCreationStrategy { get; set; } = OtpCreationStrategy.AlphaNumeric;

        /// <summary>The length of the code. Used by some key creation strategies</summary>
        /// /// <remarks>Default 8</remarks>
        public int KeyLength { get; set; } = 8;
    }

    /// <summary>All keys used for the same purposes should share a group</summary>
    public enum OtpGroup
    {
        /// <summary>Key is used to reset password by email</summary>
        ResetPasswordEmail = 1,

        /// <summary>Key is used to reset password by mobile</summary>
        ResetPasswordMobile = 2,

        /// <summary>Key is used to confirm email</summary>
        ConfirmationEmail = 3,

        /// <summary>Key is used to confirm mobile</summary>
        ConfirmationMobile = 4,

        /// <summary>Mife unsubcribe otp</summary>
        MifeUnsubscribe = 5
    }

    /// <summary>Strategies for rendomly generating one time passes</summary>
    public enum OtpCreationStrategy
    {
        /// <summary>A random sequence of mixed case, alphanumeric characters</summary>
        AlphaNumeric = 0,

        /// <summary>A random sequence of digits 0-9</summary>
        Numeric = 1,

        /// <summary>A sequence of 4 digits. Unused values are selected from a pregenerated table.</summary>
        FourDigitPregenerated = 2,
    }

    /// <summary>A one time pass entity stored in the user database</summary>
    [Table("OneTimePasses")]
    public class OneTimePassEntity
    {
        /// <summary>Unique id of the item which is associated with (eg. UserId, SubscriptionId)</summary>
        public Guid UserId { get; set; }

        /// <summary>All codes in each group are constrained to be unique</summary>
        public OtpGroup OtpGroup { get; set; }

        /// <summary>The randomly generated code</summary>
        public string Code { get; set; }

        /// <summary>The address (sms or email) that the one time pass was sent to</summary>
        public string RecipientAddress { get; set; }

        /// <summary>Expiry time of the one time pass</summary>
        public DateTime Expires { get; set; }
    }

    /// <summary>Interface to manage one time passes</summary>
    public interface IOneTimePassRepository : IBaseRepository<OneTimePassEntity>
    {
        /// <summary>Create a one time pass</summary>
        Task<Result<OneTimePassEntity>> CreateCode(Guid ownerId, string recipientAddress, OtpDescriptor descriptor);

        /// <summary>Validate a confirmation code</summary>
        Task<Result<OneTimePassEntity>> ValidateCode(OtpGroup otpGroup, string code);

        /// <summary>Validate a confirmation code</summary>
       Task<Result<OneTimePassEntity>> ValidateCodev2(OtpGroup otpGroup, ConfirmUserCommandv2 confirmUserCommandv2);

        /// <summary>Delete all codes for one group for one user</summary>
        Task DeleteCodes(Guid userId, OtpGroup otpGroup);
    }

    /// <summary>A repository for one time passes sent by email or mobile</summary>
    public class OneTimePassRepository : BaseRepository<OneTimePassEntity>, IOneTimePassRepository
    {
        private readonly ILogger _logger;

        private static readonly object CleanupLock = new object();
        private static DateTime _lastCleanup = DateTime.MinValue;

        /// <inheritdoc />
        public OneTimePassRepository(IOptions<DbConnectionOptions> dbOptions, ILoggerFactory logger) 
            : base(dbOptions.Value.MSDatabaseConnection)
        {
            _logger = logger?.CreateLogger(GetType().Name);
        }

        /// <inheritdoc />
        public async Task<Result<OneTimePassEntity>> CreateCode(Guid ownerId, string recipientAddress, OtpDescriptor descriptor)
        {
            var expires = DateTime.UtcNow.Add(descriptor.Duration);
            var codes = await GenerateCodes(descriptor.OtpGroup, descriptor.OtpCreationStrategy, descriptor.KeyLength);
            foreach (var code in codes)
            {
                var otp = new OneTimePassEntity
                {
                    UserId = ownerId,
                    OtpGroup = descriptor.OtpGroup,
                    Code = code,
                    RecipientAddress = recipientAddress,
                    Expires = expires
                };

                var result = await Insert(otp);

                if (!result.Success)
                {
                    if (IsUniqueConstraintViolation(result.Error))
                        continue;

                    return Result<OneTimePassEntity>.FromError(result.Error);
                }

                CleanupExpiredConfirmationKeys();
                return Result<OneTimePassEntity>.FromValue(otp);
            }

            _logger.LogError($"Failed to update one time pass: {descriptor.OtpGroup.ToString()}. Could not generate a unique code for user in 20 attempts.");
            return Result<OneTimePassEntity>.FromError(1, "DB update failure", 500);
        }

        /// <inheritdoc />
        public async Task<Result<OneTimePassEntity>> ValidateCode(OtpGroup otpGroup, string code)
        {
            var otp = await SingleOrDefaultWhere(nameof(OneTimePassEntity.Code), code, nameof(OneTimePassEntity.OtpGroup), otpGroup);
            return otp == null
                ? Result<OneTimePassEntity>.FromError(1, "Confirmation code was not found")
                : otp.Expires < DateTime.UtcNow
                    ? Result<OneTimePassEntity>.FromError(1, "Confirmation code expired")
                    : Result<OneTimePassEntity>.FromValue(otp);
        }

        public async Task<Result<OneTimePassEntity>> ValidateCodev2(OtpGroup otpGroup, ConfirmUserCommandv2 confirmUserCommandv2)
        {
            var otp = await SingleOrDefaultWhere2(nameof(OneTimePassEntity.Code), confirmUserCommandv2.Code, nameof(OneTimePassEntity.OtpGroup), otpGroup, nameof(confirmUserCommandv2.RecipientAddress), confirmUserCommandv2.RecipientAddress);
            return otp == null
                ? Result<OneTimePassEntity>.FromError(1, "Confirmation code was not found")
                : otp.Expires < DateTime.UtcNow
                    ? Result<OneTimePassEntity>.FromError(1, "Confirmation code expired")
                    : Result<OneTimePassEntity>.FromValue(otp);
        }

        /// <inheritdoc />
        public async Task DeleteCodes(Guid ownerId, OtpGroup otpGroup)
        {
            using (var connection = Connection)
            {
                await connection.ExecuteAsync(
                $" DELETE FROM OneTimePasses WHERE {nameof(OneTimePassEntity.UserId)} = @ownerId AND {nameof(OneTimePassEntity.OtpGroup)} = @otpGroup "
                , new { ownerId, otpGroup });
            }
        }

        /// <summary>Clean up expired confirmation keys. This isn't crucial other than to preserve the performance of queries when creating new confirmation keys.
        /// Each program instance will run this query at most once per hour.</summary>
        private void CleanupExpiredConfirmationKeys()
        {
            lock (CleanupLock)
            {
                if (_lastCleanup > DateTime.UtcNow.AddMinutes(-15)) return;
                _lastCleanup = DateTime.UtcNow;
            }
            Task.Run(() =>
            {
                try
                {
                    // create a new connection. The one from http scope may be disposed already
                    using (var connection = Connection)
                    {
                        connection.Execute(
                            $" DELETE FROM OneTimePasses WHERE {nameof(OneTimePassEntity.Expires)} < @UtcNow ",
                            new { DateTime.UtcNow });
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while cleaning up expired one time passes");
                }
            });
        }

        /// <summary>Generates 20 random codes</summary>
        private async Task<IEnumerable<string>> GenerateCodes(OtpGroup keyGroup, OtpCreationStrategy otpCreationStrategy, int length)
        {
            if (otpCreationStrategy == OtpCreationStrategy.FourDigitPregenerated)
                return await GetRandomFourDigitPregeneratedCodes(keyGroup);

            return GenerateRandomCodes(otpCreationStrategy, length);
        }

        private IEnumerable<string> GenerateRandomCodes(OtpCreationStrategy otpCreationStrategy, int length)
        {
            for (var i = 0; i < 20; i++)
                yield return GenerateRandomCode(otpCreationStrategy, length);
        }

        /// <summary>Selects 20 random codes 4 digit numeric codes that are not currently in use</summary>
        private async Task<IEnumerable<string>> GetRandomFourDigitPregeneratedCodes(OtpGroup otpGroup)
        {
            using (var connection = Connection)
            {
                return (await connection.QueryAsync<string>($@" 
                        SELECT TOP 20 * FROM MobileOtpCodes
                        WHERE Code NOT IN (SELECT {nameof(OneTimePassEntity.Code)} FROM OneTimePasses WHERE {nameof(OneTimePassEntity.OtpGroup)} = @otpGroup)
                        ORDER BY NEWID() "
                    , new { otpGroup }))
                .ToList();
            }
        }

        private const string AlphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const string NumericChars = "0123456789";

        // we use a locked static instance to prevent duplication
        private static readonly Random Random = new Random();
        private static int RandomSeed()
        {
            lock (Random) return Random.Next();
        }

        /// <summary>Generates a random confirmation key</summary>
        private static string GenerateRandomCode(OtpCreationStrategy otpCreationStrategy, int length)
        {
            var random = new Random(RandomSeed());
            var characters = otpCreationStrategy == OtpCreationStrategy.Numeric ? NumericChars : AlphanumericChars;
            return new string(Enumerable.Repeat(characters, length).Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}