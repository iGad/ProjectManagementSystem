using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PMS.Model.Models
{
    public enum RoleType
    {
        Executor = 0,
        Manager = 1,
        MainProjectEngeneer = 2,
        Director = 3,
        Admin = 4
    }
    public class Role : IdentityRole<string, UserRole>
    {
        public Role()
        {
            Id = Guid.NewGuid().ToString();
        }
        public Role(string name, RoleType code):this()
        {
            Name = name;
            RoleCode = code;
        }
        public RoleType RoleCode { get; set; }
    }
}
