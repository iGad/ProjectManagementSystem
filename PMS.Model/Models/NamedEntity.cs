using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Model.Models
{
    public class NamedEntity : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
