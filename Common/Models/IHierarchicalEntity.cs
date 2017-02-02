using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
    public interface IHierarchicalEntity
    {
        int Id { get; set; }
        IHierarchicalEntity Parent { get; set; }
        int? ParentId { get; set; }
        string Name { get; set; }
    }
}
