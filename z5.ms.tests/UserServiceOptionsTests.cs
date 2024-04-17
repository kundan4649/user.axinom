using FluentAssertions;
using Xunit;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;

public class UserServiceOptionsTests
{
    private readonly UserServiceOptions _options;
        
    public UserServiceOptionsTests()
    {
        _options = new UserServiceOptions
        {
            SkipVerifyEmail = false,
            SkipVerifyEmailExceptInCountries =
            {
                "IN",
                "EE"
            },
            SkipVerifyMobile = false,
            SkipVerifyMobileExceptInCountries =
            {
                "IN",
                "EE"
            }
        };
    }

    /// <summary>
    /// Verify that the email verification cannot be skipped when the country is in the countries list and SkipVerifyEmail is false.
    /// </summary>
    [Fact]
    public void SkipVerifyForEmail_NotVerified_CountryInList()
    {
        // Arrange
        var country = "EE";
        // Act
        var result = _options.SkipVerify(AuthenticationMethod.Email, country);
        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Verify that the mobile verification cannot be skipped when the country is in the countries list and SkipVerifyMobile is false.
    /// </summary>
    [Fact]
    public void SkipVerifyForMobile_NotVerified_CountryInList()
    {
        // Arrange
        var country = "EE";
        // Act
        var result = _options.SkipVerify(AuthenticationMethod.Mobile, country);
        // Assert
        result.Should().BeFalse();
    }


    [Theory]
    [InlineData(false, AuthenticationMethod.Email, "EE", false, true)]
    [InlineData(false, AuthenticationMethod.Email, "GA", true,  true)]
    [InlineData(false, AuthenticationMethod.Mobile, "FR", false, true)]
    [InlineData(false, AuthenticationMethod.Mobile, "TL", true, true)]
    [InlineData(false, AuthenticationMethod.Email, "LV", false, false)]
    [InlineData(true, AuthenticationMethod.Email, "LV", true, false)]
    [InlineData(false, AuthenticationMethod.Mobile, "IN", false, false)]
    [InlineData(false, AuthenticationMethod.Mobile, "IN", true, false)]
    public void SkipVerify_Success_MultiTest(bool expectedResult, AuthenticationMethod method, string country, bool skipVerifyEnabled, bool countryInList)
    {
        // Arrange
        var options = CreateUserServiceOptions(method, country, skipVerifyEnabled, countryInList);

        // Act
        var result = options.SkipVerify(method, country);
        
        // Assert
        result.Should().Be(expectedResult);
    }

    private static UserServiceOptions CreateUserServiceOptions(AuthenticationMethod method, string country, bool skipVerifyEnabled, bool countryInList)
    {
        var options = new UserServiceOptions();

        switch (method)
        {
            case AuthenticationMethod.Email:
            {
                options.SkipVerifyEmail = skipVerifyEnabled;
                if (countryInList)
                    options.SkipVerifyEmailExceptInCountries.Add(country);
                break;
            }

            case AuthenticationMethod.Mobile:
            {
                options.SkipVerifyMobile = skipVerifyEnabled;
                if (countryInList)
                    options.SkipVerifyMobileExceptInCountries.Add(country);
                break;
            }
        }

        return options;
    }
}