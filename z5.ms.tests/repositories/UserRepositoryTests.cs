//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using AutoMapper;
//using Dapper;
//using Dapper.FastCrud;
//using FluentAssertions;
//using media.ms.infrastructure.user.tests;
//using z5.ms.common.abstractions;
//using Microsoft.Extensions.Logging.Abstractions;
//using Microsoft.Extensions.Options;
//using Moq;
//using Xunit;
//using z5.ms.common.infrastructure.db;
//using z5.ms.common.infrastructure.events;
//using z5.ms.common.infrastructure.events.model;
//using z5.ms.common.infrastructure.id;
//using z5.ms.domain.user;
//using z5.ms.domain.user.datamodels;
//using z5.ms.domain.user.viewmodels;
//using z5.ms.infrastructure.subscription.geoip;
//using z5.ms.infrastructure.user;
//using z5.ms.infrastructure.user.mocks;
//using z5.ms.infrastructure.validation;

//namespace z5.ms.user.tests
//{
//    /// <summary>Tests for the user rpository</summary>
//    public class UserRepositoryTests
//    {
//        private Mock<IMapper> _mockMapper;
//        private Mock<IAuthTokenService> _mockAuthTokenService;
//        private Mock<IEventPublisher<UserEvent>> _mockPublisher;
//        private Mock<IPasswordEncryptionStrategy> _mockPasswordStrategy;
//        private Mock<IOneTimePassRepository> _mockOneTimePassRepository;
//        private Mock<IGeoIpService> _mockGeoIpService;
//        private IOptions<UserErrors> _userErrorOptions;
//        private IOptions<TokenServiceOptions> _tokenServiceOptions;
//        private IOptions<DbConnectionOptions> _dbConnectionOptions;
//        private IOptions<UserServiceOptions> _userServiceOptions;
//        private IDbConnection _connection;

//        private readonly List<UserEntity> _userDbos = new UserMocks().UserDbos;
//        private readonly IUserRepository _userRepository;

//        public UserRepositoryTests()
//        {
//            SetupDatabase();
//            SetupOptions();
//            SetupMocks();

//            _userRepository = new UserRepository(_userServiceOptions, _mockPublisher.Object, _mockMapper.Object, _mockPasswordStrategy.Object,
//                _mockOneTimePassRepository.Object, NullLoggerFactory.Instance, _userErrorOptions, _mockAuthTokenService.Object, _mockGeoIpService.Object, _tokenServiceOptions,
//                _dbConnectionOptions);
//        }

//        #region Helper methods

//        private void SetupMocks()
//        {
//            _mockMapper = new Mock<IMapper>();
//            _mockMapper.Setup(m => m.Map(It.IsAny<UserEntity>(), It.IsAny<UserEvent>())).Returns(new UserEvent());

//            _mockAuthTokenService = new Mock<IAuthTokenService>();
//            _mockAuthTokenService.Setup(authService => authService.CreateAuthToken(It.IsAny<UserEntity>())).ReturnsAsync(Result<string>.FromValue(new UserMocks().Tokens.First().AuthToken));
//            _mockAuthTokenService.Setup(authService => authService.GetCountryInfo(It.IsAny<string>()))
//                .ReturnsAsync(("country", "state"));

//            _mockPublisher = new Mock<IEventPublisher<UserEvent>>();
//            _mockPublisher.Setup(m => m.Publish(It.IsAny<UserEvent>())).ReturnsAsync(new Result<Success>());

//            _mockPasswordStrategy = new Mock<IPasswordEncryptionStrategy>();
//            _mockPasswordStrategy.Setup(strategy => strategy.HashPassword(It.IsAny<string>())).Returns<string>(param => param);
//            _mockPasswordStrategy.Setup(strategy => strategy.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

//            _mockOneTimePassRepository = new Mock<IOneTimePassRepository>();
//            _mockOneTimePassRepository
//                .Setup(otpRepo => otpRepo.CreateCode(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<OtpDescriptor>()))
//                .ReturnsAsync((Guid guid, string code, OtpDescriptor descriptor) => Result<OneTimePassEntity>.FromValue(
//                    new OneTimePassEntity
//                    {
//                        Code = guid.ToString(),
//                    }));

