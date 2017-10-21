using Common;

namespace PMS.Model.Models
{
    public enum UserSettingType
    {
        [LocalizedDescription("NotificationsSound", typeof(Resources))]
        NotificationsSound = 0,
        [LocalizedDescription("NotificationsToEmail", typeof(Resources))]
        NotificationsToEmail = 1,
        [LocalizedDescription("NotificationsEmail", typeof(Resources))]
        NotificationsEmail = 2
    }
}