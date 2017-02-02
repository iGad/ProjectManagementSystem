using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PMS.Model
{
    public static class Extensions
    {
       
        public static IList<TEnum> ToEnumList<TEnum>() where TEnum : struct, IConvertible
        {
            return Enum.GetValues(typeof (TEnum)).OfType<TEnum>().OrderBy(x => x).ToList();
        }

        public static string GetDescription(this Enum @enum) 
        {
            var e = GetEnumList(@enum).First();
            DescriptionAttribute attr = null;
            var fieldInfo = e.GetType().GetField(e.ToString());
            if (Attribute.IsDefined(fieldInfo, typeof(DescriptionAttribute)))
                attr = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

            return attr == null ? e.ToString() : attr.Description;
        }

        private static IEnumerable<Enum> GetEnumList(Enum @enum)
        {
            var type = @enum.GetType();

            if (!Validate(@enum))
                throw new ValidationException("Enum " + type.Name + " does not contain member with name " + @enum);

            return new[] { @enum };
        }

        private static bool Validate(Enum @enum)
        {
            return @enum.GetType().GetFields().Any(fi => fi.Name == @enum.ToString());
        }
    }
}
