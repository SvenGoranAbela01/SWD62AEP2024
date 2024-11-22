using Domain.Models;

namespace Presentation.Models
{
    public class SelectGroupSubjectViewModel
    {
        public List<Subject> Subjects { get; set; }
        public List<Group> Groups { get; set; }
        public List<SelectPastAttendancesViewModel> PastAttendances { get; set; }

    }

    public class SelectPastAttendancesViewModel
    {
        public string GroupCode { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public DateTime Date { get; set; }
    }
}
