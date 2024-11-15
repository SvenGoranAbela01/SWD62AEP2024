using Domain.Models;

namespace Presentation.Models
{
    public class AttendancesListViewModel
    {
    public string SubjectFK { get; set; }
    public String SubjectName { get; set; }
    public DateTime Date { get; set; }

    public string Group { get; set; }
    }
}
