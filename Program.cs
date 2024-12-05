using EmployeeManagement.Data;
using EmployeeManagement.Repositories.Implementations;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmployeeManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            builder.Services.AddControllersWithViews(); 

           
            builder.Services.AddScoped<DapperDbContext>(); 

            
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

       
            builder.Services.AddDistributedMemoryCache(); 
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
                options.Cookie.HttpOnly = true; 
                options.Cookie.IsEssential = true; 
            });

            
            var app = builder.Build();

          
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); 
                app.UseHsts(); 
            }

          
            app.UseHttpsRedirection();

           
            app.UseStaticFiles();

            
            app.UseRouting();

         
            app.UseSession(); 

            
            app.UseAuthorization();

            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Employee}/{action=Signup}/{id?}");

          
            app.Run();
        }
    }
}
