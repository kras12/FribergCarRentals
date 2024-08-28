using AutoMapper;
using Blazored.SessionStorage;
using FribergCarRentals.Shared.Models.Dto.User;
using FribergCarRentalsBlazor.Services.Authentication;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Customer
{
    /// <summary>
    /// Page component for confirming the email account for a customer.
    /// </summary>
    public partial class ConfirmEmail : ComponentBase
    {
		#region Constants

		/// <summary>
		/// The url template for the page. 
		/// </summary>
		private const string PageUrlTemplate = "/customer/confirm-email";

        #endregion

        #region Fields

        /// <summary>
        /// Returns true if the email confirmation failed.
        /// </summary>
        private bool _emailConfirmationFailed = false;

        /// <summary>
        /// Returns true if the query parameters are invalid.
        /// </summary>
        private bool _invalidQueryParameters = false;

        #endregion

        #region InjectedProperties

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        /// <summary>
        /// The injected customer API service.
        /// </summary>
        [Inject]
        private ICustomerApiService CustomerApiService { get; set; } = default!;

        /// <summary>
        /// The injected customer authentication service. 
        /// </summary>
        [Inject]
        private ICustomerAuthenticationService CustomerAuthenticationService { get; set; } = default!;
        /// <summary>
        /// The injected navigation manager. 
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        /// <summary>
        /// The injected session storage service.
        /// </summary>
        [Inject]
        private ISessionStorageService SessionStorageService { get; set; } = default!;

        #endregion

        #region ParameterProperties

        [SupplyParameterFromQuery]
        public string Code { get; set; } = "";

        [SupplyParameterFromQuery]
        public string Email { get; set; } = "";

		#endregion

		#region Methods

		/// <summary>
		/// Gets the page URL.
		/// </summary>
		/// <returns>A <see cref="string"/> that contains the URL of the page.</returns>
		public static string GetPageUrl()
		{
			return PageUrlTemplate;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var uri = new Uri(NavigationManager.Uri);
            var parameters = System.Web.HttpUtility.ParseQueryString(uri.Query);

            string? code = parameters["code"];
            string? email = parameters["email"];

            if (code == null || email == null)
            {
                _invalidQueryParameters = true;
                return;
            }

            ConfirmEmailDto confirmEmailDto = new ConfirmEmailDto(code, email);
            var response = await CustomerApiService.ConfirmEmail(confirmEmailDto);

            if (response.Success)
            {
                await CustomerAuthenticationService.LoginCustomer(response.Value!.Token);

                if (await SessionStorageService.ContainKeyAsync(Authenticate.RedirectUrlStorageKey))
                {
                    string redirectToUrl = await SessionStorageService.GetItemAsStringAsync(Authenticate.RedirectUrlStorageKey);
                    await SessionStorageService.RemoveItemAsync(Authenticate.RedirectUrlStorageKey);
                    NavigationManager.NavigateTo(redirectToUrl);
                }
                else
                {
                    NavigationManager.NavigateTo(CustomerHome.GetPageUrl());
                }
            }
            else
            {
                _emailConfirmationFailed = true;
            }
        }

        #endregion
    }
}
