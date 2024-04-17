//using System.Threading.Tasks;
//using AutoMapper;
//using Microsoft.Extensions.Options;
//using Moq;
//using Xunit;
//using z5.ms.common.abstractions;
//using z5.ms.domain.user.customer;
//using z5.ms.domain.user.datamodels;
//using z5.ms.domain.user.viewmodels;
//using z5.ms.infrastructure.user.customer;
//using z5.ms.infrastructure.user.repositories;

//namespace customer
//{
//    /// <summary>Tests for create customer with mobile number command handler</summary>
//    public class CreateCustomerMobileCommandHandlerTests
//    {
//        private readonly IMapper _mapper;
//        private readonly Mock<ICustomerRepository> _customerRepository = new Mock<ICustomerRepository>();
//        private readonly Mock<IUserRepository> _userRepository = new Mock<IUserRepository>();
//        private readonly IOptions<UserServiceOptions> _options;
        
//        /// <inheritdoc />
//        public CreateCustomerMobileCommandHandlerTests()
//        {
//            _mapper = new MapperConfiguration(m => m.AddProfile<CustomerProfile>()).CreateMapper();
//            _options = Options.Create(new UserServiceOptions());
//        }

//        /// <summary>Ensure that handler returns success with successful insert</summary>
//        [Fact]
//        public async Task CreateCustomerMobileCommand_Success()
//        {
//            // Arrange
//            var command = new CreateCustomerMobileCommand();

//            _userRepository.Setup(r => r.GetCountryInfo(It.IsAny<string>())).ReturnsAsync(() => ("Country", "State"));
            
//            _customerRepository.Setup(r => r.CreateCustomer(It.IsAny<CustomerCreate>()))
//                .ReturnsAsync(new Result<Customer> {Success = true});
            
//            var handler = new CreateCustomerMobileCommandHandler(_customerRepository.Object, _mapper, _options, _userRepository.Object);

//            // Act
//            var result = await handler.Handle(command);

//            // Assert
//            Assert.True(result.Success);
//        }
//    }
//}