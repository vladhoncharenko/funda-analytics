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
            var propertyListings = await _cacheService.GetDataAsync<IDictionary<string, PropertyListing>>("PropertyListings", "$");

            var propertyListingsGroupedByBroker = propertyListings
                .Values
                .GroupBy(pl => pl.Broker);

            var propertyBrokerListings = propertyListingsGroupedByBroker.Where(s => s.Key.FundaId == fundaId);

            var propertyBrokerListingsDto = propertyBrokerListings.Select(kvp => new RealEstateBrokerInfoDto
            {
                FundaId = kvp.Key.FundaId,
                Name = kvp.Key.Name,
                PhoneNumber = kvp.Key.PhoneNumber,
                AmountOfHomesWithGarden = kvp.Count(pl => pl.HasGarden),
                AmountOfHomesWithBalconyOrTerrace = kvp.Count(pl => pl.HasBalconyOrTerrace),
                AmountOfHomesWithGarage = kvp.Count(pl => pl.HasGarage),
                TotalAmountOfHomes = kvp.Count(),
                PropertyListings = kvp.ToList()
            }).FirstOrDefault();

            return propertyBrokerListingsDto;
        }

        public async Task<IList<RealEstateBrokerInfoDto>> GetRealEstateBrokersInfoAsync()
        {
            var propertyListings = await _cacheService.GetDataAsync<IDictionary<string, PropertyListing>>("PropertyListings", "$");

            var propertyListingsGroupedByBroker = propertyListings
                .Values
                .GroupBy(pl => pl.Broker);

            var realEstateBrokersList = propertyListingsGroupedByBroker
                .Select(kvp => new RealEstateBrokerInfoDto
                {
                    FundaId = kvp.Key.FundaId,
                    Name = kvp.Key.Name,
                    PhoneNumber = kvp.Key.PhoneNumber,
                    AmountOfHomesWithGarden = kvp.Count(pl => pl.HasGarden),
                    AmountOfHomesWithBalconyOrTerrace = kvp.Count(pl => pl.HasBalconyOrTerrace),
                    AmountOfHomesWithGarage = kvp.Count(pl => pl.HasGarage),
                    TotalAmountOfHomes = kvp.Count()
                }).ToList();

            return realEstateBrokersList;
        }
    }
}
