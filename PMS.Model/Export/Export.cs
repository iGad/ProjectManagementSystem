﻿using Common.DI;
using Common.Repositories;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;
using PMS.Model.Services.EventDescribers;

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

            compositionContainer.RegisterExport<UsersService>();
            compositionContainer.RegisterExport<WorkItemService>();
            compositionContainer.RegisterExport<EventService, IEventService>();

            #region Event describers

            compositionContainer.RegisterExport<ItemAddedEventDescriber, EventDescriber>();
            compositionContainer.RegisterExport<ItemAppointedEventDescriber, EventDescriber>();
            compositionContainer.RegisterExport<ItemDeletedEventDescriber, EventDescriber>();
            compositionContainer.RegisterExport<ItemDisappointedEventDescriber, EventDescriber>();
            compositionContainer.RegisterExport<ItemChangedEventDecriber, EventDescriber>();
            compositionContainer.RegisterExport<StateChangedEventDescriber, EventDescriber>();

            #endregion
        }
    }
}
