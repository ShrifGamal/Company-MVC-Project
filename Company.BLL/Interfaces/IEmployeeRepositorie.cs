using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
    public interface IEmployeeRepositorie : IGenericRepositorie<Employee>
    {
        Task<IEnumerable<Employee>>GetByNameAsync(string name);
        //IEnumerable<Employee> GetAll();
        //Employee Get(int Id);
        //int Add(Employee entity);
        //int Delete(Employee entity);
        //int Update(Employee entity);
    }
}
