using Sklepix.Data;
using Sklepix.Repositories;

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}/{secondId?}");

            app.Run();
        }
    }
}