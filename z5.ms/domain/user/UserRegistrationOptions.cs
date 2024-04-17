using z5.ms.common;

namespace z5.ms.domain.user
{
    // TODO: Add options to enable/disable different controllers.
    /// <inheritdoc />
    /// <summary>User module registration options</summary>
    public class UserRegistrationOptions : IComponentRegistrationOptions
    {
        /// <summary>Default registration options</summary>
        public static readonly UserRegistrationOptions Default = new UserRegistrationOptions();
        
        /// <summary>Enables template controllers</summary>
        public bool UseTemplateControllers { get; set; } = true;

        /// <summary>Toogle to enable/disable asycn repositories (eg. WatchHistory)</summary>
        public bool UseAsyncRepositories { get; set; } = false;
    }
}
