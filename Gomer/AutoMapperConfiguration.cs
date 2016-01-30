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
            Mapper.CreateMap<PileModel, PileDto>();

            Mapper.CreateMap<ListModel, ListDto>();
            Mapper.CreateMap<PlatformModel, PlatformDto>();
            Mapper.CreateMap<StatusModel, StatusDto>();

            Mapper.CreateMap<GameModel, GameDto>();

            // DTO -> Model
            Mapper.CreateMap<ListDto, ListModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            Mapper.CreateMap<PlatformDto, PlatformModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            Mapper.CreateMap<StatusDto, StatusModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            Mapper.CreateMap<GameDto, GameModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.List, opt => opt.Ignore())
                .ForMember(dest => dest.Platform, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            Mapper.CreateMap<PileDto, PileModel>()
                .ForMember(dest => dest.Games, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    foreach (var gameDto in src.Games)
                    {
                        var game = Mapper.Map<GameModel>(gameDto);
                        game.List = dest.Lists.FirstOrDefault(l => l.Name == gameDto.ListName);
                        game.Platform = dest.Platforms.FirstOrDefault(p => p.Name == gameDto.PlatformName);
                        game.Status = dest.Statuses.FirstOrDefault(s => s.Name == gameDto.StatusName);

                        dest.Games.Add(game);
                    }
                });

            Mapper.AssertConfigurationIsValid();
        }
    }
}
