using CacheClient.Services;
using Microsoft.Extensions.Logging;
using PartnerApiModels.DTOs;
using PartnerApiModels.Models;

namespace DataApi.Services
{
    public class RealEstateBrokersService : IRealEstateBrokersService
    {
        private readonly ILogger<RealEstateBrokersService> _logger;
        private readonly ICacheService _cacheService;

        public RealEstateBrokersService(ILogger<RealEstateBrokersService> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<RealEstateBrokerInfoDto> GetRealEstateBrokerInfoAsync(int fundaId)
        {
            var realEstateBrokersList = await GetRealEstateBrokersInfoAsync();
            var realEstateBrokerInfo = realEstateBrokersList.FirstOrDefault(reb => reb.FundaId.Equals(fundaId));

            return realEstateBrokerInfo;
        }

        public async Task<IList<RealEstateBrokerInfoDto>> GetRealEstateBrokersInfoAsync()
        {
            var propertyListings = await _cacheService.GetDataAsync<IDictionary<string, PropertyListing>>("PropertyListings", "$");

            var propertyListingsGroupedByBroker = propertyListings
                .Values
                .GroupBy(pl => pl.Broker)
                .ToDictionary(group => group.Key, group => group);

            var realEstateBrokersList = propertyListingsGroupedByBroker
                .Select(kvp => new RealEstateBrokerInfoDto
                {
                    FundaId = kvp.Key.FundaId,
                    Name = kvp.Key.Name,
                    PhoneNumber = kvp.Key.PhoneNumber,
                    AmountOfHomesWithGarden = kvp.Value.Count(pl => pl.HasGarden),
                    AmountOfHomesWithBalconyOrTerrace = kvp.Value.Count(pl => pl.HasBalconyOrTerrace),
                    AmountOfHomesWithGarage = kvp.Value.Count(pl => pl.HasGarage)
                }).ToList();

            return realEstateBrokersList;
        }
    }
}
