using Blazored.LocalStorage;
using Blazored.SessionStorage;
using FribergCarRentals.Shared.Constants;
using FribergFastigheter.Client.Services.FribergFastigheterApi;
using FribergFastigheter.Shared.Constants;
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

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

			// ==================================================================================================================
			// Storage
			// ==================================================================================================================
			builder.Services.AddBlazoredLocalStorage();
			builder.Services.AddBlazoredSessionStorage();

			await builder.Build().RunAsync();
        }
    }
}
