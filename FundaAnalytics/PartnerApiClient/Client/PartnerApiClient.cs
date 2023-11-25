using Microsoft.Extensions.Logging;
using PartnerApi.Mappers;
using PartnerApi.Models;
using PartnerApi.Models.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace PartnerApi.Client
{
    public class PartnerApiClient : IPartnerApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory = null;
        private readonly ILogger<PartnerApiClient> _logger = null;

        private readonly string _partnerApiBaseUrl = "https://partnerapi.funda.nl/feeds/Aanbod.svc/json";
        private readonly string _getPropertyListingUrlTemplate = "/detail/{0}/koop/{1}/";
        private readonly string _getPropertyListingIdsUrlTemplate = "/{0}/?type=koop&zo=/amsterdam/sorteer-datum-af/&page={1}&pagesize={2}";
        private readonly string _apiKey = "ac1b0b1572524640a0ecc54de453ea9f";

        public PartnerApiClient(IHttpClientFactory httpClientFactory, ILogger<PartnerApiClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<PropertyListing?> GetPropertyListingAsync(string propertyFundaId)
        {
            using var httpClient = _httpClientFactory.CreateClient("HttpClient");

            try
            {
                var requestUrl = $"{_partnerApiBaseUrl}{string.Format(_getPropertyListingUrlTemplate, _apiKey, propertyFundaId)}";
                var propertyListingDto = await httpClient.GetFromJsonAsync<PropertyListingDto>(requestUrl, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                var propertyListing = ObjectMapper.Mapper.Map<PropertyListing>(propertyListingDto);

                return propertyListing;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during getting property listing details: {Error}", ex);
            }

            return null;
        }

        public async Task<PropertyListingIds> GetPropertyListingIdsAsync(HttpClient httpClient, int page, int pageSize = 25)
        {
            try
            {
                var requestUrl = $"{_partnerApiBaseUrl}{string.Format(_getPropertyListingIdsUrlTemplate, _apiKey, page, pageSize)}";
                var propertyListingIdsDto = await httpClient.GetFromJsonAsync<PropertyListingIdsDto>(requestUrl, new JsonSerializerOptions(JsonSerializerDefaults.Web));
                var propertyListingIds = ObjectMapper.Mapper.Map<PropertyListingIds>(propertyListingIdsDto);

                return propertyListingIds;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error during getting property listing IDs: {Error}", ex);
            }

            return null;
        }
    }
}