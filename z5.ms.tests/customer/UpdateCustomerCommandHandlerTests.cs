//using System;
//using System.Threading.Tasks;
//using AutoMapper;
//using Moq;
//using Xunit;
//using z5.ms.common.abstractions;
//using z5.ms.domain.user.customer;
//using z5.ms.domain.user.viewmodels;
//using z5.ms.infrastructure.user.customer;
//using z5.ms.infrastructure.user.repositories;

//namespace customer
//{
//    /// <summary>Tests for updating customer command handler</summary>
//    public class UpdateCustomerCommandHandlerTests
//    {
//        private readonly Mock<ICustomerRepository> _customerRepository = new Mock<ICustomerRepository>();
//        private readonly IMapper _mapper;
        
//        /// <inheritdoc />
//        public UpdateCustomerCommandHandlerTests()
//        {
//            _mapper = new MapperConfiguration(m => m.AddProfile<CustomerProfile>()).CreateMapper();
//        }

//        /// <summary>Ensure that handler returns success with update customer command</summary>
//        [Fact]
//        public async Task UpdateCustomerCommand_Success()
//        {
//            // Arrange
//            var command = new UpdateCustomerCommand {Id = Guid.NewGuid()};
            
//            _customerRepository.Setup(r => r.UpdateCustomer(It.IsAny<CustomerChangeDetails>()))
//                .ReturnsAsync(new Result<Customer> {Success = true});
            
//            var handler = new UpdateCustomerCommandHandler(_customerRepository.Object, _mapper);

//            // Act
//            var result = await handler.Handle(command);

//            // Assert
//            Assert.True(result.Success);
//        }
//    }
//}