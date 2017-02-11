using System;
using PMS.Model;

namespace ProjectManagementSystem.ViewModels
{
    public class EnumViewModel<T> where T: struct, IComparable
    {
        public EnumViewModel(T value)
        {
            Value = value;
            Description = (value as Enum).GetDescription();
            SystemName = (value as Enum).ToString();
        } 
        public string Description { get; set; }
        public string SystemName { get; set; }
        public T Value { get; set; }
    }
}