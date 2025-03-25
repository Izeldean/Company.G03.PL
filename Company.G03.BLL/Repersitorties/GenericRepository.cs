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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _context;

        public GenericRepository(CompanyDbContext context)
        {
          _context = context;
        }

        public async Task AddAsync(T department)
        {
         await  _context.Set<T>().AddAsync(department);
          
        }

        public void Delete(T department)
          
        {
            _context.Set<T>().Remove(department);
         
        }

        public async Task<T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _context.Employees.Include(E => E.Department).FirstOrDefaultAsync(E => E.Id == id) as T;
            }
            return _context.Set<T>().Find(id);
        }

        // async must be task or void
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee)) { 

            return (IEnumerable<T>) await _context.Employees.Include(A=> A.Department).ToListAsync();
            }
          return await _context.Set<T>().ToListAsync();
        }

        public void Update(T department)
        {
            _context.Set<T>().Update(department);

        }

       
    }
}
