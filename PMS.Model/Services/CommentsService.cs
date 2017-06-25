using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class CommentsService
    {
        private readonly ICommentRepository _repository;
        private readonly UsersService _usersService;

        public CommentsService(ICommentRepository repository, UsersService usersService)
        {
            this._repository = repository;
            this._usersService = usersService;
        }

        public async Task<Comment> Add(Comment comment)
        {
            var user = this._usersService.GetCurrentUser();
            comment.UserId = user.Id;
            comment = this._repository.AddComment(comment);
            await this._repository.SaveChanges();
            comment.User = user;
            return comment;
        }

        public async Task<List<Comment>> GetCommentsForObject(int objectId)
        {
            return await this._repository.GetCommentsForObject(objectId);
        }

        public async Task DeleteComment(int id)
        {
            var comment = await this._repository.Get(id);
            if (comment == null)
                throw new PmsException($"Комментарий с идентификатором {id} не найден");
            var user = this._usersService.GetCurrentUser();
            var roles = this._usersService.GetRolesByIds(user.Roles.Select(x => x.RoleId));
            if (user.Id != comment.UserId && roles.All(x => x.RoleCode != RoleType.Admin && x.RoleCode != RoleType.Director))
                throw new PmsException("Нет доступа для этой операции");
            this._repository.DeleteComment(comment);
            await this._repository.SaveChanges();
        }
    }
}
