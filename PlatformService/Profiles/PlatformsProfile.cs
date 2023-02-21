using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<PlatformCreateDTO, Platform>();
            CreateMap<PlatformReadDTO, PlatformPublishedDTO>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(gpm => gpm.PlatformId, options =>
                {
                    options.MapFrom(p => p.Id);
                })
                .ForMember(gpm => gpm.Name, options =>
                {
                    options.MapFrom(p => p.Name);
                })
                .ForMember(gpm => gpm.Publisher, options =>
                {
                    options.MapFrom(p => p.Publisher);
                });
        }
    }
}