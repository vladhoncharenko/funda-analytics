﻿using AutoMapper;
using PartnerApiModels.DTOs;
using PartnerApiModels.Models;

namespace PartnerApi.Mappers
{
    /// <summary>
    /// Class extends AutoMapper's Profile to define custom mapping configurations.
    /// </summary>
    public class CustomDtoMapper : Profile
    {
        /// <summary>
        /// Constructor to configure mappings.
        /// </summary>
        public CustomDtoMapper()
        {
            CreateMap<PropertyListingDto, PropertyListing>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Adres))
                .ForMember(dest => dest.FundaId, opt => opt.MapFrom(src => src.InternalId))
                .ForMember(dest => dest.HasBalconyOrTerrace, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.BalkonDakterras)))
                .ForMember(dest => dest.HasGarden, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Tuin)))
                .ForMember(dest => dest.HasGarage, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Garage)))
                .ForMember(dest => dest.AddedDateTime, opt => opt.MapFrom(src => ConvertJsonTimestampToDateTime(src.PublicatieDatum)))
                .ForMember(dest => dest.Broker, opt => opt.MapFrom(src => new RealEstateBroker
                {
                    Name = src.Makelaar,
                    FundaId = src.MakelaarId,
                    PhoneNumber = src.MakelaarTelefoon,
                }));

            CreateMap<PropertyListingIdsDto, PropertyListingIds>()
                .ForMember(dest => dest.TotalAmountOfListings, opt => opt.MapFrom(src => src.TotaalAantalObjecten))
                .ForMember(dest => dest.PropertyListingInfo, opt => opt.MapFrom(src =>
                    src.Objects.Select(w => new PropertyListingInfo
                    {
                        Id = w.Id,
                        AddedDateTime = ConvertJsonTimestampToDateTime(w.PublicatieDatum),
                    })))
                .ForMember(dest => dest.PagingInfo, opt => opt.MapFrom(src => new PagingInfo
                {
                    TotalPages = src.Paging.AantalPaginas,
                    CurrentPage = src.Paging.HuidigePagina,
                }));
        }

        /// <summary>
        /// Converts a JSON timestamp to a DateTime instance.
        /// </summary>
        /// <param name="jsonTimestamp">The JSON timestamp string.</param>
        /// <returns>The corresponding DateTime instance.</returns>
        private DateTime ConvertJsonTimestampToDateTime(string jsonTimestamp)
        {
            var startPos = jsonTimestamp.IndexOf("(", StringComparison.Ordinal) + 1;
            var endPos = jsonTimestamp.IndexOf(")", StringComparison.Ordinal);
            var timestampPart = jsonTimestamp.Substring(startPos, endPos - startPos);
            var parts = timestampPart.Split('+');
            var milliseconds = long.Parse(parts[0]);
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var cetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            dateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, cetTimeZone);

            return dateTime.AddMilliseconds(milliseconds);
        }
    }
}