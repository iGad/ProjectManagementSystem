using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PMS.Model.Models;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly CommentsService service;
        private readonly UsersService usersService;

        public CommentsController(CommentsService service, UsersService usersService)
        {
            this.service = service;
            this.usersService = usersService;
        }

        [HttpGet]
        public ActionResult GetCommentsForObject(int objectId)
        {
            return Json(this.service.GetCommentsForObject(objectId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            return Json(this.service.Add(comment), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteComment(int id)
        {
            await this.service.DeleteComment(id);
            return Json("OK");
        }
    }
}