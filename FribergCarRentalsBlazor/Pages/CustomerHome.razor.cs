using AutoMapper;
using FribergCarRentals.Shared.Models.Blazor.ViewModels;
using FribergCarRentals.Shared.Models.ViewModels.Message;
using FribergCarRentals.Shared.Models.ViewModels.Other;
using FribergCarRentalsBlazor.Pages.Customer.Order;
using FribergCarRentalsBlazor.Services.FribergCarRentalsApi.CustomerApi;
using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Pages
{
    /// <summary>
    /// Component for the home page.
    /// </summary>
    public partial class CustomerHome : ComponentBase
    {
		#region Constants

		/// <summary>
		/// The url template for the page. 
		/// </summary>
		private const string PageUrlTemplate = "/";

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

        //// <summary>
        /// The injected customer order API service.
        /// </summary>
        [Inject]
        private ICustomerOrderApiService CustomerOrderApiService { get; set; } = default!;

        /// <summary>
        /// A collection of image view models for the image slide. 
        /// </summary>
        private ListViewModel<BlazorSlideShowImageViewModel>? ImageSlides { get; set; }

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

            var result = await CustomerOrderApiService.GetFirstCarPerCategory();

            if (result.Success)
            {
                ImageSlides = new ListViewModel<BlazorSlideShowImageViewModel>();

                foreach (var car in result.Value!)
                {
                    var image = car.Images.First();

                    ImageSlides.Models.Add(new BlazorSlideShowImageViewModel(
                        image.Url, image.FileName, image.ImageId,
                        imageCaption: car.Category!.CategoryName,
                        linksToPage: $"{BookCar.GetPageUrl(car.Category.CarCategoryId)}"
                    ));
                }
            }
            else
            {
                _apiValidationErrors = result.Errors.Select(x => new MessageViewModel(MessageType.Error, x.ErrorMessage, title: x.ErrorType)).ToList();                
            }
        }

		#endregion
	}
}
