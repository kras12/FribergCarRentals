using FribergCarRentals.DataAccess.DatabaseContexts;
using FribergCarRentals.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using FribergCars.Shared.Settings;
using MvcRazorPages.Shared.ModelBinders;

namespace FribergCarRentals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Pages with model binders
            builder.Services.AddRazorPages().AddMvcOptions(options => options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider()));

            // DB Context
            builder.Services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString(AppSettingsHelper.ApplicationDbContextConnectionStringKey)));
            
            // Repositories
            builder.Services.AddTransient<ICarRepository, CarRepository>();
            builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
            builder.Services.AddTransient<ICarOrderRepository, CarOrderRepository>();
            builder.Services.AddTransient<IAdminRepository, AdminRepository>();
            builder.Services.AddTransient<ICarCategoryRepository, CarCategoryRepository>();

            // Sessions
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }

            app.Run();
        }
    }
}
