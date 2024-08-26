using System.ComponentModel;
using FribergCarRentals.Shared.Models.ViewModels.Car;
using FribergCarRentals.Shared.Models.ViewModels.CarCategory;
using FribergCarRentals.Shared.Models.ViewModels.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FribergCarRentals.Shared.Models.Mvc.ViewModels.Car
{
	/// <summary>
	/// A view model class to handle data used for editing a car. 
	/// </summary>
	public class EditCarViewModel : EditCarViewModelBase<IFormFile>
	{
		#region Properties

		/// <summary>
		/// A collection of available car categories to choose from.
		/// </summary>
		[DisplayName("Categories")]
		[BindNever]
		public override List<CarCategoryViewModel> Categories { get; set; } = new();

		/// <summary>
		/// A collection of images for the car.
		/// </summary>
		[DisplayName("Images")]
		[BindNever]
		public override List<ImageViewModel> Images { get; set; } = new();

		#endregion
	}
}