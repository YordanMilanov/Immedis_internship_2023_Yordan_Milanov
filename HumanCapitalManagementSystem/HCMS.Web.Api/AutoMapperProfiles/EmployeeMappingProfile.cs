using AutoMapper;
using HCMS.Common.Structures;
using HCMS.Data.Models;
using HCMS.Services.ServiceModels.Employee;

namespace HCMS.Services.AutoMapperProfiles
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => new Name(src.FirstName)))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => new Name(src.LastName)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => new Phone(src.PhoneNumber)))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new LocationStruct(src.Location!.Address, src.Location.State, src.Location.Country)));

            CreateMap<EmployeeDto, Employee>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.ToString()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToString()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.ToString()))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new Location {Address = src.Location.GetAddress(),State = src.Location.GetState(), Country = src.Location.GetCountry()}));
        }
    }
}
