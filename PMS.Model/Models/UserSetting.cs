using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace PMS.Model.Models
{
    public class UserSetting : NamedEntity
    {
        public ValueType ValueType { get; set; }
        public UserSettingType Type { get; set; }
        public string DefaultValue { get; set; }
        public string Regex { get; set; }
    }
}
