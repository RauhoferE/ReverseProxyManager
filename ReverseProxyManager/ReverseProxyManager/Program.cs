using ReverseProxyManager.MappingProfiles;
using ReverseProxyManager.Services;
using ReverseProxyManager.Settings;
using Serilog;
using FluentValidation;
using ReverseProxyManager.Validation;
using System.Reflection;
using ReverseProxyManager.Middleware;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ReverseProxyManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // Add services to the container.
            builder.Services.AddControllers();

            // Settings
            builder.Services.Configure<UserSettings>(builder.Configuration.GetSection("UserSettings"));

            // Automapper
            builder.Services.AddAutoMapper(x =>
            {
                x.AddProfile<DomainToDtoProfile>();
            });

            // Middleware
            builder.Services.AddMvc(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });

            builder.Services.AddControllers(x =>
            {
                x.Filters.Add<HttpResponseExceptionFilter>();
            });

            // Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReverseProxyPolicy",
                    builder => builder.WithOrigins(configuration.GetSection("AllowedCorsHosts").Get<string[]>())
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials());
            });

            // Db
            builder.Services.AddDbContext<ReverseProxyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                });

            // Authorization
            builder.Services.AddAuthorization(options =>
            {
            });

            //Validation
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Services
            builder.Services.AddTransient<IFileService, FileService>();
            builder.Services.AddTransient<ICertificationService, CertificationService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IManagementService, ManagementService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
