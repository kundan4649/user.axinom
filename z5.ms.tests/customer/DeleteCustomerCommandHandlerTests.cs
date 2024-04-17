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
    /// <summary>Tests for delete customer command handler</summary>
    public class DeleteCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepository = new Mock<ICustomerRepository>();
        private readonly IOptions<UserErrors> _options;
        
        /// <inheritdoc />
        public DeleteCustomerCommandHandlerTests()
        {
            _options = Options.Create(new UserErrors());
        }

        /// <summary>Ensure that handler returns success with successful delete</summary>
        [Fact]
        public async Task DeleteCustomerCommand_Success()
        {
            // Arrange
            var command = new DeleteCustomerCommand{CustomerId = Guid.NewGuid()};
            
            _customerRepository.Setup(r => r.DeleteCustomer(It.IsAny<Guid>()))
                .ReturnsAsync(new Result<Success> {Success = true});
            
            var handler = new DeleteCustomerCommandHandler(_customerRepository.Object, _options);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
    }
}