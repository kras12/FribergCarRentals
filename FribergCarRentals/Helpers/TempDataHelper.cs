using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace FribergCarRentals.Helpers
{
    public static class TempDataHelper
    {
        public static void Set<T>(ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T? Get<T>(ITempDataDictionary tempData, string key) where T : class
        {
            if (tempData.TryGetValue(key, out object? value))
            {
                Remove(tempData, key);
                return JsonConvert.DeserializeObject<T>((string)value!);
            }

            return null;
        }

        public static bool TryGet<T>(ITempDataDictionary tempData, string key, [NotNullWhen(returnValue: true)] out T? data) where T : class
        {
            data = Get<T>(tempData, key);
            return data is not null;
        }

        private static void Remove(ITempDataDictionary tempData, string key)
        {
            tempData.Remove(key);
        }
    }
}
