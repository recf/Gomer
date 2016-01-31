using System.Linq;
using AutoMapper;
using Gomer.DataAccess.Dto;
using Gomer.Models;

namespace Gomer
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            // Model -> DTO
            Mapper.CreateMap<ListModel, ListDto>();
            Mapper.CreateMap<PlatformModel, PlatformDto>();
            Mapper.CreateMap<StatusModel, StatusDto>();

            Mapper.CreateMap<GameModel, GameDto>()
                .ForMember(dest => dest.PlatformNames, opt => opt.MapFrom(src => src.Platforms.Select(p => p.Name)));

            // DTO -> Model
            Mapper.CreateMap<ListDto, ListModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.GameCount, opt => opt.Ignore());

            Mapper.CreateMap<PlatformDto, PlatformModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            Mapper.CreateMap<StatusDto, StatusModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            Mapper.CreateMap<GameDto, GameModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.List, opt => opt.Ignore())
                .ForMember(dest => dest.Platforms, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            Mapper.AssertConfigurationIsValid();
        }
    }
}
