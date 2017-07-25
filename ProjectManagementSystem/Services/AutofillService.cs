using System.Collections.Generic;
using System.Linq;
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

        public List<AutofillViewModel> GetAll()
        {
            return _repository.Get(x => true).Select(x => new AutofillViewModel(x)).ToList();
        }

        public AutofillViewModel AddAutofill(Autofill autofill)
        {
            var result = new AutofillViewModel(_repository.Add(autofill));
            _repository.SaveChanges();
            return result;
        }

        public void Update(Autofill autofill)
        {
            var oldAutofill = GetAutofill(autofill.Id);
            _repository.Update(oldAutofill, autofill);
            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            var autofill = GetAutofill(id);
            _repository.Delete(autofill);
            _repository.SaveChanges();
        }

        private Autofill GetAutofill(int id)
        {
            return _repository.Get(x => x.Id == id).Single();
        }
    }
}
