using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Presentation.Models;
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
        public IActionResult Update(Student student, IFormFile file, [FromServices] GroupsRepository groupRepository, [FromServices] IWebHostEnvironment host)
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
                    ModelState.Remove("file");

                    //file upload
                    if (file != null)
                    {
                        string filename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
                        string absolutePath = host.WebRootPath + "\\images\\" + filename;

                        using (var f = System.IO.File.Create(absolutePath))
                        {
                            file.CopyTo(f);
                        }

                        string relativePath = "\\images\\" + filename;
                        student.ImagePath = relativePath;
                    }


                    //this line will ensure that if there are validation policies (Centralized /not)
                    //applied, they will have to pass from here; it ensures that validations have been triggered
                    if (ModelState.IsValid)
                    {
                        _studentsRepository.UpdateStudent(student);
                        TempData["message"] = "Student was added successfully";

                        return RedirectToAction("List");
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

        [HttpGet] //used to load the page with empty textboxes
        public IActionResult Create([FromServices] GroupsRepository groupRepository)
        {

            //eventually: we need to fetch a list of existing groups the end user can select from

            var myGroups = groupRepository.getGroups();

            //How are we going to pass the myGroups into the View?
            //Approach 1 - we can pass a model into the View where we create a ViewModel
            //problem is: you cannot pass IQueryable<Group> model into Student model
            StudentCreateViewModel myModel = new StudentCreateViewModel();
            myModel.Groups = myGroups.ToList();
            //  myModel.Student = new Student();

            return View(myModel);

            //Approach 2



        }

        [HttpPost] //is triggered by the submit button of the form
        public IActionResult Create(Student s, IFormFile file, [FromServices] GroupsRepository groupRepository, [FromServices] IWebHostEnvironment host)
        {

            if (_studentsRepository.GetStudent(s.IdCard) != null)
            {
                TempData["error"] = "Student already exists";
                return RedirectToAction("List");
            }
            else
            {
                ModelState.Remove(nameof(Group));
                ModelState.Remove("file");

                //this line will ensure that if there are validation policies (Centralized /not)
                //applied, they will have to pass from here; it ensures that validations have been triggered
                if (ModelState.IsValid)
                {
                    //file upload
                    if(file != null)
                    {
                        string filename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
                        string absolutePath = host.WebRootPath + "\\images\\" + filename;

                        using (var f = System.IO.File.Create(absolutePath))
                        {
                            file.CopyTo(f);
                        }

                        string relativePath = "\\images\\" + filename;
                        s.ImagePath = relativePath;
                    }

                    //save the details in db
                    _studentsRepository.AddStudent(s);
                    TempData["message"] = "Student was added successfully";

                    return RedirectToAction("List");
                }

                //add some error messages here
                TempData["error"] = "Check your inputs";

                //populating a StudentCreateViewModel
                var myGroups = groupRepository.getGroups();
                StudentCreateViewModel myModel = new StudentCreateViewModel();
                myModel.Groups = myGroups.ToList();
                myModel.Student = s; //why do i assign Student s that was submitted in this method?
                                     //passing the same instance back to the page
                                     //so that I show the end-user the same data he/she gave me

                return View(myModel); //will be looking for a view as the action name.....Create
            }
        }
    
        public IActionResult Delete(string id)
        {
            _studentsRepository.DeleteStudent(id);
            TempData["message"] = "Student with id " + id + " was deleted sucessfully";

            return RedirectToAction("List");
        }
    }
}
