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

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // Adds MVC services to the container

            // Register the DapperDbContext for the database connection (using Dapper)
            builder.Services.AddScoped<DapperDbContext>(); // Register Dapper DbContext as Scoped

            // Register repositories (using interfaces and implementations)
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // Register repository with interface and implementation

            // Add Session Services (distributed memory cache and session configuration)
            builder.Services.AddDistributedMemoryCache(); // Store session data in memory
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
                options.Cookie.HttpOnly = true; // Security setting
                options.Cookie.IsEssential = true; // Allow session even if user doesn't accept cookies
            });

            // Build the app
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Show detailed exception page in development
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Generic error handling in production
                app.UseHsts(); // HTTP Strict Transport Security (HSTS)
            }

            // Enable HTTPS redirection
            app.UseHttpsRedirection();

            // Enable static files (CSS, JS, Images, etc.)
            app.UseStaticFiles();

            // Enable routing
            app.UseRouting();

            // Enable session middleware
            app.UseSession(); // Enable session functionality

            // Enable authorization (if necessary)
            app.UseAuthorization();

            // Map controller routes (default route)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Employee}/{action=Signup}/{id?}");

            // Run the application
            app.Run();
        }
    }
}
