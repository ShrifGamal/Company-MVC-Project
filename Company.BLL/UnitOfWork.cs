using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDepartmentRepositorie _departmentRepositorie;
        private IEmployeeRepositorie _employeeRepositorie;
        public UnitOfWork(AppDbContext context)
        {
            _departmentRepositorie = new DepartmentRepositorie(context);
            _employeeRepositorie = new EmployeeRepositorie(context);
            _context = context;
        }
        public IEmployeeRepositorie EmployeeRepositorie => _employeeRepositorie;

        public IDepartmentRepositorie DepartmentRepositorie => _departmentRepositorie;

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
