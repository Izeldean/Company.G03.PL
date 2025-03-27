using Company.G03.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.G03.DAL.Data.Contexts
{
    public class CompanyDbContext: IdentityDbContext<AppUser>
    {
        public DbSet<Department> Departments { get; set; }
       
        public DbSet<Employee> Employees { get; set; }
   
        //public DbSet<IdentityUser<int>> IdentityUser { get; set; }

        

      
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server= .; Database=CompanyG03_P ; Trusted_Connection=True; TrustServerCertificate= True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
           
            
        }
    }
}
