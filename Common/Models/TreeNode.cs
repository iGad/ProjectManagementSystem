using System.Collections.Generic;

namespace Common.Models
{
    public class TreeNode
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
        public int? ParentId { get; set; }
        public TreeNode Parent { get; set; }
        public ICollection<TreeNode> Children { get; } = new List<TreeNode>();
    }
}
