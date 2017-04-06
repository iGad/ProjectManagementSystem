using PMS.Model.Models;
using PMS.Model.Services;

namespace PMS.Model.UnitTests.Fakes
{
    public class TestEventDescriber : EventDescriber
    {
        public override bool CanDescribeEventType(EventType eventType)
        {
            return true;
        }

        protected override string GetDescription(WorkEvent workEvent, ApplicationUser forUser)
        {
            return "test";
        }
    }
}
