using System.Threading.Tasks;
using z5.ms.domain.user;
using z5.ms.domain.user.customer;
using z5.ms.domain.user.viewmodels;
using z5.ms.infrastructure.user.repositories;
using z5.ms.common.abstractions;
using MediatR;
using Microsoft.Extensions.Options;

namespace z5.ms.infrastructure.user.customer
{
    /// <summary>Handler for get customer query</summary>
    public class GetCustomerQueryHandler : IAsyncRequestHandler<GetCustomerQuery, Result<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UserErrors _errors;
        
        /// <inheritdoc />
        public GetCustomerQueryHandler (ICustomerRepository customerRepository, IOptions<UserErrors> errors)
        {
            _customerRepository = customerRepository;
            _errors = errors.Value;
        }
        
        /// <inheritdoc />
        public async Task<Result<Customer>> Handle(GetCustomerQuery message)
        {
            if (message.CustomerId == null)
                return Result<Customer>.FromError(_errors.MissingCustomerId);
            
            return await _customerRepository.GetCustomer(message.CustomerId.Value);
        }
    }
}