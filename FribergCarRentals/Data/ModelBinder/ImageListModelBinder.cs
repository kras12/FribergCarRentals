using FribergCarRentals.DataAccess.EntityClasses;
using System.Diagnostics.CodeAnalysis;

namespace FribergCarRentals.Data.ModelBinder
{
    public class ImageListModelBinder : SingleValueModelBinderBase<List<ImageEntity>>
    {
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
    }
}
