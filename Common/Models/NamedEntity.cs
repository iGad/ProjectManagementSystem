using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class NamedEntity : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
