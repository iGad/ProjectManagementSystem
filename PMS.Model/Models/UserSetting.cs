using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace PMS.Model.Models
{
    public enum UserSettingType
    {
        NotificationsSound = 0,
        NotificationsToEmail = 1,

    }

    public class UserSetting : NamedEntity
    {
        public ValueType ValueType { get; set; }
        public UserSettingType Type { get; set; }
        public string DefaultValue { get; set; }
    }

    public class UserSettingValue
    {
        public int UserSettingId { get; set; }
        public UserSetting UserSetting { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Value { get; set; }
    }
}
