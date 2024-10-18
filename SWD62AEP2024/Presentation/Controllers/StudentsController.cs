using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    //as a good practice of how we are structuing our architecture :

    //keep the repository classes to interact directly with the database;
    //keep the controllers to handle requests/responses i.e. user input and then sanitaze accordingly
    // in other words do not make any calls directly to the database in the controller

    public class StudentsController : Controller
    {
        private StudentsRepository _studentsRepository;

        //Constructor Injection
        public StudentsController(StudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
        }
        public IActionResult List()
        {
            var list = _studentsRepository.GetStudents();
            return View(list);//we are passing into the View the ist containing the fetched students
        }
    }
}
