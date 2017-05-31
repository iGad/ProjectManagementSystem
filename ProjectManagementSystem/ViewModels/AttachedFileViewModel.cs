using PMS.Model.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ProjectManagementSystem.ViewModels
{
    public class AttachedFileViewModel
    {
        public AttachedFileViewModel() { }
        public AttachedFileViewModel(AttachedFile file)
        {
            Id = file.Id;
            FullName = file.FullName;
            Name = Path.GetFileName(FullName);
            //Size = 
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Size { get; set; }
    }
}