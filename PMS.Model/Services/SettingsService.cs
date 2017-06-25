using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class SettingsService
    {
        private readonly ISettingRepository _repository;

        public SettingsService(ISettingRepository repository)
        {
            this._repository = repository;
        }

        public List<Setting> GetSettings()
        {
            return this._repository.GetSettings(x => true).ToList();
        }

        public void UpdateSetting(Setting setting)
        {
            var oldSetting = this._repository.Get(setting.Id);
            if(oldSetting == null)
                throw new PmsException($"Настройка с идентификатором {setting.Id} не найдена");
            oldSetting.Value = setting.Value;
            this._repository.SaveChanges();
        }
    }
}
