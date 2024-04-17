using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.HealthChecks;
using z5.ms.common.abstractions;
using z5.ms.common.healthcheck;

namespace z5.ms.userservice.controllers
{
    /// <inheritdoc />
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CheckController : Controller
    {
        private readonly IHealthCheckService _healthCheck;

        /// <inheritdoc />
        public CheckController(IHealthCheckService healthCheck)
        {
            _healthCheck = healthCheck;
        }

        /// <summary>
        /// Get the list of all the checks that the checker can execute.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">List of checks</response>
        /// <response code="500">The API produced an unexpected error. This could happen on every API call and is thus not added everywhere. </response>
        [HttpGet]
        [Route("/api/checks")]
        public virtual IActionResult GetChecks()
        {
            return Json(_healthCheck.GetAllChecks().Select(a => new CheckItem { Name = a.Name, Title = a.GetDescription()}).ToList());
        }

        /// <summary>
        /// Execute specified check and get the result.
        /// </summary>
        /// <param name="checkName"></param>
        /// <returns></returns>
        /// <response code="200">Check passed</response>
        /// <response code="520">Check failed</response>
        /// <response code="404">Check type not found</response>
        /// <response code="500">The API produced an unexpected error. This could happen on every API call and is thus not added everywhere. </response>
        [HttpPost]
        [Route("/api/checks/{check_name}/execute")]
        public virtual async Task<IActionResult> ExecuteCheck([FromRoute(Name = "check_name")]string checkName)
        {
            try
            {
                var check = _healthCheck.GetCheck(checkName);
                var result = await _healthCheck.RunCheckAsync(check);

                return new JsonResult(result.CheckStatus == CheckStatus.Healthy
                        ? new ExecuteCheckResponse
                        {
                            Check = checkName,
                            Result = "Success",
                            Message = result.Description
                        }
                        : new ExecuteCheckResponse
                         {
                             Check = checkName,
                             Result = "Failed",
                             Message = result.Description
                         }){ StatusCode = result.CheckStatus == CheckStatus.Healthy ? 200 : 520 };
            }
            catch (KeyNotFoundException)
            {
                return new JsonResult(new ExecuteCheckResponse
                {
                    Check = checkName,
                    Result = "Failed",
                    Message = "Check type not found"
                })
                { StatusCode = 404 };
            }
        }
    }
}
