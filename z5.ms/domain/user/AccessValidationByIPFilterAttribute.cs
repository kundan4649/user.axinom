using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using z5.ms.common.extensions;
using z5.ms.domain.user.datamodels;
using z5.ms.infrastructure.user.repositories;

namespace z5.ms.domain.user
{
    public class AccessValidationByIPFilterAttribute : IAsyncActionFilter
    {
        IAccessValidationByIPRepository AccessValidationByIPRepository;
        AccessValidationByIPSettings _accessValidationByIPSettings;

        public AccessValidationByIPFilterAttribute(IAccessValidationByIPRepository accessValidationByIPRepository, IOptions<AccessValidationByIPSettings> accessValidationByIPSettings)
        {
            AccessValidationByIPRepository = accessValidationByIPRepository;
            _accessValidationByIPSettings = accessValidationByIPSettings.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var badRequest = false;

            var requestEndPoint = context.HttpContext.Request.Path;
            var ipAddress = context.HttpContext.Request.GetRemoteIp();

            var result = await AccessValidationByIPRepository.GetItemAsync(ipAddress, requestEndPoint);

            if (result.Value != null)
            {
                var isWithinTheTimeFrame = (DateTime.UtcNow - result.Value.LastRequestReceivedDate).TotalMinutes < _accessValidationByIPSettings.NextAccessAfter;
                var resetCount = false;

                if (result.Value.RequestCount > _accessValidationByIPSettings.MaxRequestCount)
                {
                    if (isWithinTheTimeFrame)
                        badRequest = true;
                    else
                        resetCount = true;
                }

                await AccessValidationByIPRepository.UpdateItemAsync(result.Value, resetCount, badRequest);
            }
            else
                await AccessValidationByIPRepository.AddItemAsync(new viewmodels.AccessValidationByIPItem { IpAddress = ipAddress, RequestCount = 1, RequestEndPoint = requestEndPoint });

            if (badRequest)
            {
                //need to work on next available after timeslot
                //context.Result = new BadRequestObjectResult($"Further requests to this resource from this source will be allowed only after { (DateTime.UtcNow - (result.Value.RequestBlockedDate == null ? DateTime.UtcNow.AddMinutes(-1 * _accessValidationByIPSettings.NextAccessAfter) : result.Value.RequestBlockedDate)).Minutes.ToString() } Minutes");
                //context.Result = new BadRequestObjectResult($"Further requests to this resource from this source will be allowed only after { _accessValidationByIPSettings.NextAccessAfter - (!result.Value.RequestBlockedDate.HasValue ? 0 : (DateTime.UtcNow - result.Value.RequestBlockedDate.Value).Minutes) } Minutes");
                context.Result = new BadRequestObjectResult($"Further requests to this resource from this source will be allowed only after sometime");

                return;
            }

            await next();
        }
    }
}