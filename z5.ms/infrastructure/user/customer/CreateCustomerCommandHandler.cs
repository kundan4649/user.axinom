using System.Threading.Tasks;
using AutoMapper;
using z5.ms.infrastructure.user.repositories;
using MediatR;
using z5.ms.common.abstractions;
using z5.ms.domain.user.customer;
using z5.ms.domain.user.viewmodels;

namespace z5.ms.infrastructure.user.customer
{
    /// <summary>Handler for creating a customer command</summary>
    public class CreateCustomerCommandHandler : IAsyncRequestHandler<CreateCustomerCommand, Result<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        
        /// <inheritdoc />
        public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        
        /// <inheritdoc />
        public async Task<Result<Customer>> Handle(CreateCustomerCommand message)
        {
            var customerCreate = _mapper.Map<CreateCustomerCommand, CustomerCreate>(message);
            return await _customerRepository.CreateCustomer(customerCreate);
        }
    }
}