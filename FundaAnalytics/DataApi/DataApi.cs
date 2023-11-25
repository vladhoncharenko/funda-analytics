using DataApi.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Newtonsoft.Json;

namespace DataApi
{
    public class DataApi
    {
        private readonly ILogger<DataApi> _logger;
        private readonly IRealEstateBrokersService _realEstateBrokersService;

        public DataApi(ILogger<DataApi> logger, IRealEstateBrokersService realEstateBrokersService)
        {
            _logger = logger;
            _realEstateBrokersService = realEstateBrokersService;
        }

        [Function("GetAllRealEstateBrokers")]
        public async Task<HttpResponseData> GetAllRealEstateBrokers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RealEstateBrokers")] HttpRequestData req)
        {
            var realEstateBrokersInfoAsync = await _realEstateBrokersService.GetRealEstateBrokersInfoAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/json; charset=utf-8");
            await response.WriteStringAsync(JsonConvert.SerializeObject(realEstateBrokersInfoAsync));

            return response;
        }

        [Function("GetRealEstateBrokerById")]
        public async Task<HttpResponseData> GetRealEstateBrokerByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RealEstateBrokers/{id}")] HttpRequestData req, int id)
        {
            var realEstateBrokerInfoAsync = await _realEstateBrokersService.GetRealEstateBrokerInfoAsync(id);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/json; charset=utf-8");
            await response.WriteStringAsync(JsonConvert.SerializeObject(realEstateBrokerInfoAsync));

            return response;
        }
    }
}