//            _mockOneTimePassRepository
//                .Setup(otpRepo => otpRepo.ValidateCode(It.IsAny<OtpGroup>(), It.IsAny<string>()))
//                .ReturnsAsync((OtpGroup group, string code) => Result<OneTimePassEntity>.FromValue(
//                    new OneTimePassEntity
//                    {
//                        Code = code,
//                        OtpGroup = group,
//                        UserId = Guid.TryParse(code, out var guid) ? guid : Guid.NewGuid() // a small hack so the user search using the otp entity works, pass userid as code
//                    }));

//            _mockGeoIpService = new Mock<IGeoIpService>();
//            _mockGeoIpService.Setup(ipService => ipService.LookupCountry(It.IsAny<string>()))
//                .Returns(Result<Country>.FromValue(new Country
//                {
//                    CountryCode = "CO",
//                    State = "State"
//                }));
//        }

//        private void SetupOptions()
//        {
//            _userErrorOptions = Options.Create(new UserErrors());
//            _tokenServiceOptions = Options.Create(new TokenServiceOptions());
//            _dbConnectionOptions = Options.Create(new DbConnectionOptions
//            {
//                MSDatabaseConnection = _connection.ConnectionString
//            });

//            _userServiceOptions = Options.Create(new UserServiceOptions
//            {
//                SessionKey = "YQ=="
//            });
//        }

//        private void SetupDatabase()
//        {
//            SqlLiteHelpers.ConfigureDapper();
//            _connection = SqlLiteHelpers.CreateTestDatabase();

//            _connection.Execute(@"
//            CREATE TABLE [Users] (Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
//                                                    Email NVARCHAR NULL,
//                                                    Mobile NVARCHAR NULL,
//                                                    FirstName NVARCHAR NOT NULL,
//                                                    LastName NVARCHAR NOT NULL,
//                                                    PasswordHash NVARCHAR NOT NULL,
//                                                    EmailConfirmationKey NVARCHAR NULL,
//                                                    MobileConfirmationKey NVARCHAR NULL,
//                                                    EmailConfirmationExpiration DATETIME NULL,
//                                                    MobileConfirmationExpiration DATETIME NULL,
//                                                    IsEmailConfirmed BIT NULL,
//                                                    IsMobileConfirmed BIT NULL,
//                                                    PasswordResetKey NVARCHAR(48),
//                                                    PasswordResetExpiration DATETIME NULL,
//                                                    LastLogin DATETIME NULL,
//                                                    System INT NULL,
//                                                    RegistrationCountry NVARCHAR NULL,
//                                                    MacAddress NVARCHAR NULL,
//                                                    Birthday DATETIME NULL,
//                                                    Gender INT NULL,
//                                                    ActivationDate DATETIME NULL,
//                                                    CreationDate DATETIME NULL,
//                                                    State INT NULL,
//                                                    RegistrationRegion NVARCHAR NULL,
//                                                    FacebookUserId NVARCHAR NULL,
//                                                    GoogleUserId NVARCHAR NULL,
//                                                    TwitterUserId NVARCHAR NULL,
//                                                    AmazonUserId NVARCHAR NULL,
//                                                    B2BUserId NVARCHAR NULL,
//                                                    ProviderName NVARCHAR NULL,
//                                                    ProviderSubjectId NVARCHAR NULL,
//                                                    NewEmail NVARCHAR NULL,
//                                                    NewEmailConfirmationKey NVARCHAR NULL,
//                                                    NewEmailConfirmationExpiration NVARCHAR NULL,
//                                                    IpAddress NVARCHAR NULL,
//                                                    Json NVARCHAR NULL
//                                                    )"
//            );
//        }

//        #endregion

//        /// <summary>
//        /// Verify that creating an user using an email is successful.
//        /// </summary>
//        [Fact]
//        public async void CreateUserUsingEmailAsync_ReturnsUserEntityWithCorrectEmail()
//        {
//            // Arrange
//            var userDbo = new UserMocks().UserDbos.First();

//            // Act
//            var result = await _userRepository.CreateUserUsingEmailAsync(userDbo, "Internal");

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();
//            result.Value.Email.Should().Be(userDbo.Email);
//        }

//        /// <summary>
//        /// Verify that creating an user using a mobile number is successful.
//        /// </summary>
//        [Fact]
//        public async void CreateUserUsingMobileAsync_ReturnsUserEntity()
//        {
//            // Arrange
//            var userDbo = new UserMocks().UserDbos.First();

