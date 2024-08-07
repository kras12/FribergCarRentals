using Microsoft.AspNetCore.Components;

namespace FribergCarRentalsBlazor.Layout
{
	/// <summary>
	/// A component that handles both login and logout functionality for brokers.
	/// </summary>
	public partial class LoginButton : ComponentBase
	{
		#region Constants

		/// <summary>
		/// The key where the component looks for redirect url targets after logging in. 
		/// </summary>
		public const string RedirectUrlStorageKey = "RedirectUrlAfterLoginKey";

		#endregion
	}
}
