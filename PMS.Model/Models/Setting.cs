using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Models;
using PMS.Model.Services;

namespace PMS.Model.Models
{
    public enum SettingType
    {
        FileStoragePath = 0,
        MaxDisplayWorkItemCount = 1,

    }

    public enum ValueType
    {
        String = 0,
        Int = 1,
        Bool = 2
    }

    public class Setting : NamedEntity
    {
        public SettingType Type { get; set; }
        public ValueType ValueType { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }
        public string ValueRegex { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }

    public static class SettingExtensions
    {
        public static string GetValue(this Setting setting)
        {
            return string.IsNullOrWhiteSpace(setting.Value) ? setting.DefaultValue : setting.Value;
        }

        public static void SetValue(this Setting setting, string value)
        {
            var realValue = value;
            switch (setting.ValueType)
            {
                case ValueType.String:
                    if (!string.IsNullOrWhiteSpace(setting.ValueRegex) && !Regex.IsMatch(value, setting.ValueRegex))
                        throw new PmsException("Значение не удовлетворяет ожидаемой маске");
                    break;
                case ValueType.Int:
                    int integer;
                    if(!int.TryParse(value, out integer))
                        throw new PmsException("Значение не является целым числом");
                    if (setting.MinValue.HasValue && setting.MinValue.Value > integer || setting.MaxValue.HasValue && setting.MaxValue.Value < integer)
                        throw new PmsException("Значение не входит в ожидаемый интервал");
                    break;
                case ValueType.Bool:
                    bool flag;
                    if(!bool.TryParse(value, out flag))
                        throw new PmsException("Значение не является логическим значением");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            setting.Value = realValue;
        }
    }
}
