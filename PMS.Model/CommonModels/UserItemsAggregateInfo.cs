

namespace PMS.Model.CommonModels
{
    public class UserItemsAggregateInfo
    {
        public string UserId { get; set; }
        public string UserInfo { get; set; }
        public int PlannedCount { get; set; }
        public int AtWorkCount { get; set; }
        public int ReviewingCount { get; set; }
    }
}
