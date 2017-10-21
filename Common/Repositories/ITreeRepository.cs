using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Models;

namespace Common.Repositories
{
    public interface ITreeRepository
    {
        ICollection<TreeNode> GetNodes(Expression<Func<IHierarchicalEntity, bool>> whereExpression);
    }
}
