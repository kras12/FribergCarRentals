using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor
{
	/// <summary>
	/// The application component.
	/// </summary>
	public partial class App : ComponentBase
	{
		#region Properties

		/// <summary>
		/// The injected navigation manager.
		/// </summary>
		[Inject]
#pragma warning disable CS8618
		private NavigationManager NavigationManager { get; set; }
#pragma warning restore CS8618

		#endregion
	}
}
