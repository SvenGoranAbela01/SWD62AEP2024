using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class AttendanceController : Controller
    {
        AttendancesRepository _attendancesRepository;
        GroupsRepository _groupsRepository;
        SubjectsRepository _subjectsRepository;
        StudentsRepository _studentsRepository;

        public AttendanceController(AttendancesRepository attendancesRepositorty
            , GroupsRepository groupRepositorty
            , SubjectsRepository subjectRepositorty
            , StudentsRepository studentsRepository
            ) 
        {
            _attendancesRepository = attendancesRepositorty;
            _groupsRepository = groupRepositorty;
            _subjectsRepository = subjectRepositorty;
            _studentsRepository = studentsRepository;
        }

        //note: getting all attendances, but then extracting some information which i will put on screen
        //      being Subject Date, Group
        //... this requires me to create a viewmodel to accomodate this newly formed group of data
        public IActionResult Index()
        {
            //note: getting all attendances, but then extracting some information which i will put on screen
            //      being Subject Date, Group
            //... this requires me to create a viewmodel to accomodate this newly formed group of data

            //a history of attendances
            //var groupedAttendances = _attendancesRepository.GetAttendances()
            //        .GroupBy(a => new {
            //            a.SubjectFK,
            //            a.Student.GroupFK,
            //            a.Timestamp.Date,
            //            Subject = a.Subject.Name,
            //        })
            //        .OrderByDescending(x => x.Key.Date)
            //        .Select(g => new AttendancesListViewModel()   //anonymous type
            //        {
            //            SubjectFK = g.Key.SubjectFK,
            //            SubjectName = g.Key.Subject,
            //            Date = g.Key.Date,
            //            Group = g.Key.GroupFK
            //        })
            //        .ToList();

            //todo: 
            //we need to pass to the page a list of Groups, a list of Subjects we have
            //GroupsRepository >> GetGroups
            // SubjectsRepository >> GetSubjects

            var subjects = _subjectsRepository.getSubjects().ToList();
            var groups = _groupsRepository.getGroups().ToList();

            SelectGroupSubjectViewModel viewModel = new SelectGroupSubjectViewModel();
            viewModel.Subjects = subjects;
            viewModel.Groups = groups;

            //Create a new ViewModel which accespts and laso a list of groups and a list of Subjects

            return View(viewModel);
        }

        [HttpGet]// it needs to show me which students are supposed to be in that attendance list
        public IActionResult Create(string groupCode, string subjectCode)
        {
            //todo:
            //a list of students pertaining to the group
            //group code
            //subject code/name

            var students = _studentsRepository.GetStudents() //Select * from Sutdent
                            .Where(x=>x.GroupFK == groupCode) //Select * from Students where GroupFK = groupCode
                            .OrderBy(x=>x.LastName)// Select * From Students Where GroupFK = groupCode order by LastName
                            .ToList();//here is where the execution ie opening a connection to a db actually happens

            CreateAttendanceViewModel viewModel = new CreateAttendanceViewModel();
            viewModel.SubjectCode = subjectCode;
            viewModel.Students = students;
            viewModel.groupCode = groupCode;

            var mySubject = _subjectsRepository.getSubjects()
                            .SingleOrDefault(x => x.Code == subjectCode);

            if (mySubject == null)
            {
                viewModel.SubjectName = string.Empty; // we throw an exception, we do exception handling or redirect the user to an error page
            }
            else
            {
                viewModel.SubjectName = mySubject.Name;
            }

            return View(viewModel);
        }

        [HttpPost]//it saves the absents and presents of all the students from the first Create method
        public IActionResult Create(List<Attendance> attendances)
        {
            if (attendances.Count > 0)
            {
                _attendancesRepository.AddAttendances(attendances);
                TempData["message"] = "Attendance Saved";
            }

            return RedirectToAction("Index");
        }
    }
}
