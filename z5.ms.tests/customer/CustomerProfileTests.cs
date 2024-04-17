//using AutoMapper;
//using Xunit;
//using z5.ms.infrastructure.user.customer;

//namespace customer
//{
//    /// <summary>Tests to verify that CustomerProfile mapper profile works as expected</summary>
//    public class CustomerProfileTests
//    {
//        private readonly IMapper _mapper;

//        /// <inheritdoc />
//        public CustomerProfileTests()
//        {
//            _mapper = new MapperConfiguration(m => m.AddProfile<CustomerProfile>()).CreateMapper();
//        }

//        [Fact]
//        public void ValidateCustomerProfile()
//        {
//            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
//        }
//    }
//}