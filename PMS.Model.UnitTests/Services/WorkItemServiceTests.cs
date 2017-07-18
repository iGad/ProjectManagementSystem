using NUnit.Framework;
using PMS.Model.Models;
using PMS.Model.Services;
using PMS.Model.UnitTests.Fakes;

namespace PMS.Model.UnitTests.Services
{
    [TestFixture]
    public class WorkItemServiceTests
    {
        private WorkItemService CreateWorkItemService()
        {
            var repo = TestHelper.CreateFilledWorkItemRepository(new TestUserRepository());
            var settingsProvider = new TestSettingsValueProvider();
            settingsProvider.SetValueForType(SettingType.MaxDisplayWorkItemCount, "100000");
            return new WorkItemService(repo, settingsProvider);
        }

        [Test]
        [TestCase(WorkItemState.AtWork, 114)]
        [TestCase(WorkItemState.New, 156)]
        [TestCase(WorkItemState.Done, 156)]
        [TestCase(WorkItemState.Planned, 156)]
        [TestCase(WorkItemState.Reviewing, 96)]
        public void GetActualWorkItems_Always_ReturnExpectedItemCount(WorkItemState state, int expectedCount)
        {
            var service = CreateWorkItemService();

            var result = service.GetActualWorkItems();

            Assert.AreEqual(expectedCount, result[state].Count);
        }
    }
}
