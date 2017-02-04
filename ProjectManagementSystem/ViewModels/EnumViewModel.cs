using System;
using PMS.Model;

namespace ProjectManagementSystem.ViewModels
{
    public class EnumViewModel<T> where T: struct, IComparable
    {
        public string Description { get; set; }
        public T Value { get; set; }
    }
}