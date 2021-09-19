using AutoMapper;
using DAL;
using WebCase.Areas.Basic.ViewModels;

namespace WebCase.Mappings
{

    //AutoMap 7.0.1.0
    public class ServiceMappings : Profile
    {
        public ServiceMappings()
        {

            CreateMap<Customers, CustomersVM>().ReverseMap();
         
            /*
           cfg.CreateMap<Employee, EmployeeDto>()
        .ForMember("EmployeeID", opt => opt.MapFrom(src => src.ID))
        .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.JoinYear, opt => opt.MapFrom(src => src.JoinTime.Year));
                .ReverseMap(); 
             */
        }

    }
}