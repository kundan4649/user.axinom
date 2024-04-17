using System;
using AutoMapper;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.user;

namespace user
{
    public class UserProfileTests
    {
        #region mocks

        private static readonly UserEntity UserEntity = new UserEntity
        {
            State = UserState.Verified,
            ActivationDate = DateTime.MinValue,
            B2BUserId = "b2b_user_id",
            Birthday = DateTime.MinValue,
            CreationDate = DateTime.MinValue,
            Email = "email@email.com",
            EmailConfirmationExpiration = DateTime.MinValue,
            EmailConfirmationKey = "email_confirmation_key",
            FacebookUserId = "facebook_user_id",
            FirstName = "first_name",
            Gender = Gender.Female,
            GoogleUserId = "google_user_id",
            Id = Guid.Parse("65b67e8e-f95e-4449-b1d6-8584b8fee51d"),
            IsEmailConfirmed = true,
            IsMobileConfirmed = true,
            LastLogin = DateTime.MinValue,
            LastName = "last_name",
            MacAddress = "mac_address",
            Mobile = "mobile",
            MobileConfirmationExpiration = DateTime.MinValue,
            MobileConfirmationKey = "mobile_confirmation_key",
            PasswordHash = "password",
            PasswordResetExpiration = DateTime.MinValue,
            PasswordResetKey = "password_reset_key",
            RegistrationCountry = "registration_country",
            System = "Internal",
            TwitterUserId = "twitter_user_id"
        };

        private static readonly User User = new User
        {
            Activated = true,
            ActivationDate = DateTime.MinValue,
            Birthday = DateTime.MinValue,
            Email = "email@email.com",
            EmailVerified = true,
            FirstName = "first_name",
            Gender = Gender.Female,
            Id = Guid.Parse("65b67e8e-f95e-4449-b1d6-8584b8fee51d"),
            LastName = "last_name",
            Mobile = "mobile",
            MobileVerified = true,
            System = "Internal",
            MacAddress = "mac_address",
            RegistrationCountry = "registration_country",
            Additional = new JObject()
        };

        private static readonly RegisterMobileUserCommand UserCreateMobile = new RegisterMobileUserCommand
        {
            FirstName = "first_name",
            LastName = "last_name",
            Mobile = "mobile",
            Password = "password"
        };

        private static readonly UserEntity UserEntityFromUserCreateMobile = new UserEntity
        {
            FirstName = "first_name",
            LastName = "last_name",
            Mobile = "mobile",
            PasswordHash = "password"
        };

        private static readonly UpdateUserCommand UpdateUserCommand = new UpdateUserCommand
        {
            Birthday = DateTime.MinValue,
            FirstName = "first_name",
            Gender = Gender.Female,
            LastName = "last_name",
            MacAddress = "mac_address"
        };

        #endregion

        private readonly IMapper _mapper;

        public UserProfileTests()
        {
            _mapper = new MapperConfiguration(m => m.AddProfile<UserProfile>()).CreateMapper();
        }

        [Fact]
        public void ValidateUserProfile()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void TestMapping_UserEntity_To_User()
        {
            // Arrange
            var sourceObject = UserEntity;
            var expectedObject = User;

            // Act
            var mappedObject = _mapper.Map<UserEntity, User>(sourceObject);

            // Assert
            mappedObject.Should().BeEquivalentTo(expectedObject);
        }

        [Fact]
        public void TestMapping_UserCreateMobile_To_UserEntity()
        {
            // Arrange
            var sourceObject = UserCreateMobile;
            var expectedObject = UserEntityFromUserCreateMobile;

            // Act
            var mappedObject = _mapper.Map<RegisterMobileUserCommand, UserEntity>(sourceObject);

            // Assert
            mappedObject.Should().BeEquivalentTo(expectedObject);
        }
    }
}