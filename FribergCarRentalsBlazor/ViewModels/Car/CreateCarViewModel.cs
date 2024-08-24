using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using Microsoft.AspNetCore.Components.Forms;

namespace FribergCarRentalsBlazor.ViewModels.Car
{
    /// <summary>
    ///  A view model class that handles data for creating a new car.
    /// </summary>
    public class CreateCarViewModel : CreateCarViewModelBase<IBrowserFile>
    {
        #region Constructors

        /// <summary>
        ///  A constructor.
        /// </summary>
        public CreateCarViewModel() : base()
        {

        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="categories">A collection of available car categories to choose from.</param>
        public CreateCarViewModel(IEnumerable<CarCategoryViewModel> categories) : base(categories)
        {

        }

        #endregion
    }
}