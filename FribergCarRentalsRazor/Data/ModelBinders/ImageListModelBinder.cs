using FribergCarRentals.DataAccess.EntityClasses;
using System.Diagnostics.CodeAnalysis;

namespace FribergCarRentals.Data.ModelBinders
{
    /// <summary>
    /// A model binder class that converts text strings to a collection of <see cref="ImageEntity"/> objects.
    /// </summary>
    public class ImageListModelBinder : SingleValueModelBinderBase<List<ImageEntity>>
    {
        #region Methods

        /// <summary>
        /// Attempts to convert a text string to a collection of <see cref="ImageEntity"/> objects.
        /// </summary>
        /// <param name="value">The text string to convert.</param>
        /// <param name="result">The collection of class instances if the operation was successful.</param>
        /// <returns>True if the operation was successful.</returns>
        protected override bool TryCreateObjectFromString(string value, [NotNullWhen(true)] out List<ImageEntity>? result)
        {
            if (!string.IsNullOrEmpty(value))
            {
                result = new List<ImageEntity>();
                var imagePaths = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

                foreach (var imagePath in imagePaths)
                {
                    result.Add(new ImageEntity(imagePath));
                }

                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        #endregion
    }
}
