using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper.FastCrud;
using Microsoft.Extensions.Options;
using z5.ms.common.abstractions;
using z5.ms.common.infrastructure.db;
using z5.ms.common.infrastructure.events;
using z5.ms.common.infrastructure.events.model;
using z5.ms.common.infrastructure.id;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.user;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using z5.ms.infrastructure.user.services;

namespace z5.ms.infrastructure.user.repositories
{
    /// <summary>
    /// Interface to manage customers(users)
    /// </summary>
    public interface ICustomerRepository : IBaseRepository<UserEntity>
    {
        /// <summary>
        /// Get specified customer
        /// </summary>
        /// <param name="id">The unique ID of the customer</param>
        Task<Result<Customer>> GetCustomer(Guid id,bool useReplica = false);

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <param name="customerCreate">Customer creation request</param>
        Task<Result<Customer>> CreateCustomer(CustomerCreate customerCreate);

        /// <summary>
        /// Delete specified customer
        /// </summary>
        /// <param name="id">The unique ID of the customer</param>
        Task<Result<Success>> DeleteCustomer(Guid id);

        /// <summary>
        /// Update specified customer
        /// </summary>
        /// <param name="customer">Customer model</param>
        Task<Result<Customer>> UpdateCustomer(CustomerChangeDetails customer);

        /// <summary>Set customer password</summary>
        Task<Result<Success>> SetCustomerPassword(Guid customerId, string newPassword, string IpAddress, string CountryCode);
    }

    /// <inheritdoc />
    public class CustomerRepository : BaseRepository<UserEntity>, ICustomerRepository
    {
        private readonly IMapper _mapper;
        private readonly IPasswordEncryptionStrategy _passwordStrategy;
        private readonly IEventPublisher<UserEvent> _userEventPublisher;
        private readonly UserErrors _errors;
        IHipiHandler HipiHandler { get; }
        private readonly ILogger _logger;
        private readonly ISubscriptionAPIService _subscriptionAPIService;
        IUserProfileUpdateHistoryRepository _userProfileUpdateHistoryRepository { get; }
        /// <summary>
        /// Constructor method of User Repository
        /// </summary>
        public CustomerRepository(IOptions<DbConnectionOptions> options, IMapper mapper, IPasswordEncryptionStrategy passwordStrategy,
            IEventPublisher<UserEvent> userEventPublisher, IOptions<UserErrors> errors, IHipiHandler hipiHandler, IUserProfileUpdateHistoryRepository userProfileUpdateHistoryRepository, ILoggerFactory loggerFactory, ISubscriptionAPIService subscriptionAPIService) : base(options.Value.MSDatabaseConnection,options.Value.ReplicaDatabaseConnection)
        {
            _mapper = mapper;
            _passwordStrategy = passwordStrategy;
            _userEventPublisher = userEventPublisher;
            _errors = errors.Value;
            HipiHandler = hipiHandler;
            _logger = loggerFactory.CreateLogger("Registration for success and fuilure");
            _userProfileUpdateHistoryRepository = userProfileUpdateHistoryRepository;
            _subscriptionAPIService = subscriptionAPIService;
        }

        /// <inheritdoc />
        public async Task<Result<Customer>> GetCustomer(Guid id, bool useReplica = false)
        {
            var user = await (useReplica ? GetFromReplica(id) : Get(id));
            if (user == null)
                return Result<Customer>.FromError(_errors.CustomerNotFound, 404);

            var customer = _mapper.Map<UserEntity, Customer>(user);

            return Result<Customer>.FromValue(customer);
        }

