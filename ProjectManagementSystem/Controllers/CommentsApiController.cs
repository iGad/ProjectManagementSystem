using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PMS.Model.Models;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class CommentsApiController : Controller
    {
        private readonly CommentsService service;
        private readonly UsersService usersService;

        public CommentsApiController(CommentsService service, UsersService usersService)
        {
            this.service = service;
            this.usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult> GetCommentsForObject(int objectId)
        {
            return Json(await this.service.GetCommentsForObject(objectId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(Comment comment)
        {
            return Json(await this.service.Add(comment), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteComment(int id)
        {
            await this.service.DeleteComment(id);
            return Json("OK");
        }
    }
}