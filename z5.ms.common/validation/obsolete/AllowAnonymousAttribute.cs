using System;

namespace z5.ms.common.validation.obsolete
{
    /// <inheritdoc />
    /// <summary>
    /// Mark method or controller to allow access if no Authorization header is supplied
    /// </summary>
    [Obsolete("Use AuthorizeAttribute with AnonymousAuthProvider and another auth provider")]
    public class AllowAnonymousAttribute : Attribute
    {
    }
}