        // TODO remove validations when all projects are using validators
        // TODO Replace CustomerCreate with UserEntity
        /// <inheritdoc />
        public async Task<Result<Customer>> CreateCustomer(CustomerCreate customerCreate)
        {
            var sourceapp = customerCreate.Additional.ContainsKey("sourceapp") ? customerCreate.Additional["sourceapp"].ToString() : (customerCreate.Additional.ContainsKey("Sourceapp") ? customerCreate.Additional["Sourceapp"].ToString() : string.Empty);

            if (!string.IsNullOrEmpty(customerCreate.Email))
            {
                if (!ValidateContactDetails.IsEmail(customerCreate.Email))
                {
                    _logger.LogInformation($"FAILTURE | {DateTime.UtcNow} | {_errors.InvalidEmail.Code} | FALSE |{_errors.InvalidEmail.Message}|{sourceapp}|");
                    return Result<Customer>.FromError(_errors.InvalidEmail);
                }
                        
                var existingUser = await GetItemsWhere(nameof(UserEntity.Email), customerCreate.Email, 640);
                if (existingUser.Any())
                {
                    _logger.LogInformation($"FAILTURE | {DateTime.UtcNow} | {_errors.CustomerExists.Code} | FALSE |{_errors.CustomerExists.Message}|{sourceapp}|");
                    return Result<Customer>.FromError(_errors.CustomerExists);
                }
            }

            if (!string.IsNullOrEmpty(customerCreate.Mobile))
            {
                //Method calling changed from IsPhoneNumber to IsMobileNumber for registration issue starting with 6
                if (!ValidateContactDetails.IsMobileNumberforRegistration(customerCreate.Mobile))
                {
                    _logger.LogInformation($"FAILTURE | {DateTime.UtcNow} | {_errors.InvalidPhone.Code} | FALSE |{_errors.InvalidPhone.Message}|{sourceapp}|");
                    return Result<Customer>.FromError(_errors.InvalidPhone);
                }

                var existingUser = await GetItemsWhere(nameof(UserEntity.Mobile), customerCreate.Mobile);
                if (existingUser.Any())
                {
                    _logger.LogInformation($"FAILTURE | {DateTime.UtcNow} | {_errors.CustomerExists.Code} | FALSE |{_errors.CustomerExists.Message}|{sourceapp}|");
                    return Result<Customer>.FromError(_errors.CustomerExists);
                }
            }

            var newUser = _mapper.Map<CustomerCreate, UserEntity>(customerCreate);
            newUser.State = UserState.Verified;
            newUser.Id = Guid.NewGuid();
            newUser.PasswordHash = _passwordStrategy.HashPassword(customerCreate.Password);
            newUser.CreationDate = DateTime.UtcNow;
            newUser.ActivationDate = DateTime.UtcNow;
            newUser.IsEmailConfirmed = !string.IsNullOrWhiteSpace(newUser.Email);
            newUser.IsMobileConfirmed = !string.IsNullOrWhiteSpace(newUser.Mobile);

            var insertResult = await Insert(newUser);
            if (!insertResult.Success)
                return Result<Customer>.FromError(_errors.CustomerCreateFailed, 500);

            _logger.LogInformation($"Registration: db insert completed | UserName: {newUser.Email}|{newUser.Mobile} | UserId: {newUser.Id}");

            await _subscriptionAPIService.CreatePromotionalSubscription(newUser.Id.ToString(), newUser.IpAddress, newUser.RegistrationCountry);

            _logger.LogInformation($"Registration: SetConfirmationKey | UserName: {newUser.Email}|{newUser.Mobile}| UserId: {newUser.Id}");

            _logger.LogInformation($"SUCCESS | {DateTime.UtcNow} | {insertResult.StatusCode} | {insertResult.Success}|{customerCreate.Message} | {sourceapp}|");
            return Result<Customer>.FromValue(_mapper.Map<UserEntity, Customer>(newUser));
        }

        /// <inheritdoc />
        public async Task<Result<Success>> DeleteCustomer(Guid id)
        {
            var user = await Get(id);
            if (user == null)
                return Result<Success>.FromError(_errors.CustomerNotFound, 404);

            if (user.State == UserState.Deleted)
                return Result<Success>.FromError(_errors.CustomerDeleted);

            user.State = UserState.Deleted;
            user.PasswordHash = string.Empty;

            if (!string.IsNullOrEmpty(user.Email))
            {
                user.Email = $"{user.Email}_deleted_{DateTime.UtcNow:yyyyMMddhhmmss}";
            }
            
            if (!string.IsNullOrEmpty(user.Mobile))
            {
                user.Mobile = $"{user.Mobile}_deleted_{DateTime.UtcNow:yyyyMMddhhmmss}";
            }
            
            user.FacebookUserId = "";
            user.GoogleUserId = "";
            user.TwitterUserId = "";
            user.B2BUserId = "";

            var updateResult = await Update(user);
            if (!updateResult.Success)
                return Result<Success>.FromError(_errors.CustomerDeleteFailed, 500);

            // raise event
            await _userEventPublisher.Publish(_mapper.Map(user, new UserEvent { Type = UserEventType.Delete }));

            return Result<Success>.FromValue(new Success
            {
                Code = 1,
                Message = "The customer was deleted successfully."
            });
        }

