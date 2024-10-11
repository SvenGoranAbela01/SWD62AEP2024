using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public string StudentFK { get; set; }
        public bool Present {  get; set; }
        public DateTime Timestamp {  get; set; }
        public string SubjectFK {  get; set; }
    }
}
