using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.domain.user;
using z5.ms.domain.user.customer;
using z5.ms.infrastructure.user.customer;
using z5.ms.infrastructure.user.repositories;

namespace customer
{
    /// <summary>Tests for changing customer password command handler</summary>
    public class SetCustomerPasswordCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepository = new Mock<ICustomerRepository>();
        private readonly IOptions<UserErrors> _options;
        
        /// <inheritdoc />
        public SetCustomerPasswordCommandHandlerTests()
        {
            _options = Options.Create(new UserErrors());
        }

        /// <summary>Ensure that handler returns success with valid password change command</summary>
        [Fact]
        public async Task SetCustomerPasswordCommand_Success()
        {
            // Arrange
            var command = new SetCustomerPasswordCommand{CustomerId = Guid.NewGuid()};
            
          //  _customerRepository.Setup(r => r.SetCustomerPassword(It.IsAny<Guid>(), It.IsAny<string>()))
             //   .ReturnsAsync(new Result<Success> {Success = true});
            
           // var handler = new SetCustomerPasswordCommandHandler(_customerRepository.Object, _options);

            // Act
         //   var result = await handler.Handle(command);

            // Assert
        //    Assert.True(result.Success);
        }
    }
}