using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using z5.ms.common.abstractions;

namespace z5.ms.common.controllers
{
    /// <inheritdoc />
    /// <summary>Get release version of the service</summary>
    [Route("/version")]
    public class VersionController : Controller
    {
        /// <summary>
        /// Get the current API version
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="500">
        /// The API produced an unexpected error. This could happen on every API call and is thus not added
        /// everywhere.
        /// </response>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(Error), 500)]
        public JsonResult GetVersion()
        {
            var versionInfo = Assembly.GetEntryAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
            return Json(versionInfo);
        }
    }
}