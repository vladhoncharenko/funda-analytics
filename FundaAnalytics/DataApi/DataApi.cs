using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DataApi
{
    public class DataApi
    {
        private readonly ILogger _logger;

        public DataApi(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DataApi>();
        }

        [Function("GetAllRealEstateAgents")]
        public HttpResponseData GetAllRealEstateAgents([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RealEstateAgents")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }

        [Function("GetRealEstateAgentById")]
        public HttpResponseData GetRealEstateAgentById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RealEstateAgents/{id}")] HttpRequestData req, int id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
