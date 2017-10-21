using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.CommonModels
{
    public class UserSettingModel
    {
        public UserSettingModel()
        {
        }

        public UserSettingModel(UserSetting setting, UserSettingValue value)
        {
            Id = setting.Id;
            ValueType = setting.ValueType;
            Type = setting.Type;
            DefaultValue = setting.DefaultValue;
            UserId = value.UserId;
            Value = value.Value;
            Name = setting.Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ValueType ValueType { get; set; }
        public UserSettingType Type { get; set; }
        public string DefaultValue { get; set; }
        public string UserId { get; set; }
        public string Value { get; set; }
    }
}
