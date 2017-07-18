using System.Collections.Generic;
using PMS.Model.Models;
using PMS.Model.UnitTests.Fakes;
using NUnit.Framework;
using PMS.Model.Services;

namespace PMS.Model.UnitTests.Services.Notifications
{
    [TestFixture]
    public class EventNotificatorTests
    {
        [Test]
        public void Notify_WhenEventIsInvalid_ExceptionExpected()
        {
            var notificator = new TestEventNotificator();
            var @event = new WorkEvent {Type = EventType.WorkItemAppointed};

            Assert.Catch<PmsException>(() => notificator.Notify(@event, new List<ApplicationUser>()));
        }
    }
}
