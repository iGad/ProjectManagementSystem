using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PMS.Model.Models;

namespace ProjectManagementSystem.Controllers
{
    //[Author]
    public class UsersApiController : ApiController
    {
        [HttpGet]
        public IEnumerable<ApplicationUser> GetUsers()
        {
            return null; //User.Identity.GetUserName()
        } 
    }
}