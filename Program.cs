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

            var connectionString = "Server=localhost;Database=Sklepix;Trusted_Connection=True;TrustServerCertificate=True;";
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<CategoriesRepository>();
            builder.Services.AddTransient<AislesRepository>();
            builder.Services.AddTransient<AisleRowsRepository>();
            builder.Services.AddTransient<ProductsRepository>();
            builder.Services.AddDbContext<AppDbContext>();

            var app = builder.Build();

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