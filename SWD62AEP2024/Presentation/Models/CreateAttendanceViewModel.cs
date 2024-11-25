using Domain.Models;

namespace Presentation.Models
{
    public class CreateAttendanceViewModel
    {
        public CreateAttendanceViewModel() 
        {
            Presence = new List<bool>();
        }

        public List<Student> Students { get; set; }
        public List<bool> Presence { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string groupCode { get; set; }
    }
}
