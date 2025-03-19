using Company.G03.BLL.Interfaces;
using Company.G03.DAL.Data.Contexts;
using Company.G03.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G03.BLL.Repersitorties
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
		private readonly CompanyDbContext _context;

		public EmployeeRepository(CompanyDbContext context): base(context)
        {
			_context = context;
		}

	
		List<Employee>? IEmployeeRepository.GetByName(string name)
		{
			return _context.Employees.Include(E=>E.Department).Where(E => E.Name.ToLower().Contains(name.ToLower())).ToList();

		}
	}
}
