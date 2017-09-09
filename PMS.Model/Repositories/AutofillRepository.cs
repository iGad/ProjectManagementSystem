using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
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
            IQueryable<Autofill> query = _context.Autofills;
            if (!string.IsNullOrWhiteSpace(filterModel.Name))
            {
                query = query.Where(x => x.Name.Contains(filterModel.Name));
            }
            if (!string.IsNullOrWhiteSpace(filterModel.Description))
            {
                query = query.Where(x => x.Description.Contains(filterModel.Description));
            }
            if (filterModel.WorkItemTypes.Any())
            {
                query = query.Where(x => filterModel.WorkItemTypes.Contains(x.WorkItemType));
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
