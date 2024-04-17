using AutoMapper;
using Xunit;
using z5.ms.infrastructure.user.settings;

namespace settings
{
    /// <summary>Tests to verify that SettingsProfile mapper profile works as expected</summary>
    public class SettingsProfileTests
    {
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public SettingsProfileTests()
        {
            _mapper = new MapperConfiguration(m => m.AddProfile<SettingsProfile>()).CreateMapper();
        }
        
        [Fact]
        public void ValidateCustomerProfile()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}