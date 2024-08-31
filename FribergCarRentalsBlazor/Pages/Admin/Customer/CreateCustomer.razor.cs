using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin.Customer
{
	/// <summary>
	/// The page component class for creating customers.
	/// </summary>
	public partial class CreateCustomer : AdminPageComponentBase
	{
        #region Constants

        /// <summary>
        /// The key for the ID of the customer that was created.
        /// </summary>
        public const string CreatedCustomerIdTempDataKey = "AdminCreatedCustomerId";

		/// <summary>
		/// The url template for the page. 
		/// </summary>
		private const string PageUrlTemplate = "/admin/customer/create";

		#endregion

		#region Fields

		/// <summary>
		/// A collection of validation errors returned from the API.
		/// </summary>
		private List<MessageViewModel> _apiValidationErrors = new();

		#endregion

		#region Properties

		/// <summary>
		/// The injected Auto Mapper service. 
		/// </summary>
		[Inject]
		private IMapper AutoMapper { get; set; } = default!;

		/// <summary>
		/// The injected admin customer API service.
		/// </summary>
		[Inject]
		private IAdminCustomerApiService AdminCustomerApiService { get; set; } = default!;

		/// <summary>
		/// The view model used for customer creation.
		/// </summary>
		[SupplyParameterFromForm]
		private RegisterCustomerViewModel CreateCustomerViewModel { get; set; } = new RegisterCustomerViewModel();

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
		/// Creates a customer
		/// </summary>
		/// <returns>A <see cref="Task"/> that represents an async operation.</returns>
		private async Task OnCreateCustomer()
		{
			var result = await AdminCustomerApiService.CreateCustomerAsync(AutoMapper.Map<CreateCustomerDto>(CreateCustomerViewModel));

			if (result.Success)
			{
				await SessionStorageService.SetItemAsStringAsync(ListCustomers.CreatedCustomerIdStorageDataKey, result.Value!.CustomerId.ToString());
				NavigationManager.NavigateTo(ListCustomers.GetPageUrl());
			}
			else
			{
				_apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.ErrorMessage, title: x.ErrorType)).ToList();
			}
		}

		#endregion
	}
}
