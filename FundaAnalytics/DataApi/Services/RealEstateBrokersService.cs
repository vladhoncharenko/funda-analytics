using CacheClient.Services;
using DataApi.Enums;
using Microsoft.Extensions.Logging;
using PartnerApiModels.DTOs;
using PartnerApiModels.Models;

namespace DataApi.Services
{
    public class RealEstateBrokersService : IRealEstateBrokersService
    {
        private readonly ILogger<RealEstateBrokersService> _logger;
        private readonly IJsonCacheService _cacheService;

        public RealEstateBrokersService(ILogger<RealEstateBrokersService> logger, IJsonCacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<RealEstateBrokerInfoDto?> GetRealEstateBrokerInfoAsync(int fundaId)
        {
            var propertyListings = await _cacheService.GetJsonDataAsync<IDictionary<string, PropertyListing>>("PropertyListings", "$");

            var propertyListingsGroupedByBroker = propertyListings
                .Values.Where(pl => pl != null)
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
            var propertyListings = await _cacheService.GetJsonDataAsync<IDictionary<string, PropertyListing>>("PropertyListings", "$");

            var propertyListingsGroupedByBroker = propertyListings
                .Values.Where(pl => pl != null)
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

        public async Task<IList<RealEstateBrokerInfoDto>?> GetTopRealEstateBrokersInfoAsync(TopRealEstateBrokersCategoryEnum topRealEstateBrokersCategoryEnum)
        {
            var realEstateBrokersList = await GetRealEstateBrokersInfoAsync();

            Func<RealEstateBrokerInfoDto, int> keySelector = topRealEstateBrokersCategoryEnum switch
            {
                TopRealEstateBrokersCategoryEnum.TotalAmountOfPropertyListings => broker => broker.TotalAmountOfHomes,
                TopRealEstateBrokersCategoryEnum.PropertyListingsWithGarden => broker => broker.AmountOfHomesWithGarden,
                TopRealEstateBrokersCategoryEnum.PropertyListingsWithBalconyOrTerrace => broker => broker.AmountOfHomesWithBalconyOrTerrace,
                TopRealEstateBrokersCategoryEnum.PropertyListingsWithGarage => broker => broker.AmountOfHomesWithGarage,
                _ => broker => broker.TotalAmountOfHomes
            };

            return realEstateBrokersList.OrderByDescending(keySelector).Take(20).ToList();
        }
    }
}
