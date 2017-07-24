using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public class AutofillRepository : IAutofillRepository
    {
        private readonly ApplicationContext _context;

        public AutofillRepository(ApplicationContext context)
        {
            _context = context;
        }
        public IEnumerable<Autofill> Get(Expression<Func<Autofill, bool>> whereExpression)
        {
            return null;
        }

        public void Delete(Autofill autofill)
        {
            
        }

        public Autofill Add(Autofill autofill)
        {
            return null;
        }

        public void Update(Autofill old, Autofill newAutofill)
        {
            
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
