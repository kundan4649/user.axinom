
namespace z5.ms.common.validation.authproviders
{
    /// <inheritdoc />
    /// <summary>Internal authentication from CRM</summary>
    public class CrmAuthProvider : SecretAuthProvider
    {
        /// <inheritdoc />
        public CrmAuthProvider() : base("CrmApiSecret")
        {
        }
    }
}
