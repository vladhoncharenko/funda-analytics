using AutoMapper;

namespace PartnerApi.Mappers
{
    public class ObjectMapper
    {
        public static IMapper Mapper => Lazy.Value;

        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod != null && (p.GetMethod.IsPublic || p.GetMethod.IsAssembly);
                cfg.AddProfile<CustomDtoMapper>();
            });

            return config.CreateMapper();
        });
    }
}