using AutoMapper;
using Company.G03.DAL.Models;
using Company.G03.PL.Dtos;

namespace Company.G03.PL.Mapping
{
    //CLR
    public class EmployeeProfile: Profile
    {
        public EmployeeProfile() { 
        CreateMap<CreateEmployeedto, Employee>();

            /*
             * 
              CreateMap<CreateEmployeedto, Employee>().Reverse();
             
             */


            /* 
             // used when CreateEmployeedto instance variable name is different 
                        // from instance variables in the Employee class 
                        // CreateMap<CreateEmployeedto, Employee>().ForMember( d => d.Name, o => o.MapFrom(s => s.EmpName ))


             */

            CreateMap< Employee, CreateEmployeedto>();

        }
    }
}
