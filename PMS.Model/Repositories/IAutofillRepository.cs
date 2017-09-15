using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface IAutofillRepository : IRepository
    {
        Autofill Add(Autofill autofill);
        void Delete(Autofill autofill);
        Task<Autofill> Get(int id);
        Task<ICollection<Autofill>> Get(AutofillFilterModel filterModel);
        Task<ICollection<Autofill>> Get(Expression<Func<Autofill, bool>> whereExpression);
        Task<int> GetTotalCount(AutofillFilterModel filterModel);
    }
}