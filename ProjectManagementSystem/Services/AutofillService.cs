using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMS.Model.CommonModels;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;
using PMS.Model.Repositories;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Services
{
    public class AutofillService
    {
        private readonly IAutofillRepository _repository;

        public AutofillService(IAutofillRepository repository)
        {
            _repository = repository;
        }

        public async Task<TableCollectionModel<AutofillViewModel>> GetAutofillList(AutofillFilterModel filterModel)
        {
            var model = new TableCollectionModel<AutofillViewModel>
            {
                Collection = (await _repository.Get(filterModel)).Select(x => new AutofillViewModel(x)).ToList(),
                TotalCount = await _repository.GetTotalCount(filterModel)
            };
            return model;
        }

        public async Task<AutofillViewModel> AddAutofill(Autofill autofill)
        {
            var result = new AutofillViewModel(_repository.Add(autofill));
            await _repository.SaveChangesAsync();
            return result;
        }

        public async Task Update(Autofill autofill)
        {
            var oldAutofill = await GetAutofill(autofill.Id);
            oldAutofill.WorkItemType = autofill.WorkItemType;
            oldAutofill.Description = autofill.Description;
            oldAutofill.Name = autofill.Name;
            await _repository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var autofill = await GetAutofill(id);
            _repository.Delete(autofill);
            _repository.SaveChanges();
        }

        private async Task<Autofill> GetAutofill(int id)
        {
            return await _repository.Get(id);
        }
    }
}
