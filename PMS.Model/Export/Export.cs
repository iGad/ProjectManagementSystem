using Common.DI;
using Common.Repositories;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;

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

            compositionContainer.RegisterExport<UsersService>();
            compositionContainer.RegisterExport<WorkItemService>();
        }
    }
}
