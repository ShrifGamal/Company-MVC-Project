using AutoMapper;
using Company.DAL.Models;
using Company.PL.ViewModels.Employees;

namespace Company.PL.Mapping.Employees
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() 
        {
            CreateMap<Employee , EmployeeViewModel>().ReverseMap();
        }
    }
}
