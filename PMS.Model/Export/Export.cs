using Common.DI;
using Common.Repositories;
using Common.Services;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;
using PMS.Model.Services.EventDescribers;
using PMS.Model.Services.Notifications;

namespace PMS.Model.Export
{
    public class Export : ICompositionExport
    {
        public void RegisterExport(ICompositionContainer compositionContainer)
        {
            compositionContainer.RegisterExportInRequestScope<ApplicationContext, ApplicationContext>();
            compositionContainer.RegisterExport<WorkItemRepository, IWorkItemRepository>();
            compositionContainer.RegisterExport<WorkItemRepository, ITreeRepository>();
            compositionContainer.RegisterExport<UserRepository, IUserRepository>();
            compositionContainer.RegisterExport<EventRepository, IEventRepository>();
            compositionContainer.RegisterExport<UserPermissionsRepository, IUserPermissionsRepository>();
            compositionContainer.RegisterExport<SettingRepository, ISettingRepository>();
            compositionContainer.RegisterExport<SettingRepository, ISettingsValueProvider>();     
            compositionContainer.RegisterExport<CommentRepository, ICommentRepository>();
            compositionContainer.RegisterExport<AutofillRepository, IAutofillRepository>();

            compositionContainer.RegisterExport<LocalFileSystemManager, IFileSystemManager>();
            compositionContainer.RegisterExport<FileSystemManagerProvider>();

            compositionContainer.RegisterExport<UsersService, IUsersService>();
            compositionContainer.RegisterExport<UsersService, ICurrentUserProvider>();
            compositionContainer.RegisterExport<WorkItemService>();
            compositionContainer.RegisterExport<EventService, IEventService>();
            compositionContainer.RegisterExport<SettingsService>();
            compositionContainer.RegisterExport<CommentsService>();
            compositionContainer.RegisterExport<AttachingFileService>();

            #region Event describers

            compositionContainer.RegisterExport<ItemAddedEventDescriber, EventDescriber>();
            compositionContainer.RegisterExport<ItemAppointedEventDescriber, EventDescriber>();
            compositionContainer.RegisterExport<ItemDeletedEventDescriber, EventDescriber>();
            compositionContainer.RegisterExport<ItemDisappointedEventDescriber, EventDescriber>();
            compositionContainer.RegisterExport<ItemChangedEventDecriber, EventDescriber>();
            compositionContainer.RegisterExport<StateChangedEventDescriber, EventDescriber>();

            #endregion

            compositionContainer.RegisterExport<EventNotificatorsUsersProvider, IEventNotificatorsUsersProvider>();
            compositionContainer.RegisterExport<NotificationService, INotificationService>();
            compositionContainer.RegisterExport<DatabaseEventNotificator, EventNotificator>();
            compositionContainer.RegisterExport<DataUpdater>();



        }
    }
}
