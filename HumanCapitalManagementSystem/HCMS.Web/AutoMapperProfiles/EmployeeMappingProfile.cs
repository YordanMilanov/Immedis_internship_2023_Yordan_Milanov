namespace HCMS.Web.AutoMapperProfiles
{
    using AutoMapper;
    using HCMS.Common.Structures;
    using HCMS.Services.ServiceModels;
    using HCMS.Web.ViewModels.Employee;

    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            //Mapping
            CreateMap<EmployeeFormModel, EmployeeDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => new Name(src.FirstName)))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => new Name(src.LastName)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email (src.Email)))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => new Phone (src.PhoneNumber)))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => new Photo (src.PhotoUrl)))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.AddDate, opt => opt.MapFrom(src => src.AddDate))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Location (src.Address, src.State, src.Country)));

            //Reverse mapping
            CreateMap<EmployeeDto, EmployeeFormModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.PhotoUrl))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.AddDate, opt => opt.MapFrom(src => src.AddDate))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.GetAddress()))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Location.GetState()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location.GetCountry()));
        }
    }
}
