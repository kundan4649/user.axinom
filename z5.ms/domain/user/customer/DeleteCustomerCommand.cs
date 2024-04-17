using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.customer
{
    /// <summary>Deleting customer by customer Id</summary>
    public class DeleteCustomerCommand : CustomerCommandBase {}

    /// <summary>Validator for deleting customer command</summary>
    public class DeleteCustomerCommandValidator : CustomerCommandValidator<DeleteCustomerCommand>
    {
        /// <inheritdoc />
        public DeleteCustomerCommandValidator(IOptions<UserErrors> errors) : base(errors)
        {
        }
    }
}