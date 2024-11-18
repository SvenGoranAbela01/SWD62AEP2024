using Domain.Models;

namespace Presentation.Models
{
    public class CreateAttendanceViewModel
    {
        public List<Student> Students { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string groupCode { get; set; }
    }
}
