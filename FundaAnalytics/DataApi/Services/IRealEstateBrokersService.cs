using PartnerApiModels.DTOs;

namespace DataApi.Services
{
    public interface IRealEstateBrokersService
    {
        Task<IList<RealEstateBrokerInfoDto>> GetRealEstateBrokersInfoAsync();

        Task<RealEstateBrokerInfoDto?> GetRealEstateBrokerInfoAsync(int fundaId);
    }
}