//            // Act
//            var result = await _userRepository.CreateUserUsingMobileAsync(userDbo, "Internal");

//            // Assert           
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();
//            result.Value.Mobile.Should().Be(userDbo.Mobile);
//        }

//        /// <summary>
//        /// Verify that creating an user using Facebook when the user doesn't exist is successful.
//        /// </summary>
//        [Fact]
//        public async void CreateUserUsingFacebookAsync_ReturnsUserEntity_WhenUserDoesntExist()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.FacebookUserId = "FBId";
//            // Act
//            var result = await _userRepository.CreateUserUsingFacebookAsync(userDbo, "Internal");

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();
//            result.Value.FacebookUserId.Should().Be(userDbo.FacebookUserId);
//        }

//        /// <summary>
//        /// Verify that creating an user using Facebook when the user already is registered updates the existing user.
//        /// </summary>
//        [Fact]
//        public async void CreateUserUsingFacebookAsync_ReturnsUserEntity_WhenUserExists()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();

//            _connection.Insert(userDbo);

//            userDbo.FacebookUserId = "FBId";

//            // Act
//            var result = await _userRepository.CreateUserUsingFacebookAsync(userDbo, "Internal");

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.FacebookUserId.Should().Be(userDbo.FacebookUserId);
//        }

//        /// <summary>
//        /// Verify that creating an user using Facebook when the user doesn't exist is successful. 
//        /// </summary>
//        [Fact]
//        public async void CreateUserUsingGoogleAsync_ReturnsUserEntity_WhenUserDoesntExist()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.GoogleUserId = "GoogleId";
//            // Act
//            var result = await _userRepository.CreateUserUsingGoogleAsync(userDbo, "Internal");

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();
//            result.Value.GoogleUserId.Should().Be(userDbo.GoogleUserId);
//        }


//        /// <summary>
//        /// Verify that creating an user using Google when the user already is registered updates the existing user. 
//        /// </summary>
//        [Fact]
//        public async void CreateUserUsingGoogleAsync_ReturnsUserEntity_WhenUserExists()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();

//            _connection.Insert(userDbo);
//            userDbo.GoogleUserId = "GoogleId";

//            // Act
//            var result = await _userRepository.CreateUserUsingGoogleAsync(userDbo, "Internal");

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.GoogleUserId.Should().Be(userDbo.GoogleUserId);
//        }

//        /// <summary>
//        /// Verify that creating an user using Facebook when the user doesn't exist is successful. 
//        /// </summary>
//        [Fact]
//        public async void CreateUserUsingTwitterAsync_ReturnsUserEntity_WhenUserDoesntExist()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.TwitterUserId = "TwitterId";
//            // Act
//            var result = await _userRepository.CreateUserUsingTwitterAsync(userDbo, "Internal");

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();
//            result.Value.TwitterUserId.Should().Be(userDbo.TwitterUserId);
//        }


//        /// <summary>
//        /// Verify that creating an user using Google when the user already is registered updates the existing user. 
//        /// </summary>
//        [Fact]
//        public async void CreateUserUsingTwitterAsync_ReturnsUserEntity_WhenUserExists()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            _connection.Insert(userDbo);
//            userDbo.TwitterUserId = "TwitterId";

//            // Act
//            var result = await _userRepository.CreateUserUsingTwitterAsync(userDbo, "Internal");

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.TwitterUserId.Should().Be(userDbo.TwitterUserId);
//        }

//        /// <summary>
//        /// Verify that creating an user using Facebook when the user doesn't exist is successful. 
//        /// </summary>
//        [Fact]
//        public async void CreateUserUsingAmazonAsync_ReturnsUserEntity_WhenUserDoesntExist()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.AmazonUserId = "AmazonId";
//            // Act
//            var result = await _userRepository.CreateUserUsingAmazonAsync(userDbo, "Internal");

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();
//            result.Value.AmazonUserId.Should().Be(userDbo.AmazonUserId);
//        }


//        /// <summary>
//        /// Verify that creating an user using Google when the user already is registered updates the existing user. 
//        /// </summary>
//        [Fact]
//        public async void CreateUserUsingAmazonAsync_ReturnsUserEntity_WhenUserExists()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();

//            _connection.Insert(userDbo);
//            userDbo.AmazonUserId = "AmazonId";

