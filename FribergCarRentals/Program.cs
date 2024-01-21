using FribergCarRentals.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Data.ModelBinder;

namespace FribergCarRentals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(new ConnectionStringManager().GetConnectionString(builder.Configuration)));
            builder.Services.AddControllersWithViews(options => options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider()));
            builder.Services.AddTransient<ICarRepository, CarRepository>();
            builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
            builder.Services.AddTransient<ICarOrderRepository, CarOrderRepository>();
            builder.Services.AddTransient<IAdminRepository, AdminRepository>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
