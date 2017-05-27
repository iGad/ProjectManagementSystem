using System.Collections.Generic;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetCommentsForObject(int objectId);
        Task<Comment> Get(int id);
        void DeleteComment(Comment comment);
        Comment AddComment(Comment comment);
        Task<int> SaveChanges();
    }
}