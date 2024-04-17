namespace z5.ms.common.validation.authproviders
{
    /// <inheritdoc />
    /// <summary>Internal authentication from Billdesk</summary>
    public class BilldeskAuthProvider : SecretAuthProvider
    {
        /// <inheritdoc />
        public BilldeskAuthProvider() : base("billdesk", "BilldeskAuthToken")
        {
        }
    }

    /// <inheritdoc />
    /// <summary>Internal authentication from Billdesk</summary>
    public class BilldeskAuthAttribute : AuthorizeAttribute
    {
        /// <inheritdoc />
        public BilldeskAuthAttribute() : base(typeof(BilldeskAuthProvider))
        {
        }
    }
}
