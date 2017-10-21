namespace PMS.Model.Models
{
    public class UserSettingValue
    {
        public int UserSettingId { get; set; }
        public UserSetting UserSetting { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Value { get; set; }
    }
}