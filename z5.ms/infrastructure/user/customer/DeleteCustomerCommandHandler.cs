using System.Threading.Tasks;
using z5.ms.domain.user;
using z5.ms.domain.user.customer;
using z5.ms.infrastructure.user.repositories;
using z5.ms.common.abstractions;
using MediatR;
using Microsoft.Extensions.Options;

namespace z5.ms.infrastructure.user.customer
{
    /// <summary>Handler for delete customer command</summary>
    public class DeleteCustomerCommandHandler : IAsyncRequestHandler<DeleteCustomerCommand, Result<Success>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UserErrors _errors;
        
        /// <inheritdoc />
        public DeleteCustomerCommandHandler (ICustomerRepository customerRepository, IOptions<UserErrors> errors)
        {
            _customerRepository = customerRepository;
            _errors = errors.Value;
        }
        
        /// <inheritdoc />
        public async Task<Result<Success>> Handle(DeleteCustomerCommand message)
        {
            if (message.CustomerId == null)
                return Result<Success>.FromError(_errors.MissingCustomerId);
            
            return await _customerRepository.DeleteCustomer(message.CustomerId.Value);
        }
    }
}