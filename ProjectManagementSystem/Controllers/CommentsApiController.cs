using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PMS.Model.Models;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    [System.Web.Http.Authorize]
    public class CommentsApiController : Controller
    {
        private readonly CommentsService _service;

        public CommentsApiController(CommentsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetCommentsForObject(int objectId)
        {
            return Json(await _service.GetCommentsForObject(objectId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(Comment comment)
        {
            return Json(await _service.Add(comment), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteComment(int id)
        {
            await _service.DeleteComment(id);
            return Json("OK");
        }
    }
}