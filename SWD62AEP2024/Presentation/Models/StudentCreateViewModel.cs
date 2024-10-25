using Domain.Models;

namespace Presentation.Models
{
    //ViewModels >> are models which help transfer data to and from the views
    //Models >> are models which will be used to design and 

    public class StudentCreateViewModel
    {
        public Student Student { get; set; }
        public List<Group> Groups { get; set; }
    }
}
