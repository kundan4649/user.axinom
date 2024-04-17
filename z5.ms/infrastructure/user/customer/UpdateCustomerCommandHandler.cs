using System.Threading.Tasks;
using AutoMapper;
using z5.ms.domain.user.customer;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;
using z5.ms.common.abstractions;
using MediatR;

namespace z5.ms.infrastructure.user.customer
{
    /// <summary>Handler for updating customer fields command</summary>
    public class UpdateCustomerCommandHandler : IAsyncRequestHandler<UpdateCustomerCommand, Result<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        
        /// <inheritdoc />
        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        
        /// <inheritdoc />
        public async Task<Result<Customer>> Handle(UpdateCustomerCommand message)
        {
            return await _customerRepository.UpdateCustomer(_mapper.Map<UpdateCustomerCommand, CustomerChangeDetails>(message));
        }
    }
}