using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface ISettingsValueProvider
    {
        string GetSettingValue(SettingType type);
    }

    public class SettingRepository : ISettingRepository, ISettingsValueProvider
    {
        private readonly ApplicationContext context;

        public SettingRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public string GetSettingValue(SettingType type)
        {
            var setting = this.context.Settings.SingleOrDefault(x => x.Type == type);
            return setting?.Value ?? setting?.DefaultValue;
        }

        public IEnumerable<Setting> GetSettings(Expression<Func<Setting, bool>> whereExpression)
        {
            return this.context.Settings.Where(whereExpression);
        }

        public Setting Get(int id)
        {
            return this.context.Settings.SingleOrDefault(x => x.Id == id);
        }

        public void UpdateSetting(Setting oldSetting, Setting setting)
        {
            oldSetting.Type = setting.Type;
            oldSetting.Value = setting.Value;
            oldSetting.DefaultValue = setting.DefaultValue;
            oldSetting.MaxValue = setting.MaxValue;
            oldSetting.MinValue = setting.MinValue;
            oldSetting.ValueType = setting.ValueType;
            oldSetting.ValueRegex = setting.ValueRegex;
            oldSetting.Name = setting.Name;
        }

        public Setting AddSetting(Setting setting)
        {
            return this.context.Settings.Add(setting);
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
    }
}
