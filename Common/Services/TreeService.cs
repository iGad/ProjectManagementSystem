using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Models;
using Common.Repositories;

namespace Common.Services
{
    public class TreeService
    {
        private readonly ITreeRepository _repository;

        public TreeService(ITreeRepository repository)
        {
            this._repository = repository;
        }

        public ICollection<TreeNode> GetTree()
        {
            var rootItems = this._repository.GetNodes(x => !x.ParentId.HasValue).ToArray();
            foreach (var rootItem in rootItems)
            {
                FillNodes(rootItem);
            }
            return rootItems;
        }

        private void FillNodes(TreeNode parentNode)
        {
            parentNode.Children.AddRange(this._repository.GetNodes(x => x.ParentId == parentNode.Id));
            foreach (var node in parentNode.Children)
            {
                FillNodes(node);
            }
        }
    }
}
