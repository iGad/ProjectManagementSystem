using Common.DI;
using Microsoft.AspNet.SignalR;
using ProjectManagementSystem.Hubs;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Export
{
    public class Export : ICompositionExport
    {
        public void RegisterExport(ICompositionContainer compositionContainer)
        {
            compositionContainer.RegisterExport<WorkItemApiService>();
            var notificationService = new NotificationService(GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients);
            compositionContainer.RegisterInstance(notificationService);
        }
    }
}