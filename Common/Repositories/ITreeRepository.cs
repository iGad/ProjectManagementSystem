using System;
using System.Collections.Generic;
using Common.Models;

namespace Common.Repositories
{
    public interface ITreeRepository
    {
        ICollection<TreeNode> GetNodes(Func<IHierarchicalEntity, bool> whereExpression);
    }
}
