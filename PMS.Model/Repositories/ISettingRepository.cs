using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface ISettingRepository : IRepository
    {
        IEnumerable<Setting> GetSettings(Expression<Func<Setting, bool>> whereExpression);
        void UpdateSetting(Setting oldSetting, Setting setting);
        Setting AddSetting(Setting setting);
        Setting Get(int id);
    }
}