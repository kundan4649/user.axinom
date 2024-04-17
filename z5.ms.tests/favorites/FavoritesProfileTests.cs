using AutoMapper;
using Xunit;
using z5.ms.infrastructure.user.favorites;

namespace favorites
{
    /// <summary>Tests to verify that FavoritesProfile mapper profile works as expected</summary>
    public class FavoritesProfileTests
    {
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public FavoritesProfileTests()
        {
            _mapper = new MapperConfiguration(m => m.AddProfile<FavoritesProfile>()).CreateMapper();
        }

        [Fact]
        public void ValidateUserProfile()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}