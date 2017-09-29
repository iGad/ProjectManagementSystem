using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PMS.Model.CommonModels.FilterModels;
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
        
        public async Task<int> GetTotalCount(AutofillFilterModel filterModel)
        {
            return await CreateQuery(filterModel).CountAsync();
        }
        
        public async Task<ICollection<Autofill>> Get(Expression<Func<Autofill,bool>> whereExpression)
        {
            return await _context.Autofills.Where(whereExpression).ToListAsync();
        }

        public Task<Autofill> Get(int id)
        {
            return _context.Autofills.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<Autofill>> Get(AutofillFilterModel filterModel)
        {
            var query = CreateQuery(filterModel);
            query = query.OrderBy(filterModel.Sorting.FieldName + " " + filterModel.Sorting.Direction);
            query = query.Skip((filterModel.CorrectPageNumber - 1) * filterModel.CorrectPageSize).Take(filterModel.CorrectPageSize);
            return await query.ToListAsync();
        }

        private IQueryable<Autofill> CreateQuery(AutofillFilterModel filterModel)
        {
            if (string.IsNullOrWhiteSpace(filterModel.SearchText))
                return _context.Autofills;
            
            var filterParams = filterModel.ParseText(filterModel.SearchText);
            var goodTypes = Extensions.ToEnumList<WorkItemType>().ToDictionary(x => x, x => x.GetDescription()).Where(x => filterParams.Any(p => x.Value.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0)).Select(x => x.Key).ToArray();
            string firstElement = filterParams[0];
            IQueryable<Autofill>  query = _context.Autofills.Where(x => x.Name.Contains(firstElement) || x.Description.Contains(firstElement) || goodTypes.Contains(x.WorkItemType));
            for (var i = 1; i < filterParams.Length; i++)
            {
                var param = filterParams[i];
                query = query.Union(_context.Autofills.Where(x => x.Name.Contains(param) || x.Description.Contains(param)));
            }
            return query;
        }
        

        public void Delete(Autofill autofill)
        {
            _context.Autofills.Remove(autofill);
        }

        public Autofill Add(Autofill autofill)
        {
            return _context.Autofills.Add(autofill);
        }
        
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        
    }
}
