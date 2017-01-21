using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Model.Services
{
    public class PmsExeption : Exception
    {
        public PmsExeption()
        {
            
        }

        public PmsExeption(string message) : base(message)
        {
            
        }

        public PmsExeption(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
