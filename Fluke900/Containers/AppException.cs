using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class AppException : System.Exception
    {
        private Exception _exception = null;
        private DateTime _throwDateTime = DateTime.Now;

        public Exception Exception { get { return _exception; } }
        public DateTime ThrownDateTime { get { return _throwDateTime; } }

        public AppException(Exception ex)
        {
            _exception = ex;
        }
    }
}
