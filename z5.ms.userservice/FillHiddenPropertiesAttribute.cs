using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using z5.ms.common.extensions;
using z5.ms.common.infrastructure.geoip;
using z5.ms.domain.user.user;

namespace z5.ms.userservice
{
    /// <summary>Attribute to fill additional registration parameters</summary>
    public class FillHiddenPropertiesAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (context.ActionArguments?.Values?.FirstOrDefault(a => a is RegisterUserCommand) is RegisterUserCommand register)
                    FillRegisterValues(register, context.HttpContext.RequestServices, context.HttpContext.Request);
                if (context.ActionArguments?.Values?.FirstOrDefault(a => a is LoginUserCommand) is LoginUserCommand login)
                    FillLoginValues(login, context.HttpContext.RequestServices, context.HttpContext.Request);
            }
            catch (Exception ex)
            {
                GetLogger(context.HttpContext.RequestServices).LogWarning("Unable to fill additional fields for registration", ex);
            }
            base.OnActionExecuting(context);
        }

        private void FillRegisterValues(RegisterUserCommand register, IServiceProvider services, HttpRequest request)
        {
            // Get IP address if it's not provided as input parameter
            register.IpAddress = string.IsNullOrWhiteSpace(register.IpAddress) ? request.GetRemoteIp() : register.IpAddress;
            
            // Get country code and state using Country endpoint (maxmind) from subscription service if it's not provided
            if (string.IsNullOrWhiteSpace(register.RegistrationCountry))
            {
                var country = GetCountry(services, register.IpAddress);
                register.RegistrationCountry = country.CountryCode;
                register.RegistrationRegion = country.State;
            }
            
            register.Additional = register.Additional ?? new JObject();

            AddToJsonIfNotPresent(request, register.Additional, "original_user_agent", "User-Agent");
            AddToJsonIfNotPresent(request, register.Additional, "X-Forwarded-For", "X-Forwarded-For");
            AddToJsonIfNotPresent(request, register.Additional, "True-Client-IP", "True-Client-IP");
        }

        private static void AddToJsonIfNotPresent(HttpRequest request, JObject json, string jsonKey, string requestHeader)
        {
            if (string.IsNullOrWhiteSpace(json.GetValue(jsonKey)?.ToString())
                && request.TryGetHeader(requestHeader, out var headerResult))
                json.Add(jsonKey, headerResult);
        }

        private void FillLoginValues(LoginUserCommand login, IServiceProvider services, HttpRequest request)
        {
           login.Country = GetCountry(services, request.GetRemoteIp()).CountryCode;
        }

        private Country GetCountry(IServiceProvider services, string ip) 
            => services.GetService<IGeoIpService>().LookupCountry(ip);
        

        private static ILogger GetLogger(IServiceProvider services)
            => services.GetService<ILogger<FillHiddenPropertiesAttribute>>();
    }
}