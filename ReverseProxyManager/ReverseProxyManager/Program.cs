using ReverseProxyManager.MappingProfiles;
using ReverseProxyManager.Services;
using ReverseProxyManager.Settings;
using Serilog;
using System.Reflection;
using ReverseProxyManager.Middleware;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Core.Helpers;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;

namespace ReverseProxyManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create required system folders
            FolderHelper.CreateSystemFolders();

            var builder = WebApplication.CreateBuilder(args);

            var environment = builder.Environment.EnvironmentName;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(builder.Configuration));

            Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

            builder.Services.AddSerilog();

            // Add services to the container.
            builder.Services.AddControllers();

            // Settings
            // Get the values from the env variables like in docker
            var username = builder.Configuration["Username"];
            var password = builder.Configuration["Password"];
            builder.Services.AddSingleton<UserSettings>(new UserSettings()
            {
                Username = username ?? "test",
                Password = password ?? "password"
            });

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
            var dbConnection = FolderHelper.GetSqliteFilePath();
            builder.Services.AddDbContext<ReverseProxyDbContext>(options =>
                options.UseSqlite($"Data Source={dbConnection}"));

            // Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            context.Response.StatusCode = 401; // Unauthorized
                            return Task.CompletedTask;
                        },
                        OnRedirectToAccessDenied = context =>
                        {
                            context.Response.StatusCode = 403; // Forbidden
                            return Task.CompletedTask;
                        }
                    };
                    options.Cookie.HttpOnly = true;
                    if (environment == "Development")
                    {
                        options.Cookie.SameSite = SameSiteMode.None;
                    }
                    
                });

            // Authorization
            builder.Services.AddAuthorization(options =>
            {
            });

            builder.Services.AddControllers(x =>
            {
                x.Filters.Add<HttpResponseExceptionFilter>();
            });

            // Services
            builder.Services.AddTransient<IFileService, FileService>();
            builder.Services.AddTransient<IProcessService, ProcessService>();
            builder.Services.AddTransient<ICertificationService, CertificationService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IManagementService, ManagementService>();
            

            var app = builder.Build();

            //  Ensure Db Created and migrations applied
            CreateDbAndApplyMigration(app);

            app.UseCors("ReverseProxyPolicy");
            app.UseDefaultFiles();
            app.UseStaticFiles();


            // Configure the HTTP request pipeline.
            //if (!app.Environment.IsDevelopment())
            //{
            //    //app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            //
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }

        private static void CreateDbAndApplyMigration(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>(); // For logging
                try
                {
                    var context = services.GetRequiredService<ReverseProxyDbContext>();

                    // Optional: Implement retry logic for database connection readiness
                    var maxRetries = 5;
                    var retryDelay = TimeSpan.FromSeconds(10); // Wait 10 seconds between retries

                    for (int i = 0; i < maxRetries; i++)
                    {
                        try
                        {
                            logger.LogInformation($"Attempting to apply migrations (attempt {i + 1}/{maxRetries})...");
                            context.Database.Migrate();
                            logger.LogInformation("Database migrations applied successfully.");
                            break; // Exit loop on success
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Error applying migrations. Retrying in {retryDelay.TotalSeconds} seconds...");
                            if (i < maxRetries - 1)
                            {
                                Thread.Sleep(retryDelay); // Wait before retrying
                            }
                            else
                            {
                                logger.LogCritical("Failed to apply migrations after multiple retries. Application will not start.");
                                throw; // Re-throw if all retries fail, stopping the app
                            }
                        }
                    }

                    // Optional: Seed initial data after migrations
                    // SeedData.Initialize(services);

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during database migration or seeding.");
                    // Depending on your environment, you might want to throw here to prevent the app from starting
                    // if the database isn't ready. For production, this is often a good idea.
                    throw;
                }
            }

        }
    }
}