//            // Act
//            var result = await _userRepository.CreateUserUsingAmazonAsync(userDbo, "Internal");

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.AmazonUserId.Should().Be(userDbo.AmazonUserId);
//        }

//        /// <summary>Verify that requesting email change confirmation code works.</summary>
//        [Fact]
//        public async void EmailChangeConfirmationCodeAsync_Success()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.State = UserState.Verified;
//            _connection.Insert(userDbo);

//            // Act
//            var newEmailAddress = "new@email.cc";
//            var result = await _userRepository.EmailChangeConfirmationCodeAsync(userDbo.Id, newEmailAddress);
//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.EmailConfirmationKey.Should().Be(userDbo.Id.ToString());
//        }


//        /// <summary>Verify that requesting mobile change confirmation code works.</summary>
//        [Fact]
//        public async void MobileChangeConfirmationCodeAsync_Success()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.State = UserState.Verified;
//            _connection.Insert(userDbo);

//            // Act
//            var newMobile = "37255555555";
//            var result = await _userRepository.MobileChangeConfirmationCodeAsync(userDbo.Id, newMobile);
//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.MobileConfirmationKey.Should().Be(userDbo.Id.ToString());
//        }


//        [Fact]
//        public async void UpdateUserAsync_ReturnsSuccessResult_WhenFieldsChanged()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            _connection.Insert(userDbo);

//            var userChangeDetails = new UserChangeDetails
//            {
//                FirstName = "NewFirstName",
//                LastName = "NewLastName",
//                Gender = Gender.Unknown
//            };
//            // Act
//            var result = await _userRepository.UpdateUserAsync(userDbo.Id, userChangeDetails);

//            // Assert
//            result.Success.Should().BeTrue();

//            var updatedUserDbo = _connection.Get(userDbo);
//            updatedUserDbo.FirstName.Should().Be(userChangeDetails.FirstName);
//            updatedUserDbo.LastName.Should().Be(userChangeDetails.LastName);
//            updatedUserDbo.Gender.Should().Be(userChangeDetails.Gender);
//        }

//        [Fact]
//        public async void DeleteUserAsync_HasCorrectFields_AfterDeletion()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            _connection.Insert(userDbo);

//            // Act
//            var result = await _userRepository.DeleteUserAsync(userDbo.Id);

//            // Assert
//            result.Success.Should().BeTrue();

//            var deletedUserDbo = _connection.Get(userDbo);
//            deletedUserDbo.State.Should().Be(UserState.Deleted);
//            deletedUserDbo.Email.Should().StartWith($"{userDbo.Email}_deleted_");
//            deletedUserDbo.Mobile.Should().StartWith($"{userDbo.Mobile}_deleted_");
//        } 
        
//        [Fact]
//        public async void DeleteUserAsync_EmailAndMobileNotChanged_WhenTheyAreEmpty()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.Email = "";
//            userDbo.Mobile = "";
            
//            _connection.Insert(userDbo);
//            // Act
//            var result = await _userRepository.DeleteUserAsync(userDbo.Id);

//            // Assert
//            result.Success.Should().BeTrue();

//            var deletedUserDbo = _connection.Get(userDbo);
//            deletedUserDbo.State.Should().Be(UserState.Deleted);
//            deletedUserDbo.Email.Should().BeEmpty();
//            deletedUserDbo.Mobile.Should().BeEmpty();
//        }

//        [Fact]
//        public async void ConfirmEmailAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.EmailConfirmationExpiration = DateTime.UtcNow.AddHours(1);
//            userDbo.State = UserState.Registered;
//            _connection.Insert(userDbo);

//            // Act
//            var result = await _userRepository.ConfirmEmailAsync(userDbo.Id.ToString());

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();
//            result.Value.State.Should().Be(UserState.Verified);
//        }

//        [Fact]
//        public async void ConfirmMobileAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.MobileConfirmationExpiration = DateTime.UtcNow.AddHours(1);
//            _connection.Insert(userDbo);
//            // Act
//            var result = await _userRepository.ConfirmMobileAsync(userDbo.Id.ToString());

//            // Assert

//            result.Success.Should().BeTrue();
//            result.Value.Should().BeTrue();

//            var confirmedUserDbo = _connection.Get(userDbo);
//            confirmedUserDbo.State.Should().Be(UserState.Verified);
//        }

//        [Fact]
//        public async void LoginUsingEmailAsync_Basic()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.IsEmailConfirmed = true;
//            userDbo.State = UserState.Verified;
//            var password = "pass123";
//            _connection.Insert(userDbo);

