using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Model.Models
{
    public enum WorkItemStatus
    {
        InWork,
        NotAccepted,
        WaitInspection,
        Inspection,
        Rework
    }
}
