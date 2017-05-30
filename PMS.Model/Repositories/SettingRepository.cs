﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public class SettingRepository : ISettingRepository
    {
        private readonly ApplicationContext context;

        public SettingRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public IEnumerable<Setting> GetSettings(Expression<Func<Setting, bool>> whereExpression)
        {
            throw new Exception();//return this.context.Settings.Where(whereExpression);
        }

        public Setting Get(int id)
        {
            throw new Exception(); //return this.context.Settings.SingleOrDefault(x => x.Id == id);
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
            throw new Exception(); //return this.context.Settings.Add(setting);
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
    }
}