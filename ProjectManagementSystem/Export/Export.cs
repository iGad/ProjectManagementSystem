using Common.DI;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Export
{
    public class Export : ICompositionExport
    {
        public void RegisterExport(ICompositionContainer compositionContainer)
        {
            compositionContainer.RegisterExport<WorkItemApiService>();
        }
    }
}