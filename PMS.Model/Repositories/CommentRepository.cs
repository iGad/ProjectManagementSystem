using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext context;
        public CommentRepository(ApplicationContext context)
        {
            this.context = context;
        }
        
        public async Task<List<Comment>> GetCommentsForObject(int objectId)
        {
            return await this.context.Comments.Where(x => x.ObjectId == objectId).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public void DeleteComment(Comment comment)
        {
            this.context.Comments.Remove(comment);
        }

        public async Task<Comment> Get(int id)
        {
            return await this.context.Comments.SingleOrDefaultAsync(x => x.Id == id);
        }

        public Comment AddComment(Comment comment)
        {
            return this.context.Comments.Add(comment);
        }

        public async Task<int> SaveChanges()
        {
            return await this.context.SaveChangesAsync();
        }
    }
}
