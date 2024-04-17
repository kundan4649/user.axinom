using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.domain.user;
using z5.ms.domain.user.customer;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.customer;
using z5.ms.infrastructure.user.repositories;

namespace customer
{
    /// <summary>Tests for get customer query handler</summary>
    public class GetCustomerQueryHandlerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepository = new Mock<ICustomerRepository>();
        private readonly IOptions<UserErrors> _options;
        
        /// <inheritdoc />
        public GetCustomerQueryHandlerTests()
        {
            _options = Options.Create(new UserErrors());
        }

        /// <summary>Ensure that handler returns customer with valid query</summary>
        [Fact]
        public async Task GetCustomerQuery_Success()
        {
            // Arrange
            var command = new GetCustomerQuery{CustomerId = Guid.NewGuid()};
            
            _customerRepository.Setup(r => r.GetCustomer(It.IsAny<Guid>(),false))
                .ReturnsAsync(new Result<Customer> {Success = true});
            
            var handler = new GetCustomerQueryHandler(_customerRepository.Object, _options);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
    }
}