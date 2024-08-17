using Blazored.LocalStorage;
using Blazored.SessionStorage;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Mapping.AutoMapper;
using FribergCarRentalsBlazor.Services.Authentication;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi;
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
            builder.Services.AddAutoMapper(typeof(ViewModelToDtoAutoMapperProfile), typeof(DtoToViewModelAutoMapperProfile));

            // ==================================================================================================================
            // Network (API Service, data transfers)
            // ==================================================================================================================
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			// Add API services with typed http clients
			builder.Services.AddHttpClient<ICustomerApiService, CustomerApiService>(client =>
			{
				client.BaseAddress = new Uri(builder.Configuration["FribergCarRentalsApiBaseUrl"]!);
			});

            builder.Services.AddHttpClient<ICustomerOrderApiService, CustomerOrderApiService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["FribergCarRentalsApiBaseUrl"]!);
            });

            // Add API services with typed http clients
            builder.Services.AddHttpClient<IAdminApiService, AdminApiService>(client =>
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
