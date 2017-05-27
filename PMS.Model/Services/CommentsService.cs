using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class CommentsService
    {
        private readonly ICommentRepository repository;
        private readonly UsersService usersService;

        public CommentsService(ICommentRepository repository, UsersService usersService)
        {
            this.repository = repository;
            this.usersService = usersService;
        }

        public Comment Add(Comment comment)
        {
            var user = this.usersService.GetCurrentUser();
            comment.UserId = user.Id;
            comment = this.repository.AddComment(comment);
            this.repository.SaveChanges();
            return comment;
        }

        public async Task<List<Comment>> GetCommentsForObject(int objectId)
        {
            return await this.repository.GetCommentsForObject(objectId);
        }

        public async Task DeleteComment(int id)
        {
            var comment = await this.repository.Get(id);
            if(comment == null)
                throw new PmsException($"Комментарий с идентификатором {id} не найден");
            var user = this.usersService.GetCurrentUser();
            var roles = this.usersService.GetRolesByIds(user.Roles.Select(x => x.RoleId));
            if (user.Id != comment.UserId && roles.All(x => x.RoleCode != RoleType.Admin && x.RoleCode != RoleType.Director))
                throw new PmsException("Нет доступа для этой операции");
            this.repository.DeleteComment(comment);
            await this.repository.SaveChanges();
        }
    }
}
