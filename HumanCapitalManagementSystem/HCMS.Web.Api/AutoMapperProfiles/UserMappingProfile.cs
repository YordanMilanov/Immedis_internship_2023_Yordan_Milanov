using AutoMapper;
using HCMS.Data.Models;
using HCMS.Services.ServiceModels.User;

namespace HCMS.Web.Api.AutoMapperProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.RegisterDate))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UsersRoles.Select(ur => ur.Role.Name).ToList()))
                .ForMember(dest => dest.EmployeeId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<User, UserViewDto>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => src.RegisterDate))
           .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UsersRoles.Select(ur => ur.Role.Name).ToList()))
           .ReverseMap();
        }
    }
}
