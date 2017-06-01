using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Model.CommonModels
{
    public class FileInfoDisplayModel
    {
        public int Id { get; set; }
        public string FullPath { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Size { get; set; }
    }
}
