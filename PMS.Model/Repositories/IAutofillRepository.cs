using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface IAutofillRepository : IRepository
    {
        Autofill Add(Autofill autofill);
        void Delete(Autofill autofill);
        IEnumerable<Autofill> Get(Expression<Func<Autofill, bool>> whereExpression);
        void Update(Autofill old, Autofill newAutofill);
    }
}