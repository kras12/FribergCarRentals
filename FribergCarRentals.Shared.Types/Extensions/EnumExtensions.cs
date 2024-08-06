using System.Reflection;

namespace FribergCarRentals.Shared.Types.Extensions
{
	/// <summary>
	/// An enum extension class that supports the retrieval of attributes. 
	/// </summary>
	public static class EnumExtensions
	{
		#region Methods

		/// <summary>
		/// Gets an attribute. 
		/// </summary>
		/// <typeparam name="T">The type of the attribute.</typeparam>
		/// <param name="enumValue">The enum to get the attribute for.</param>
		/// <returns>An attribute of type <typeparamref name="T"/>.</returns>
		/// <exception cref="InvalidOperationException"></exception>
		public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
		{
			return enumValue.GetType()
				.GetMember(enumValue.ToString())
				.First()
				.GetCustomAttribute<T>() ??
					throw new InvalidOperationException($"The attribute of type '{typeof(T)}' could not be found on the enum value.");
		}

		#endregion
	}
}
