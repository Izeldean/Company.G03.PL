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

        public int Add(T department)
        {
           _context.Set<T>().Add(department);
            return _context.SaveChanges();
        }

        public int Delete(T department)
          
        {
            _context.Set<T>().Remove(department);
          return _context.SaveChanges();
        }

        public T? Get(int id)
        {
            //if (typeof(T) == typeof(Employee))
            //{
            //    return (IEnumerable<T>)_context.Employees.Include(E => E.Department).FirstOrDefault(E => E.Id ==id) as T;
            //}
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Employee)) { 
            return (IEnumerable<T>) _context.Employees.Include(A=> A.Department).ToList();
            }
          return _context.Set<T>().ToList();
        }

        public int Update(T department)
        {
            _context.Set<T>().Update(department);
            return _context.SaveChanges();
        }
    }
}
