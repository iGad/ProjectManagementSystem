using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext _context;
        public CommentRepository(ApplicationContext context)
        {
            this._context = context;
        }

        public async Task<List<Comment>> GetCommentsForObject(int objectId)
        {
            return await this._context.Comments.Include(x=>x.User).Where(x => x.ObjectId == objectId).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }

        public void DeleteComment(Comment comment)
        {
            this._context.Comments.Remove(comment);
        }

        public async Task<Comment> Get(int id)
        {
            return await this._context.Comments.SingleOrDefaultAsync(x => x.Id == id);
        }

        public Comment AddComment(Comment comment)
        {
            return this._context.Comments.Add(comment);
        }

        public async Task<int> SaveChanges()
        {
            return await this._context.SaveChangesAsync();
        }
    }
}
