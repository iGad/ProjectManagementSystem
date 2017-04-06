using System.Collections.Generic;
using PMS.Model.Models;

namespace PMS.Model.Services
{
    public class ApplicationUserEqualityComparer : IEqualityComparer<ApplicationUser>
    {
        public bool Equals(ApplicationUser x, ApplicationUser y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(ApplicationUser obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
