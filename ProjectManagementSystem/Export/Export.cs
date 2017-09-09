using Common.DI;
using Common.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PMS.Model.Services;
using PMS.Model.Services.Notifications;
using ProjectManagementSystem.Hubs;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Export
{
    public class Export : ICompositionExport
    {
        public void RegisterExport(ICompositionContainer compositionContainer)
        {
            compositionContainer.RegisterExport<WorkItemApiService>();
            compositionContainer.RegisterExport<AutofillService>();
            compositionContainer.RegisterExport<UserIdProvider, ICurrentUsernameProvider>();
            //compositionContainer.RegisterInstance<IHubConnectionContext<dynamic>>();
            compositionContainer.RegisterExport<SignalrClientsProvider>();
            compositionContainer.RegisterExport<SignalrNotificator, IRealtimeNotificationService>();
            compositionContainer.RegisterExport<SignalrNotificator, EventNotificator>();
            compositionContainer.RegisterExport<DataGenerator>();
            //var notificationService = new SignalrNotificator(GlobalHost.ConnectionManager.GetHubContext<NotificationHub>().Clients);
            //compositionContainer.RegisterInstance(notificationService);
        }
    }
}