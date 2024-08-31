using AutoMapper;
using FribergCarRentals.Shared.Constants;
using FribergCarRentals.Shared.Models.ViewModels.Admin;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.AdminApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages.Admin
{
    /// <summary>
    /// The page component class for the admin back office homepage. 
    /// </summary>
    public partial class AdminHome : AdminPageComponentBase
    {
        #region Constants

        /// <summary>
        /// The url template for the page. 
        /// </summary>
        private const string PageUrlTemplate = "/admin";

        #endregion

        #region Fields

        /// <summary>
        /// A collection of validation errors returned from the API.
        /// </summary>
        private List<MessageViewModel> _apiValidationErrors = new();

        /// <summary>
        /// The viewmodel for the logged in admin. 
        /// </summary>
        private AdminViewModel _admin = default!;

        #endregion

        #region Properties

        /// <summary>
        /// The injected admin API service.
        /// </summary>
        [Inject]
        private IAdminApiService AdminApiService { get; set; } = default!;

        /// <summary>
        /// The injected Auto Mapper service. 
        /// </summary>
        [Inject]
        private IMapper AutoMapper { get; set; } = default!;

        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var authorizationResult = await AuthorizationService.AuthorizeAsync((await AuthenticationStateTask).User, ApplicationUserPolicies.Admin);

            if (authorizationResult.Succeeded)
            {
                var state = await AuthenticationStateTask;
                int adminId = int.Parse(state.User.Claims.Single(x => x.Type == ApplicationUserClaims.AdminId).Value);
                var result = await AdminApiService.GetAdminById(adminId);

                if (result.Success)
                {
                    _admin = AutoMapper.Map<AdminViewModel>(result.Value!);
                }
                else
                {
                    _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.Value, title: x.Key)).ToList();
                }
            }                      
        }

        #endregion
    }
}
