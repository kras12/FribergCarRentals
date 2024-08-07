using FribergCarRentals.Shared.Models.Dto.Api;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.Dto.User;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi;
using Microsoft.AspNetCore.Components.Authorization;

namespace FribergCarRentalsBlazor.Services.Authentication
{
	/// <summary>
	/// Authentication service for customers.
	/// </summary>
	public class CustomerAuthenticationService : ICustomerAuthenticationService
	{
		#region Fields

		/// <summary>
		/// The injected authentication state provider.
		/// </summary>
		private readonly AuthenticationStateProvider _authenticationStateProvider;

		/// <summary>
		/// The injected customer API service.
		/// </summary>
		private readonly ICustomerApiService _customerApiService;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="authenticationStateProvider">The injected authentication state provider.</param>
		/// <param name="customerApiService">The injected customer API service.</param>
		public CustomerAuthenticationService(AuthenticationStateProvider authenticationStateProvider, ICustomerApiService customerApiService)
		{
			_authenticationStateProvider = authenticationStateProvider;
			_customerApiService = customerApiService;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Attempts to login a customer.
		/// </summary>
		/// <param name="loginDto">The login credentials.</param>
		/// <returns>An <see cref="ApiValueResponseDto{T}"/> containing a <see cref="LoginUserResponseDto"/> object if successful.</returns>
		public async Task<ApiValueResponseDto<LoginUserResponseDto>> Login(LoginCustomerDto loginDto)
		{
			var response = await _customerApiService.LoginCustomer(loginDto);

			if (response.Success)
			{
				await ((ApiUserAuthenticationStateProvider)_authenticationStateProvider).SetTokenAsync(response.Value!.Token);
			}

			return response;
		}

		/// <summary>
		/// Logs out the customer.
		/// </summary>
		/// <returns>A <see cref="Task"/>.</returns>
		public async Task Logout()
		{
			await ((ApiUserAuthenticationStateProvider)_authenticationStateProvider).RemoveTokenAsync();
		}

		#endregion
	}
}