//            // Act
//            var result = await _userRepository.LoginUsingEmailAsync(userDbo.Email, password, null);

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();

//            result.Value.Id.Should().Be(userDbo.Id);
//            result.Value.LastLogin.Should().NotBe(userDbo.LastLogin.GetValueOrDefault(DateTime.MinValue));
//        }

//        [Fact]
//        public async void LoginUsingMobileAsync_Basic()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.IsMobileConfirmed = true;
//            userDbo.State = UserState.Verified;
//            var password = "pass123";
//            _connection.Insert(userDbo);
//            // Act
//            var result = await _userRepository.LoginUsingMobileAsync(userDbo.Mobile, password, null);

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();

//            result.Value.Id.Should().Be(userDbo.Id);
//            result.Value.LastLogin.Should().NotBe(userDbo.LastLogin.GetValueOrDefault(DateTime.MinValue));
//        }

//        [Fact]
//        public async void LoginUsingFacebookAsync_Basic()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.State = UserState.Verified;
//            userDbo.FacebookUserId = "FBId";
//            _connection.Insert(userDbo);
//            // Act
//            var result = await _userRepository.LoginUsingFacebookAsync(userDbo, null);

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();

//            result.Value.Id.Should().Be(userDbo.Id);
//            result.Value.LastLogin.Should().NotBe(userDbo.LastLogin.GetValueOrDefault(DateTime.MinValue));
//        }

//        [Fact]
//        public async void LoginUsingGoogleAsync_Basic()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.State = UserState.Verified;
//            userDbo.GoogleUserId = "GoogleId";
//            _connection.Insert(userDbo);
//            // Act
//            var result = await _userRepository.LoginUsingGoogleAsync(userDbo, null);

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();

//            result.Value.Id.Should().Be(userDbo.Id);
//            result.Value.LastLogin.Should().NotBe(userDbo.LastLogin.GetValueOrDefault(DateTime.MinValue));
//        }

//        [Fact]
//        public async void LoginUsingAmazonAsync_Basic()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.State = UserState.Verified;
//            userDbo.AmazonUserId = "Amazon";
//            _connection.Insert(userDbo);
//            // Act
//            var result = await _userRepository.LoginUsingAmazonAsync(userDbo, null);

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();

//            result.Value.Id.Should().Be(userDbo.Id);
//            result.Value.LastLogin.Should().NotBe(userDbo.LastLogin.GetValueOrDefault(DateTime.MinValue));
//        }

//        [Fact]
//        public async void LoginUsingTwitterAsync_Basic()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.State = UserState.Verified;
//            userDbo.TwitterUserId = "TwitterId";
//            _connection.Insert(userDbo);
//            // Act
//            var result = await _userRepository.LoginUsingTwitterAsync(userDbo, null);

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.Should().BeOfType<UserEntity>();

//            result.Value.Id.Should().Be(userDbo.Id);
//            result.Value.LastLogin.Should().NotBe(userDbo.LastLogin.GetValueOrDefault(DateTime.MinValue));
//        }

//        [Fact]
//        public async void ChangePasswordAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.IsMobileConfirmed = true;
//            userDbo.State = UserState.Verified;
//            _connection.Insert(userDbo);

//            var oldPassword = "pass123";
//            var newPassword = "abcdef";
//            // Act
//            var result = await _userRepository.ChangePasswordAsync(userDbo.Id, oldPassword, newPassword);

//            // Assert
//            Assert.True(result.Success);
//            var boolResult = Assert.IsType<bool>(result.Value);
//            Assert.True(boolResult);
//        }

//        [Fact]
//        public async void PasswordForgottenAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.State = UserState.Verified;
//            _connection.Insert(userDbo);
//            // Act
//            var result = await _userRepository.PasswordForgottenEmailAsync(userDbo.Email);

//            // Assert
//            Assert.True(result.Success);
//            var userDboResult = Assert.IsType<UserEntity>(result.Value);
//            Assert.NotNull(userDboResult);
//            Assert.Equal(userDboResult.Id, userDbo.Id);
//        }

//        [Fact]
//        public async void RecreatePasswordAsync_ReturnsSuccessResult()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.PasswordResetExpiration = DateTime.UtcNow.AddHours(1);
//            userDbo.State = UserState.Verified;
//            _connection.Insert(userDbo);

