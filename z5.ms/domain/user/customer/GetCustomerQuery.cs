using Microsoft.Extensions.Options;

namespace z5.ms.domain.user.customer
{
    /// <summary>Query customer by customer Id</summary>
    public class GetCustomerQuery : CustomerQueryBase{ }
    
    /// <summary>Validator for get customer by Id query</summary>
    public class GetCustomerQueryValidator : CustomerQueryBaseValidator<GetCustomerQuery>
    {
        /// <inheritdoc />
        public GetCustomerQueryValidator(IOptions<UserErrors> errors) : base(errors)
        {
            
        }
    }
    
}