using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Response
{
    public class Error
    {
        public string ResponseCode { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionMessage { get; set; }
        public string Message { get; set; }
    }
}
