namespace z5.ms.common.validation.authproviders
{
    /// <inheritdoc />
    /// <summary>Internal authentication from CMS</summary>
    public class CmsInternalAuthProvider : SecretAuthProvider
    {
        /// <inheritdoc />
        public CmsInternalAuthProvider() : base("InternalApiSecret")
        {
        }
    }

    /// <inheritdoc />
    /// <summary>Internal authentication from CMS</summary>
    public class CmsInternalAuthAttribute : AuthorizeAttribute
    {
        /// <inheritdoc />
        public CmsInternalAuthAttribute() : base(typeof(CmsInternalAuthProvider))
        {
        }
    }
}
