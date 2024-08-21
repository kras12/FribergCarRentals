using AutoMapper;
using FribergCarRentals.Shared.Models.Dto.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsBlazor.Pages.Admin.Customer
{
	/// <summary>
	/// The page component class for creating customers.
	/// </summary>
	public partial class Create : AdminPageComponentBase
	{
        #region Constants

        /// <summary>
        /// The key for the ID of the customer that was created.
        /// </summary>
        public const string CreatedCustomerIdTempDataKey = "AdminCreatedCustomerId";

        /// <summary>
        /// The url for the page. 
        /// </summary>
        public const string PageUrl = "/admin/customer/create";

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
		[FromForm]
		private RegisterCustomerViewModel CreateCustomerViewModel { get; set; } = new RegisterCustomerViewModel();

		#endregion

		#region Methods

		/// <summary>
		/// Creates a customer
		/// </summary>
		/// <returns>A <see cref="Task"/> that represents an async operation.</returns>
		private async Task CreateCustomer()
		{
			var result = await AdminCustomerApiService.CreateCustomerAsync(AutoMapper.Map<CreateCustomerDto>(CreateCustomerViewModel));

			if (result.Success)
			{
				await SessionStorageService.SetItemAsStringAsync(List.CreatedCustomerIdStorageDataKey, result.Value!.CustomerId.ToString());
				NavigationManager.NavigateTo(List.PageUrl);
			}
			else
			{
				_apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
			}
		}

		#endregion
	}
}
