using FribergCarRentals.Data.EntityClasses;
using MvcRazorPages.Shared.ViewModels.Car;
using MvcRazorPages.Shared.ViewModels.CarCategory;
using MvcRazorPages.Shared.ViewModels.Other;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcRazorPages.Shared.ViewModels.Order
{
    /// <summary>
    /// A view model class that handles data related to order creation. 
    /// </summary>
    public class BookCarViewModel : ViewModelBase
    {
        #region Constants

        /// <summary>
        /// A constant text string to represent all car categories.
        /// </summary>
        public const string AllCarCategoriesText = "All";

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public BookCarViewModel()
        {

        }

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="availableCarCategoryFilters">A collection of car categories that can be used as filters when searching for cars to rent.</param>
        /// <param name="havePerformedCarSearch">True if the user have performed a car search. </param>
        /// <param name="availableCars">A collection of cars that matches the chosen date filters, or all cars if no filters where chosen.</param>
        /// <param name="pickupDateFilter">An optional pickup date filter to override the default date when searching for cars to rent.</param>
        /// <param name="returnDateFilter">An optional return date filter to override the default date when searching for cars to rent.</param>
        /// <param name="carCategoryFilter">The car category filter to use when searching for cars to rent.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public BookCarViewModel(List<CarCategoryEntity> availableCarCategoryFilters, bool havePerformedCarSearch, List<CarViewModel>? availableCars = null, 
            DateTime? pickupDateFilter = null, DateTime? returnDateFilter = null, int? carCategoryFilter = null)
        {
            #region Checks

            if (availableCarCategoryFilters is null)
            {
                throw new ArgumentNullException(nameof(availableCars), $"The value of parameter '{availableCarCategoryFilters}' can't be null.");
            }

            #endregion

            AvailableCars = availableCars is not null ? availableCars : new();
            HavePerformedCarSearch = havePerformedCarSearch;
            SelectedCarCategoryFilter = carCategoryFilter is not null ? carCategoryFilter.Value : 0;

            if (pickupDateFilter is not null)
            {
                PickupDateLocalTime = pickupDateFilter.Value;
            }

            if (returnDateFilter is not null)
            {
                ReturnDateLocalTime = returnDateFilter.Value;
            }

            SetAvailableCarCategoryFilters(availableCarCategoryFilters);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Cars available to rent 
        /// </summary>
        [BindNever]
        public List<CarViewModel> AvailableCars { get; } = new();

        /// <summary>
        /// A collection of car categories that can be used as filters when searching for cars to rent.
        /// </summary>
        [BindNever]
        public List<CarCategoryViewModel> AvailableCarCategoryFilters { get; set; } = new();

        /// <summary>
        /// Returns true if the user have performed a car search. 
        /// </summary>
        public bool HavePerformedCarSearch { get; }

        /// <summary>
        /// The car category filter to use when searching for cars. 
        /// </summary>
        /// <remarks>
        /// An ID of zero represents no filter. 
        /// </remarks>
        [DisplayName("Category")]
        [Required]
        public int SelectedCarCategoryFilter { get; set; }

        /// <summary>
        /// The pickup date filter to use when searching for cars.
        /// </summary>
        [DisplayName("Pickup Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required]
        public DateTime PickupDateLocalTime { get; set; } = DateTime.Now.AddDays(1);

        /// <summary>
        /// The return date filter to use when searching for cars.
        /// </summary>
        [DisplayName("Return Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DateFormatString)]
        [Required]
        public DateTime ReturnDateLocalTime { get; set; } = DateTime.Now.AddDays(1);

        #endregion

        #region Methods

        /// <summary>
        /// Sets the car category filters that can be used as filters when searching for cars to rent.
        /// </summary>
        /// <param name="availableCarCategoryFilters">The categories to set.</param>
        public void SetAvailableCarCategoryFilters(IEnumerable<CarCategoryEntity> availableCarCategoryFilters)
        {
            AvailableCarCategoryFilters = availableCarCategoryFilters.Select(x => new CarCategoryViewModel(x)).Prepend(new CarCategoryViewModel(0, AllCarCategoriesText)).ToList();
        }

        #endregion
    }
}
