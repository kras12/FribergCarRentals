using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using MvcRazorPages.Shared.Data;
using MvcRazorPages.Shared.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Message;

namespace FribergCarRentals.Areas.Admin.Pages.Customer
{
    /// <summary>
    /// Page model for listing customers in the admin back office.
    /// </summary>
    public class ListModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Customer/List";

        #endregion

        #region Fields

        /// <summary>
        /// The injected customer repository.
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

		// The injected Auto Mapper.
		private readonly IMapper _mapper;

		#endregion

		#region Constructors

		/// <summary>
		/// A constructor.
		/// </summary>
		/// <param name="customerRepository">Injected customer repository.</param>
		/// <param name="authorizationService">The injected authorization service.</param>
		/// <param name="signInManager">The injected signin manager.</param>
		/// <param name="mapper">The injected Auto Mapper.</param>
		public ListModel(ICustomerRepository customerRepository, IAuthorizationService authorizationService,
			SignInManager<ApplicationUser> signInManager, IMapper mapper) : base(authorizationService, signInManager)
		{
			_customerRepository = customerRepository;
			_mapper = mapper;
		}

		#endregion

		#region Properties

		/// <summary>
		/// A view model used to present a list of customers. 
		/// </summary>
		public ListViewModel<CustomerViewModel> CustomerListViewModel { get; private set; } = new();

        #endregion

        #region HandlerMethods

        /// <summary>
        /// Handler for GET Requests. 
        /// </summary>
        /// <returns><see cref="Task{TResult}"/> containing an <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, area: Area));
            }

            CustomerListViewModel = new ListViewModel<CustomerViewModel>(_mapper.Map<List<CustomerViewModel>>(await _customerRepository.GetAllAsync()));
            TempDataHelper.Set(TempData, DeleteModel.RedirectToPageAfterDeleteTempDataKey, new RedirectToPageData("List", area: Area));

            if (TempDataHelper.TryGet(TempData, DeleteModel.DeletedCustomerIdTempDataKey, out int deletedCustomerId))
            {
                CustomerListViewModel.Messages.Add(MessageViewModelHelper.CreateCustomerDeletionSuccessMessage(deletedCustomerId));
            }            

            return Page();
        }

        #endregion
    }
}