        /// <inheritdoc />
        public async Task<Result<Customer>> UpdateCustomer(CustomerChangeDetails customer)
        {
            var currentUser = await Get(customer.Id);
            var currentUserClone = (UserEntity)currentUser.Clone();

            if (currentUser == null)
                return Result<Customer>.FromError(_errors.CustomerNotFound, 404);

            if (!string.IsNullOrEmpty(customer.Email))
            {
                using (var connection = Connection)
                {
                    var existingUser = await connection.FindAsync<UserEntity>(queryBulider => queryBulider.Where($"Id != @UserId AND Email = @Email")
                    .WithParameters(new { UserId = currentUser.Id, customer.Email }));

                    if (existingUser.Any())
                        return Result<Customer>.FromError(_errors.CustomerEmailUsed);
                }
            }

            var customerProperties = typeof(CustomerChangeDetails).GetProperties();
            var userEntityProperties = typeof(UserEntity).GetProperties();
            foreach (var cp in customerProperties)
            {
                var entityProperty = userEntityProperties.FirstOrDefault(a => a.Name.Equals(cp.Name));
                var updateValue = cp.GetValue(customer);
                var currentValue = entityProperty?.GetValue(currentUser);
                if (updateValue != null && currentValue != updateValue)
                    entityProperty?.SetValue(currentUser, updateValue);
            }

            var updateResult = await Update(currentUser);
            if (!updateResult.Success)
                return Result<Customer>.FromError(_errors.CustomerUpdateFailed, 500);

            // raise event
            await _userEventPublisher.Publish(_mapper.Map(currentUser, new UserEvent { Type = UserEventType.Update }));

            var hipiUpdateResult = await HipiHandler.PUTDataToHipiEndPoint(JsonConvert.SerializeObject(
                new HipiPutUserProfile { FirstName = customer.FirstName, LastName = customer.LastName, Dob = customer.Birthday, Email = customer.Email, PhoneNumber = customer.Mobile }
                ), currentUser.Id.ToString());

            if (!hipiUpdateResult.Success)
                _logger.LogError($"Error while updating Hipi Put Node : {hipiUpdateResult.Error?.Message}");

            await _userProfileUpdateHistoryRepository.AddItemAsync(new UserProfileUpdateHistoryItem
            {
                UserId = customer.Id,
                EmailId = currentUserClone.Email,
                MobileNumber = currentUserClone.Mobile,
                IpAddress = customer.IpAddress,
                CountryCode = customer.CountryCode,
                RequestPayload = customer.RawRequest
            });

            return Result<Customer>.FromValue(_mapper.Map<UserEntity, Customer>(currentUser));
        }

        //TODO remove validations when all projects are using validators
        public async Task<Result<Success>> SetCustomerPassword(Guid customerId, string newPassword, string IpAddress, string CountryCode)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                return Result<Success>.FromError(3, "Password is required");

            if (newPassword.Length < 6)
                return Result<Success>.FromError(3, "Password length must be at least 6 characters");

            var user = await Get(customerId);
            if (user == null)
                return Result<Success>.FromError(_errors.CustomerNotFound, 404);

            if (user.State != UserState.Verified)
                return Result<Success>.FromError(_errors.CustomerAccountIsNotActive);

            if (!user.IsEmailConfirmed && !user.IsMobileConfirmed)
                return Result<Success>.FromError(_errors.CustomerEmailAndMobileBlank);

            user.PasswordHash = _passwordStrategy.HashPassword(newPassword);

            var updateResult = await Update(user);
            if (!updateResult.Success)
                return Result<Success>.FromError(_errors.PasswordUpdate);

            await _userProfileUpdateHistoryRepository.AddItemAsync(new UserProfileUpdateHistoryItem
            {
                UserId = user.Id,
                EmailId = user.Email,
                MobileNumber = user.Mobile,
                IpAddress = IpAddress,
                CountryCode = CountryCode,
                RequestPayload = JsonConvert.SerializeObject($"Customerid:{user.Id},Password:--##--##--"),
                PasswordUpdated = true
            });

            return Result<Success>.FromValue(new Success
            {
                Code = 1,
                Message = "The customer's password has been updated"
            });
        }
    }
}
