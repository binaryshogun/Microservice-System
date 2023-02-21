using AutoMapper;
using CommandsService.DTOs;
using CommandsService.Models;
using PlatformService;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<Command, CommandReadDTO>();
            CreateMap<CommandCreateDTO, Command>();
            CreateMap<PlatformPublishedDTO, Platform>()
                .ForMember(p => p.PlatformId, options => options.MapFrom(src => src.Id))
                .ForMember(p => p.Id, options => options.Ignore());
            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(p => p.PlatformId, options => options.MapFrom(gpm => gpm.PlatformId))
                .ForMember(p => p.Name, options => options.MapFrom(gpm => gpm.Name))
                .ForMember(p => p.Commands, options => options.Ignore());
        }
    }
}