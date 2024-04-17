namespace z5.ms.common.validation.authproviders
{
    /// <inheritdoc />
    /// <summary>Internal authentication from CRM</summary>
    public class CrmSignedAuthProvider : SecretSignedAuthProvider
    {
        /// <inheritdoc />
        public CrmSignedAuthProvider() : base("CrmApiSecret", "CrmSigningKey")
        {
        }
    }
}