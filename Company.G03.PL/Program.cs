using Company.G03.BLL.Interfaces;
using Company.G03.BLL.Repersitorties;
using Company.G03.DAL.Data.Contexts;
using Company.G03.PL.Services;
using Microsoft.EntityFrameworkCore;

namespace Company.G03.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow Dependency injection 
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
                        
            builder.Services.AddDbContext<CompanyDbContext>(
      options => {
          options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
      });

            // Life Time
            // forms of dependence injection
            //builder.Services.AddScoped(); // Create object Life Time Per Request - UnReachable object
            //builder.Services.AddTransient(); // Create object life time per operations 
            //builder.Services.AddSingleton(); // Create object life time per app

            builder.Services.AddScoped<IScopedService, ScopedService>();
            builder.Services.AddTransient<ITransientService, TransientService>();
            builder.Services.AddSingleton<ISingletonService, SingletonService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
