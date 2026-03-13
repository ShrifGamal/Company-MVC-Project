using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        public IEmployeeRepositorie EmployeeRepositorie {get;}
        public IDepartmentRepositorie DepartmentRepositorie {get;}

        Task<int> CompleteAsync();
    }
}
