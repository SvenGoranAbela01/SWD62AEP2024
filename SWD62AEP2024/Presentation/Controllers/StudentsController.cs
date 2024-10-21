using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        //handle the redirection from the List > to the Edit page
        // the first one is going to load the existent details for the end user to see in the textboxes
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var student = _studentsRepository.GetStudent(id);
            if (student == null)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View(student);
            }
        }

        // handle the click of the Submit Changes buttonl
        [HttpPost]
        public IActionResult Update(Student student)
        {
            try
            {
                if (student == null)
                {
                    // Session - keeps the data in scope on the server-side only (so across controllers, etc)
                    // ViewData - keeps the data in scope between controller and view for 1 response only NOT a redirection
                    // ViewBag - exactly like ViewData however it allows to create variables on the fly
                    // TempData - this is like ViewData but the data inside it survives one redirection

                    TempData["error"] = "Id card no supplied does not exist";

                    return Redirect("Error");
                }
                else
                {
                    //Validations, sanitization of data
                    ModelState.Remove(nameof(Student.Group));

                    //this line will ensure that if there are validation policies (Centralized /not)
                    //applied, they will have to pass from here; it ensures that validations have been triggered
                    if (ModelState.IsValid)
                    {
                        _studentsRepository.UpdateStudent(student);
                        TempData["message"] = "Student was added successfully";

                        RedirectToAction("List");
                    }
                    //Add some error messages here
                    TempData["error"] = "Check your inputs";
                    return View("Edit", student);
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong. We are working on it";
                return Redirect("Error");
            }
        }
    }
}
