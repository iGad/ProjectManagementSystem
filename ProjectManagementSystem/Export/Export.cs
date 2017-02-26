using Common.DI;
using Common.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using ProjectManagementSystem.Hubs;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Export
{
    public class Export : ICompositionExport
    {
        public void RegisterExport(ICompositionContainer compositionContainer)
        {
            compositionContainer.RegisterExport<WorkItemApiService>();
            compositionContainer.RegisterExport<UserIdProvider, ICurrentUsernameProvider>();
            compositionContainer.RegisterInstance<IHubConnectionContext<dynamic>>(GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients);
            compositionContainer.RegisterExport<NotificationService, INotifyService>();
            //var notificationService = new NotificationService(GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients);
            //compositionContainer.RegisterInstance(notificationService);
        }
    }
}