using Blazored.LocalStorage;
using Blazored.SessionStorage;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.AutoMapper;
using FribergCarRentals.Shared.Models.Blazor.AutoMapper;
using FribergCarRentalsBlazor.Services.Authentication;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.DemoApi;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace FribergCarRentalsBlazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // ==================================================================================================================
            // Mapping
            // ==================================================================================================================
            builder.Services.AddAutoMapper(
                typeof(FribergCarRentals.Shared.Models.AutoMapper.ViewModelToDtoAutoMapperProfile),
                typeof(FribergCarRentals.Shared.Models.Blazor.AutoMapper.ViewModelToDtoAutoMapperProfile),
                typeof(FribergCarRentals.Shared.Models.AutoMapper.DtoToViewModelAutoMapperProfile),
                typeof(FribergCarRentals.Shared.Models.Blazor.AutoMapper.DtoToViewModelAutoMapperProfile)
            );

            // ==================================================================================================================
            // Network (API Service, data transfers)
            // ==================================================================================================================
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			// Add API services with typed http clients
			builder.Services.AddHttpClient<ICustomerApiService, CustomerApiService>(client =>
			{
				client.BaseAddress = new Uri(builder.Configuration["FribergCarRentalsApiBaseUrl"]!);
			});

			// Add API services with typed http clients
			builder.Services.AddHttpClient<ICustomerOrderApiService, CustomerOrderApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["FribergCarRentalsApiBaseUrl"]!);
            });

            // Add API services with typed http clients
            builder.Services.AddHttpClient<IAdminApiService, AdminApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["FribergCarRentalsApiBaseUrl"]!);
            });

            // Add API services with typed http clients
            builder.Services.AddHttpClient<IAdminCarApiService, AdminCarApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["FribergCarRentalsApiBaseUrl"]!);
            });

            // Add API services with typed http clients
            builder.Services.AddHttpClient<IAdminCarCategoryApiService, AdminCarCategoryApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["FribergCarRentalsApiBaseUrl"]!);
            });

			// Add API services with typed http clients
			builder.Services.AddHttpClient<IAdminCustomerApiService, AdminCustomerApiService>(client =>
			{
				client.BaseAddress = new Uri(builder.Configuration["FribergCarRentalsApiBaseUrl"]!);
			});

            // Add API services with typed http clients
            builder.Services.AddHttpClient<IAdminOrderApiService, AdminOrderApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["FribergCarRentalsApiBaseUrl"]!);
            });

            // Add API services with typed http clients
            builder.Services.AddHttpClient<IDemoApiService, DemoApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["FribergCarRentalsApiBaseUrl"]!);
            });

            // ==================================================================================================================
            // Security (authentication, authorization)
            // ==================================================================================================================
            builder.Services.AddAuthorizationCore(options =>
            {
				options.AddPolicy(ApplicationUserPolicies.Admin, policy =>
					policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.Admin));

				options.AddPolicy(ApplicationUserPolicies.Customer, policy =>
					policy.RequireClaim(ApplicationUserClaims.UserRole, ApplicationUserRoles.Customer));
			});

			builder.Services.AddScoped<AuthenticationStateProvider, ApiUserAuthenticationStateProvider>();
            builder.Services.AddScoped<IApiUserAuthenticationStateProvider, ApiUserAuthenticationStateProvider>();
            builder.Services.AddTransient<ICustomerAuthenticationService, CustomerAuthenticationService>();
            builder.Services.AddTransient<IAdminAuthenticationService, AdminAuthenticationService>();

			// ==================================================================================================================
			// Storage
			// ==================================================================================================================
			builder.Services.AddBlazoredLocalStorage();
			builder.Services.AddBlazoredSessionStorage();

			await builder.Build().RunAsync();
        }
    }
}
