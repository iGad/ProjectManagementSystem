using System.Collections.Generic;
using System.Linq;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public class CommentRepository
    {
        private readonly ApplicationContext context;
        public CommentRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public Comment GetById(int id)
        {
            return this.context.Comments.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<Comment> GetByWorkItemId(int workItemId)
        {
            return this.context.Comments.Where(x => x.WorkItemId == workItemId);
        }

        public Comment AddComment(Comment comment)
        {
            return this.context.Comments.Add(comment);
        }
    }
}
