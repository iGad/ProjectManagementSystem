using System.Threading.Tasks;

namespace PMS.Model.Repositories
{
    public interface IRepository
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
