using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;

namespace FribergCarRentals.Shared.Models.Mvc.ViewModels.Car
{
	/// <summary>
	///  A view model class that handles data for creating a new car.
	/// </summary>
	public class CreateCarViewModel : CreateCarViewModelBase<IFormFile>
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