//            var newPassword = "abcdef";
//            // Act
//            var result = await _userRepository.RecreatePasswordAsync(userDbo.PasswordResetKey, newPassword);

//            // Assert
//            Assert.True(result.Success);
//            var boolResult = Assert.IsType<bool>(result.Value);
//            Assert.True(boolResult);
//        }

//        [Fact]
//        public async void GetUserAsync_ReturnsUserEntity()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            _connection.Insert(userDbo);
//            // Act
//            var result = await _userRepository.GetUserAsync(userDbo.Id);

//            // Assert
//            Assert.True(result.Success);
//            var userDboResult = Assert.IsType<UserEntity>(result.Value);
//            Assert.NotNull(userDboResult);
//            Assert.Equal(userDboResult.Id, userDbo.Id);
//        }

//        [Fact]
//        public async void GetUserUsingEmailAsync_ReturnsUserEntity()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            _connection.Insert(userDbo);
//            // Act
//            var result = await _userRepository.GetUserUsingEmailAsync(userDbo.Email);

//            // Assert
//            Assert.True(result.Success);
//            var userDboResult = Assert.IsType<UserEntity>(result.Value);
//            Assert.NotNull(userDboResult);
//            Assert.Equal(userDboResult.Id, userDbo.Id);
//        }

//        [Fact]
//        public async void GetUserUsingMobileAsync_ReturnsUserEntity()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            _connection.Insert(userDbo);
//            // Act
//            var result = await _userRepository.GetUserUsingMobileAsync(userDbo.Mobile);

//            // Assert
//            Assert.True(result.Success);
//            var userDboResult = Assert.IsType<UserEntity>(result.Value);
//            Assert.NotNull(userDboResult);
//            Assert.Equal(userDboResult.Id, userDbo.Id);
//        }

//        /// <summary>Verify that confirming email change works.</summary>
//        [Fact]
//        public async void ConfirmEmailChangeAsync_Success()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            userDbo.State = UserState.Verified;
//            _connection.Insert(userDbo);

//            // Act
//            var result = await _userRepository.ConfirmEmailChangeAsync(userDbo.Id.ToString());
//            // Assert
//            result.Success.Should().BeTrue();

//            var confirmedUser = _connection.Get(userDbo);
//            confirmedUser.IsEmailConfirmed.Should().BeTrue();
//        }

//        /// <summary>Verify that UpdateUser really updates the user in the database.</summary>
//        [Fact]
//        public async void UpdateUserAsync_UpdatesUser()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            _connection.Insert(userDbo);

//            // Act
//            userDbo.LastName = "UpdatedLastName";
//            var result = await _userRepository.UpdateUserAsync(userDbo);

//            // Assert
//            result.Success.Should().BeTrue();

//            var confirmedUser = _connection.Get(userDbo);
//            confirmedUser.LastName.Should().Be(userDbo.LastName);
//        }

//        /// <summary>Verify that a valid email confirmation code is recreated</summary>
//        [Fact]
//        public async void RecreateEmailConfirmationCodeAsync_Success()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            _connection.Insert(userDbo);

//            // Act
//            var email = userDbo.Email;
//            var result = await _userRepository.RecreateEmailConfirmationCodeAsync(email);

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.EmailConfirmationKey.Should().Be(userDbo.Id.ToString());
//        }

//        /// <summary>Verify that a valid mobile confirmation code is recreated</summary>
//        [Fact]
//        public async void RecreateMobileConfirmationCodeAsync_Success()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            _connection.Insert(userDbo);

//            // Act
//            var result = await _userRepository.RecreateMobileConfirmationCodeAsync(userDbo.Id);

//            // Assert
//            result.Success.Should().BeTrue();
//            result.Value.MobileConfirmationKey.Should().Be(userDbo.Id.ToString());
//        }

//        /// <summary>Verify that country info is returned.</summary>
//        [Fact]
//        public async void GetCountryInfo_Success()
//        {
//            // Arrange
//            var userDbo = _userDbos.First();
//            _connection.Insert(userDbo);

//            // Act
//            var (country, state) = await _userRepository.GetCountryInfo(userDbo.IpAddress);

//            // Assert
//            country.Should().NotBeNullOrEmpty();
//            state.Should().NotBeNullOrEmpty();
//        }
//    }
//}