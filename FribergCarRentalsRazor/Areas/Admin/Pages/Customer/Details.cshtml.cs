using Microsoft.AspNetCore.Mvc;
using FribergCarRentals.Data.Repositories;
using FribergCarRentals.Shared.Mvc.Data;
using FribergCarRentals.Shared.Mvc.Helpers;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FribergCarRentals.Shared.Models.ViewModels.Customer;
using AutoMapper;
using FribergCarRentals.Shared.Models.ViewModels.Message;

namespace FribergCarRentals.Areas.Admin.Pages.Customer
{
    /// <summary>
    /// Page model for showing details about customer in the admin back office. 
    /// </summary>
    public class DetailsModel : AdminPageModelBase
    {
        #region Constants

        /// <summary>
        /// The page URL relative to the login page
        /// </summary>
        public const string PageUrlRelativeToLoginPage = "Customer/Details";

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
		/// <param name="mapper"> The injected Auto Mapper.</param>
		public DetailsModel(ICustomerRepository customerRepository, IAuthorizationService authorizationService,
			SignInManager<ApplicationUser> signInManager, IMapper mapper) : base(authorizationService, signInManager)
		{
			_customerRepository = customerRepository;
			_mapper = mapper;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The view model used for presenting customer details. 
		/// </summary>
		public CustomerViewModel CustomerViewModel { get; set; } = default!;

        #endregion

        #region HandlerMethods       

        /// <summary>
        /// Handler for GET requests. 
        /// </summary>
        /// <param name="id">The customer ID.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {            
            if (!await IsAdminLoggedIn())
            {
                return RedirectToLogin(new RedirectToPageData(PageUrlRelativeToLoginPage, new RouteValueDictionary(new { id = id }), area: Area));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var customer = await _customerRepository.GetByIdAsync(id);

                if (customer is not null)
                {
                    CustomerViewModel = _mapper.Map<CustomerViewModel>(customer);

                    if (TempDataHelper.TryGet(TempData, CreateModel.CreatedCustomerIdTempDataKey, out int createdCustomerId))
                    {
                        CustomerViewModel.Messages.Add(MessageViewModelHelper.CreateCustomerCreationSuccessMessage(createdCustomerId));
                    }

                    if (TempDataHelper.TryGet(TempData, ResendConfirmEmailLinkModel.ResentConfirmEmailLinkForCustomerIdTempDataKey, out int resentConfirmEmailLinkCustomerId))
                    {
                        CustomerViewModel.Messages.Add(MessageViewModelHelper.CreateResentConfirmEmailLinkToCustomerSuccessMessage(resentConfirmEmailLinkCustomerId));
                    }

                    return Page();
                }
            }

            throw new Exception($"Failed to show the customer with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        #endregion
    }
}
