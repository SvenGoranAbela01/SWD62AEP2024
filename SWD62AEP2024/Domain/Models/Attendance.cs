using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        public string StudentFK { get; set; }
        [ForeignKey("StudentFK")]
        public virtual Student Student { get; set; }
        public bool Present {  get; set; }
        public DateTime Timestamp {  get; set; }
        public string SubjectFK {  get; set; }

        [ForeignKey("SubjectFK")]
        public virtual Subject Subject { get; set; }
    }
}
