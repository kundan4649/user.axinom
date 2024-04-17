using System.Threading.Tasks;
using AutoMapper;
using z5.ms.domain.user.customer;
using z5.ms.domain.user.datamodels;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;
using z5.ms.common.abstractions;
using MediatR;
using Microsoft.Extensions.Options;
using z5.ms.common.infrastructure.geoip;

namespace z5.ms.infrastructure.user.customer
{
    /// <summary>Handler for creating a user by email address command</summary>
    public class CreateCustomerMobileCommandHandler : IAsyncRequestHandler<CreateCustomerMobileCommand, Result<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UserServiceOptions _options;
        private readonly IMapper _mapper;
        private readonly IGeoIpService _geoIpService;
        
        /// <inheritdoc />
        public CreateCustomerMobileCommandHandler(ICustomerRepository customerRepository, IMapper mapper,
            IOptions<UserServiceOptions> options, IGeoIpService geoIpService)
        {
            _geoIpService = geoIpService;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _options = options.Value;
        }
        
        /// <inheritdoc />
        public async Task<Result<Customer>> Handle(CreateCustomerMobileCommand message)
        {
            var customer = _mapper.Map<CreateCustomerMobileCommand, CustomerCreate>(message,
                opts => opts.AfterMap((src, dest) => dest.System = _options.DefaultSystemType));
                
            var country = _geoIpService.LookupCountry(message.IpAddress);
            customer.RegistrationCountry = country.CountryCode;
            customer.RegistrationRegion = country.State;

            return await _customerRepository.CreateCustomer(customer);
        }
    }
}