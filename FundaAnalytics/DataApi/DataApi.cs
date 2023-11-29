using DataApi.Enums;
using DataApi.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PartnerApiModels.Utils;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataApi
{
    /// <summary>
    /// Represents an Azure Functions API for handling real estate brokers data.
    /// API contains some duplications in the code, but it is done for consuming by the OpenAI Assistant.
    /// </summary>
    public class DataApi
    {
        private readonly ILogger<DataApi> _logger;
        private readonly IRealEstateBrokersService _realEstateBrokersService;

        public DataApi(ILogger<DataApi> logger, IRealEstateBrokersService realEstateBrokersService)
        {
            _logger = logger;
            _realEstateBrokersService = realEstateBrokersService;
        }

        /// <summary>
        /// Azure Function to retrieve information about all real estate brokers.
        /// </summary>
        [Function("GetAllRealEstateBrokers")]
        public async Task<HttpResponseData> GetAllRealEstateBrokers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetAllRealEstateBrokers")] HttpRequestData req)
        {
            try
            {
                var realEstateBrokersInfoAsync = await _realEstateBrokersService.GetRealEstateBrokersInfoAsync();
                if (!realEstateBrokersInfoAsync.Any())
                    return req.CreateResponse(HttpStatusCode.NoContent);

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/json; charset=utf-8");

                await response.WriteStringAsync(JsonSerializer.Serialize(realEstateBrokersInfoAsync, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }));

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting all real estate brokers: {error}", e);

                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Azure Function to retrieve information about a specific real estate broker by ID.
        /// </summary>
        [Function("GetRealEstateBrokerById")]
        public async Task<HttpResponseData> GetRealEstateBrokerByIdAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetRealEstateBrokerById/{id}")] HttpRequestData req, int id)
        {
            try
            {
                if (id <= 0)
                    return req.CreateResponse(HttpStatusCode.BadRequest);

                var realEstateBrokerInfoAsync = await _realEstateBrokersService.GetRealEstateBrokerInfoAsync(id);
                if (realEstateBrokerInfoAsync == null)
                    return req.CreateResponse(HttpStatusCode.NoContent);

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/json; charset=utf-8");

                var options = new JsonSerializerOptions();
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.Converters.Add(new PropertyListingConverter());
                await response.WriteStringAsync(JsonSerializer.Serialize(realEstateBrokerInfoAsync, options));

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting real estate broker by ID: {error}", e);

                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Azure Function to retrieve information about top real estate brokers with the most amount of homes.
        /// </summary>
        [Function("GetTopRealEstateBrokersWithTheMostAmountOfHomes")]
        public async Task<HttpResponseData> GetTopRealEstateBrokersWithTheMostAmountOfHomes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetTopRealEstateBrokersWithTheMostAmountOfHomes")] HttpRequestData req)
        {
            return await GetTopRealEstateBrokersByCategory(TopRealEstateBrokersCategoryEnum.TotalAmountOfPropertyListings, req);
        }

        /// <summary>
        /// Azure Function to retrieve information about top real estate brokers with the most amount of homes with a garden.
        /// </summary>
        [Function("GetTopRealEstateBrokersWithTheMostAmountOfHomesWithGarden")]
        public async Task<HttpResponseData> GetTopRealEstateBrokersWithTheMostAmountOfHomesWithGarden([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetTopRealEstateBrokersWithTheMostAmountOfHomesWithGarden")] HttpRequestData req)
        {
            return await GetTopRealEstateBrokersByCategory(TopRealEstateBrokersCategoryEnum.PropertyListingsWithGarden, req);
        }

        /// <summary>
        /// Azure Function to retrieve information about top real estate brokers with the most amount of homes with a balcony or terrace.
        /// </summary>
        [Function("GetTopRealEstateBrokersWithTheMostAmountOfHomesWithBalconyOrTerrace")]
        public async Task<HttpResponseData> GetTopRealEstateBrokersWithTheMostAmountOfHomesWithBalconyOrTerrace([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetTopRealEstateBrokersWithTheMostAmountOfHomesWithBalconyOrTerrace")] HttpRequestData req)
        {
            return await GetTopRealEstateBrokersByCategory(TopRealEstateBrokersCategoryEnum.PropertyListingsWithBalconyOrTerrace, req);
        }

        /// <summary>
        /// Azure Function to retrieve information about top real estate brokers with the most amount of homes with a garage.
        /// </summary>
        [Function("GetTopRealEstateBrokersWithTheMostAmountOfHomesWithGarage")]
        public async Task<HttpResponseData> GetTopRealEstateBrokersWithTheMostAmountOfHomesWithGarage([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetTopRealEstateBrokersWithTheMostAmountOfHomesWithGarage")] HttpRequestData req)
        {
            return await GetTopRealEstateBrokersByCategory(TopRealEstateBrokersCategoryEnum.PropertyListingsWithGarage, req);
        }

        /// <summary>
        /// Common method to retrieve information about top real estate brokers by a specified category.
        /// API contains some duplications in the code, but it is done for consuming by the OpenAI Assistant.
        /// </summary>
        private async Task<HttpResponseData> GetTopRealEstateBrokersByCategory(TopRealEstateBrokersCategoryEnum topRealEstateBrokersCategoryEnum, HttpRequestData req)
        {
            try
            {
                var realEstateBrokersInfoAsync = await _realEstateBrokersService.GetTopRealEstateBrokersInfoAsync(topRealEstateBrokersCategoryEnum);
                if (realEstateBrokersInfoAsync == null)
                    return req.CreateResponse(HttpStatusCode.NoContent);

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/json; charset=utf-8");

                await response.WriteStringAsync(JsonSerializer.Serialize(realEstateBrokersInfoAsync, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }));

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting real estate brokers by category: {error}", e);

                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
