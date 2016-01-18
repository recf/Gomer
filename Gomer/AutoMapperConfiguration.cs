using System.Linq;
using AutoMapper;
using Gomer.Dto;
using Gomer.Models;

namespace Gomer
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            // Model -> Self
            Mapper.CreateMap<ListModel, ListModel>();
            Mapper.CreateMap<GameModel, GameModel>();

            // Model -> DTO
            Mapper.CreateMap<PileModel, PileDto>();
            Mapper.CreateMap<ListModel, ListDto>();
            Mapper.CreateMap<GameModel, GameDto>();

            // DTO -> Model
            Mapper.CreateMap<ListDto, ListModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            Mapper.CreateMap<GameDto, GameModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.List, opt => opt.Ignore());

            Mapper.CreateMap<PileDto, PileModel>()
                .ForMember(dest => dest.Games, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    foreach (var gameDto in src.Games)
                    {
                        var game = Mapper.Map<GameModel>(gameDto);
                        game.List = dest.Lists.FirstOrDefault(l => l.Name == gameDto.ListName);
                        dest.Games.Add(game);
                    }
                });

            Mapper.AssertConfigurationIsValid();
        }
    }
}
