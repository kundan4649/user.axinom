using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using z5.ms.common.abstractions;
using z5.ms.domain.user.datamodels;

namespace z5.ms.infrastructure.user.user
{
    public interface IHipiHandler
    {
        Task<Result<Success>> PUTDataToHipiEndPoint(string command, string userId);
    }

    public class HipiHandler : IHipiHandler
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly HipiEndpointSettings _hipiEndpointSettings;

        public HipiHandler(IHttpClientFactory httpClientFactory, IOptions<HipiEndpointSettings> hipiEndpointSettings)
        {
            _httpClientFactory = httpClientFactory;
            _hipiEndpointSettings = hipiEndpointSettings.Value;
        }

        public async Task<Result<Success>> PUTDataToHipiEndPoint(string command, string userId)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var hipiEndpointSettings = _hipiEndpointSettings.EndPointDetails.Where(i => i.HttpMethod == "PUT").FirstOrDefault();

                using (var request = new HttpRequestMessage(new HttpMethod(hipiEndpointSettings.HttpMethod), hipiEndpointSettings.Endpoint.Replace("{UserId}", userId)))
                {
                    request.Headers.TryAddWithoutValidation("x-api-key", _hipiEndpointSettings.Headers.XApiKey);

                    var multipartContent = new MultipartFormDataContent
                    {
                        { new StringContent(command), "creator" }
                    };
                    request.Content = multipartContent;

                    var response = await httpClient.SendAsync(request);

                    return response.IsSuccessStatusCode ? new Result<Success>() : Result<Success>.FromError(new Error { Message = response.ReasonPhrase, Code = Convert.ToInt32(response.StatusCode) });
                }
            }
        }
    }
}