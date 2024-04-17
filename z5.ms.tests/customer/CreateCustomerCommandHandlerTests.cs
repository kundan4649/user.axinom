using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Xunit;
using z5.ms.common.abstractions;
using z5.ms.domain.user.customer;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.customer;
using z5.ms.infrastructure.user.repositories;

namespace customer
{
    /// <summary>Tests for create customer command hander</summary>
    public class CreateCustomerCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICustomerRepository> _repository = new Mock<ICustomerRepository>();
        
        /// <inheritdoc />
        public CreateCustomerCommandHandlerTests()
        {
            _mapper = new MapperConfiguration(m => m.AddProfile<CustomerProfile>()).CreateMapper();
        }

        /// <summary>Ensure that handler returns success with successful insert</summary>
        [Fact]
        public async Task CreateCustomerCommandHandler_Success()
        {
            // Arrange
            var command = new CreateCustomerCommand {Password = "validPass"};

            _repository.Setup(r => r.CreateCustomer(It.IsAny<CustomerCreate>()))
                .ReturnsAsync(new Result<Customer> {Success = true});
            var handler = new CreateCustomerCommandHandler(_repository.Object, _mapper);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
    }
}