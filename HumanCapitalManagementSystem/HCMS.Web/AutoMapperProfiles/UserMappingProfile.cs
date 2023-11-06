﻿using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Web.ViewModels.User;
using HCMS.Services.ServiceModels.User;

namespace HCMS.Web.AutoMapperProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRegisterFormModel, UserRegisterDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => new Name(src.Username)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => new Password(src.Password)));

            CreateMap<UserRegisterDto, UserRegisterFormModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToString()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.ToString()));

            CreateMap<UserLoginFormModel, UserLoginDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => new Name(src.Username)))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => new Password(src.Password)));

            CreateMap<UserLoginDto, UserLoginFormModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.ToString()))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.ToString()));

            CreateMap<UserViewDto, UserViewModel>().ReverseMap();
            CreateMap<UserUpdateFormModel, UserUpdateDto>().ReverseMap();
            CreateMap<UserPasswordFormModel, UserPasswordDto>();
        }
    }
}
