using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string IpAddress { get; set; }
        public string Message { get; set; }
    }
}
