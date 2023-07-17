using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sklepix.Data;
using Sklepix.Repositories;

namespace Sklepix
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = "Server=localhost;Database=Sklepix;Trusted_Connection=True;TrustServerCertificate=True;";
            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<IdentityContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<CategoriesRepository>();
            builder.Services.AddTransient<AislesRepository>();
            builder.Services.AddTransient<AisleRowsRepository>();
            builder.Services.AddTransient<ProductsRepository>();
            builder.Services.AddDbContext<AppDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
                pattern: "{controller=Home}/{action=Index}/{id?}/{secondId?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}