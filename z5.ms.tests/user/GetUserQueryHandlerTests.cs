using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using z5.ms.domain.user;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.user;
using z5.ms.infrastructure.user.repositories;
using z5.ms.infrastructure.user.user;

namespace user
{
    /// <summary>Tests for get user query handler</summary>
    public class GetUserQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
        private readonly IMapper _mapper;
        private readonly IOptions<UserErrors> _errors = Options.Create(new UserErrors());

        /// <inheritdoc />
        public GetUserQueryHandlerTests()
        {
            _mapper = new MapperConfiguration(m => m.AddProfile<UserProfile>()).CreateMapper();
        }
        
        /// <summary>Ensure that handler returns success with valid forgot user password command</summary>
        [Fact]
        public async Task GetUserQuery_Success()
        {
            // Arrange
            var query = new GetUserQuery{UserId = Guid.NewGuid()};

            _userRepository.Setup(r => r.Get(It.IsAny<Guid>()))
                .ReturnsAsync(new UserEntity {Email = "test@email.com"});
            
            var handler = new GetUserQueryHandler(_userRepository.Object, _mapper, _errors);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.True(result.Success);
        }

        /// <summary>Ensure that handler returns error if user not found</summary>
        [Fact]
        public async Task GetUserQuery_NotFound_Fail()
        {
            // Arrange
            var query = new GetUserQuery { UserId = Guid.NewGuid() };

            _userRepository.Setup(r => r.Get(It.IsAny<Guid>()))
                .ReturnsAsync((UserEntity)null);

            var handler = new GetUserQueryHandler(_userRepository.Object, _mapper, _errors);

            // Act
            var result = await handler.Handle(query);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(_errors.Value.UserNotFound, result.Error);
        }
    }
}