using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementSystem.ViewModels
{
    public class NotificationViewModel
    {
        public string Text { get; set; }
        public int Type { get; set; }
        public int WorkItemId { get; set; }
    }
}