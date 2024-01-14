using System.Diagnostics.CodeAnalysis;

namespace FribergCarRentals.Data
{
    internal static class DataTransferHelper
    {
        public static bool TryTransferData<TSource, TDest>(TSource sourceObject, [NotNullWhen(returnValue: true)] out TDest destinationObject) 
            where TSource : class where TDest : class, new()
        {
            destinationObject = new TDest();
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDest).GetProperties();
            bool dataWasWritten = false;

            foreach (var sourceProperty in sourceProperties)
            {
                var destinationProperty = destinationProperties.FirstOrDefault(x => x.Name == sourceProperty.Name && x.PropertyType == sourceProperty.PropertyType && x.CanWrite);

                if (destinationProperty != null)
                {
                    destinationProperty.SetValue(destinationObject, sourceProperty.GetValue(sourceObject));
                    dataWasWritten = true;
                }
            }

            return dataWasWritten;
        }
    }
}
