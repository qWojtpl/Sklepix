using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sklepix.Data;
using Sklepix.Repositories;
using SklepixIdentity.Data;

namespace Sklepix
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<CategoriesRepository>();
            builder.Services.AddTransient<AislesRepository>();
            builder.Services.AddTransient<AisleRowsRepository>();
            builder.Services.AddTransient<ProductsRepository>();
            builder.Services.AddDbContext<AppDbContext>();

            var connectionString = "Server=localhost;Database=Sklepix;Trusted_Connection=True;TrustServerCertificate=True;";
            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<IdentityContext>();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}/{secondId?}");
            
            app.MapRazorPages();

            app.Run();
        }
    }
}