using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Data.SharedClasses
{
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<T>() ?? 
                    throw new InvalidOperationException($"The attribute of type '{typeof(T)}' could not be found on the enum value.");
        }
    }
}
