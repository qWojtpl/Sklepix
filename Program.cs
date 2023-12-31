using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sklepix.Data;
using Sklepix.Data.Entities;
using Sklepix.Data.Seeds;
using Sklepix.Repositories;

namespace Sklepix
{

    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = "Server=localhost;Database=Sklepix;Trusted_Connection=True;TrustServerCertificate=True;";
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<UserEntity>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
            })
                .AddRoles<RoleEntity>()
                .AddRoleManager<RoleManager<RoleEntity>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<CategoriesRepository>();
            builder.Services.AddTransient<AislesRepository>();
            builder.Services.AddTransient<AisleRowsRepository>();
            builder.Services.AddTransient<ProductsRepository>();
            builder.Services.AddTransient<UsersRepository>();
            builder.Services.AddTransient<TasksRepository>();

            var app = builder.Build();

            var scope = app.Services.CreateScope();
            Task.Run(async () => {
                await RolesSeeder.Seed(scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>());
                await UsersSeeder.Seed(
                    scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>(),
                    scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>()
                );
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}/{secondId?}");

            app.MapRazorPages();

            app.Run();
        }

    }
}