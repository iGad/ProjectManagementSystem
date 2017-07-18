using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PMS.Model.Models;
using PMS.Model.Services;

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

        public static void Validate(this WorkEvent workEvent)
        {
            if (string.IsNullOrEmpty(workEvent.UserId))
                throw new PmsException("UserId can not be empty");

            switch (workEvent.Type)
            {
                case EventType.WorkItemChanged:
                case EventType.WorkItemStateChanged:
                case EventType.WorkItemAdded:
                case EventType.WorkItemDeleted:
                case EventType.WorkItemCommentAdded:
                    if (!workEvent.ObjectId.HasValue)
                        throw new PmsException("Не указан идентификатор рабочего элемента");
                    break;
                case EventType.WorkItemAppointed:
                case EventType.WorkItemDisappointed:
                    if (string.IsNullOrWhiteSpace(workEvent.Data))
                        throw new PmsException("В поле Data не содержится идентификатор пользователя");
                    break;
                case EventType.User:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if(workEvent.Type == EventType.WorkItemStateChanged && string.IsNullOrWhiteSpace(workEvent.Data))
                throw new PmsException("В поле Data не содержится предыдущее состояние  РЭ");
            
        }
    }
}
