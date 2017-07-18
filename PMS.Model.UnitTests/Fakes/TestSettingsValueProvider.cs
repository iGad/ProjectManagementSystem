using System.Collections.Generic;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.UnitTests.Fakes
{
    public class TestSettingsValueProvider : ISettingsValueProvider
    {
        private readonly Dictionary<SettingType, string> _values = new Dictionary<SettingType, string>();
        public TestSettingsValueProvider(bool fillDefaultValues = true)
        {
            if (fillDefaultValues)
            {
                _values.Add(SettingType.MaxDisplayWorkItemCount, "30");
            }
        }

        
        public string GetSettingValue(SettingType type)
        {
            return _values[type];
        }

        public void SetValueForType(SettingType type, string value)
        {
            if (_values.ContainsKey(type))
                _values[type] = value;
            else
            {
                _values.Add(type, value);
            }
        }
    }
}
