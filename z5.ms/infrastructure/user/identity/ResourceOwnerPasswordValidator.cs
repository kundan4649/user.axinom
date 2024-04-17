using System;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using z5.ms.common.infrastructure.id;

namespace z5.ms.infrastructure.user.identity
{
    /// <inheritdoc />
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ITokenUserRepository _userRepository;
        private readonly IPasswordEncryptionStrategy _encryptionStrategy;

        /// <inheritdoc />
        public ResourceOwnerPasswordValidator(ITokenUserRepository userRepository,  IPasswordEncryptionStrategy encryptionStrategy)
        {
            _userRepository = userRepository;
            _encryptionStrategy = encryptionStrategy;
        }

        /// <inheritdoc />
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userRepository.FindByUsername(context.UserName);
            if (_encryptionStrategy.VerifyPassword(context.Password, user.PasswordHash))
            {
                context.Result = new GrantValidationResult(user.Id.ToString(), "password", DateTime.Now);
            }
        }
    }
}