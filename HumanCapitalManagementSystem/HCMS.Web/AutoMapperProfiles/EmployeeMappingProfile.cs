namespace HCMS.Web.AutoMapperProfiles
{
    using AutoMapper;
    using HCMS.Common.Structures;
    using HCMS.Services.ServiceModels.BaseClasses;
    using HCMS.Services.ServiceModels.Employee;
    using HCMS.Web.ViewModels.BaseViewModel;
    using HCMS.Web.ViewModels.Employee;

    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {

            //Mapping
            CreateMap<EmployeeFormModel, EmployeeDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => new Name(src.FirstName)))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => new Name(src.LastName)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => new Phone(src.PhoneNumber)))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.AddDate, opt => opt.MapFrom(src => src.AddDate))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new LocationStruct(src.Address, src.State, src.Country)));

            //Reverse mapping
            CreateMap<EmployeeDto, EmployeeFormModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.ToString()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToString()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.AddDate, opt => opt.MapFrom(src => src.AddDate))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.GetAddress()))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Location.GetState()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location.GetCountry()));

            //Mapping
            CreateMap<EmployeeViewModel, EmployeeDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => new Name(src.FirstName)))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => new Name(src.LastName)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => new Phone(src.PhoneNumber)))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.AddDate, opt => opt.MapFrom(src => src.AddDate))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new LocationStruct(src.Address, src.State, src.Country)));

            //Reverse mapping
            CreateMap<EmployeeDto, EmployeeViewModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.ToString()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToString()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.AddDate, opt => opt.MapFrom(src => src.AddDate))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.GetAddress()))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Location.GetState()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Location.GetCountry()));

            CreateMap<ResultQueryModel<EmployeeViewModel>, QueryDto>();
            CreateMap<QueryDtoResult<EmployeeDto>, ResultQueryModel<EmployeeViewModel>>();
        }
    }
}
