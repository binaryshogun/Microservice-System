using AutoMapper;
using CommandsService.DTOs;
using CommandsService.Models;

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
            CreateMap<PlatformPublishedDTO, Platform>().ForMember(p => p.PlatformId, options =>
            {
                options.MapFrom(src => src.Id);
            }).ForMember(p => p.Id, options => options.Ignore());
        }
    }
}