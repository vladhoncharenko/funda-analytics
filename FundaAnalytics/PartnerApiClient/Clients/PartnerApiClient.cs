﻿using Microsoft.Extensions.Logging;
using PartnerApi.Mappers;
using PartnerApiClient.RateLimiters;
using PartnerApiModels.DTOs;
using PartnerApiModels.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace PartnerApi.Clients
{
    public class PartnerApiClient : IPartnerApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PartnerApiClient> _logger;
        private readonly IRateLimiter _rateLimiter;

        private readonly string _partnerApiBaseUrl;
        private readonly string _getPropertyListingUrlTemplate;
        private readonly string _getPropertyListingIdsUrlTemplate;
        private readonly string _apiKey;

        public PartnerApiClient(IHttpClientFactory httpClientFactory, ILogger<PartnerApiClient> logger, IRateLimiter rateLimiter)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _rateLimiter = rateLimiter;
            _partnerApiBaseUrl = Environment.GetEnvironmentVariable("PartnerApiBaseUrl");
            _getPropertyListingUrlTemplate = Environment.GetEnvironmentVariable("PropertyListingUrlTemplate");
            _getPropertyListingIdsUrlTemplate = Environment.GetEnvironmentVariable("PropertyListingIdsUrlTemplate");
            _apiKey = Environment.GetEnvironmentVariable("ApiKey");
        }

        public async Task<PropertyListing?> GetPropertyListingAsync(string propertyFundaId)
        {
            if (string.IsNullOrWhiteSpace(propertyFundaId))
                throw new ArgumentException($"Invalid {nameof(propertyFundaId)} value.");

            if (await _rateLimiter.ShouldLimitRequestAsync("PartnerApi"))
                return null;

            try
            {
                using var httpClient = _httpClientFactory.CreateClient("HttpClient");
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

        public async Task<PropertyListingIds?> GetPropertyListingIdsAsync(int page, int pageSize = 25)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 25;

            if (await _rateLimiter.ShouldLimitRequestAsync("PartnerApi"))
                return null;

            try
            {
                using var httpClient = _httpClientFactory.CreateClient("HttpClient");
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