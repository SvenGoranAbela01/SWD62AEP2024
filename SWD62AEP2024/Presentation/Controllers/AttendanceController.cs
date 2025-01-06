using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Interfaces;
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
        ILogsRepository _logsRepository;

        public AttendanceController(AttendancesRepository attendancesRepositorty
            , GroupsRepository groupRepositorty
            , SubjectsRepository subjectRepositorty
            , StudentsRepository studentsRepository
            , ILogsRepository logsRepository
            ) 
        {
            _attendancesRepository = attendancesRepositorty;
            _groupsRepository = groupRepositorty;
            _subjectsRepository = subjectRepositorty;
            _studentsRepository = studentsRepository;
            _logsRepository = logsRepository;
        }

        //note: getting all attendances, but then extracting some information which i will put on screen
        //      being Subject Date, Group
        //... this requires me to create a viewmodel to accomodate this newly formed group of data
        public IActionResult Index()
        {
            //note: getting all attendances, but then extracting some information which i will put on screen
            //      being Subject Date, Group
            //... this requires me to create a viewmodel to accomodate this newly formed group of data

            _logsRepository.AddLog(new Log()
            {
                Message = "Accessed the Index method of the Attendence Controller",
                User = User.Identity.Name,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
            });

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

            //Time | Group | Subject
            List<SelectPastAttendancesViewModel> pastAttendances = _attendancesRepository.GetAttendances()
                 .GroupBy(x => new
                 {
                     Date = x.Timestamp,
                     SubjectCode = x.Subject.Code, //We cannot group by an entire object called subject
                     GroupCode = x.Student.GroupFK,
                     SubjectName = x.Subject.Name,
                 })
                  .Select(group => new SelectPastAttendancesViewModel()
                  {
                      SubjectCode = group.Key.SubjectCode,
                      Date = group.Key.Date,
                      GroupCode = group.Key.GroupCode,
                      SubjectName = group.Key.SubjectName,
                  }).
                 ToList();

            //foreach (var attendance in pastAttendances)
            //{
            //    attendance.SubjectName = _subjectsRepository.getSubjects().SingleOrDefault(x => x.Code == attendance.SubjectCode).Name;
            //}

            SelectGroupSubjectViewModel viewModel = new SelectGroupSubjectViewModel();
            viewModel.Subjects = subjects.ToList();
            viewModel.Groups = groups.ToList();
            viewModel.PastAttendances = pastAttendances;

            //Create a new ViewModel which accespts and laso a list of groups and a list of Subjects

            return View(viewModel);
        }

        [HttpGet]// it needs to show me which students are supposed to be in that attendance list
        public IActionResult Create(string groupCode, string subjectCode, string whichButton)
        {
            //todo:
            //a list of students pertaining to the group
            //group code
            //subject code/name

            if (whichButton == "0")
            {
                var students = _studentsRepository.GetStudents() //Select * from Sutdent
                            .Where(x => x.GroupFK == groupCode) //Select * from Students where GroupFK = groupCode
                            .OrderBy(x => x.LastName)// Select * From Students Where GroupFK = groupCode order by LastName
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

                ViewBag.update = false; //on the fly = it will be created when the application runs// another aproach to pass data to the views

                return View(viewModel);
            } 
            else
            {
                string[] myValues = whichButton.Split(new char[] {'|'});
                DateTime date = Convert.ToDateTime(myValues[0]);
                string selectedSubjectCode = myValues[1];
                string selectedGroupCode = myValues[2];

                CreateAttendanceViewModel myModel = new CreateAttendanceViewModel();
                myModel.SubjectCode = selectedSubjectCode;
                myModel.groupCode = selectedGroupCode;
                myModel.Students = _studentsRepository.GetStudents() //Select * from Sutdent
                            .Where(x => x.GroupFK == selectedGroupCode) //Select * from Students where GroupFK = groupCode
                            .OrderBy(x => x.LastName)// Select * From Students Where GroupFK = groupCode order by LastName
                            .ToList();//here is where the execution ie opening a connection to a db actually happens
                myModel.Attendances = _attendancesRepository.GetAttendances().Where(x=>x.SubjectFK == selectedSubjectCode && 
                x.Timestamp.Day == date.Day &&
                x.Timestamp.Month == date.Month &&
                x.Timestamp.Year == date.Year &&
                x.Timestamp.Hour == date.Hour&&
                x.Timestamp.Minute == date.Minute &&
                x.Student.GroupFK == selectedGroupCode
                ).OrderBy(x=>x.Student.LastName).ToList();

                ViewBag.update = true;

                return View(myModel);

            }

        }

        [HttpPost]//it saves the absents and presents of all the students from the first Create method
        public IActionResult Create(List<Attendance> attendances, bool update)
        {
            if (attendances.Count > 0)
            {
                if (update)
                {
                    _attendancesRepository.UpdateAttendances(attendances);
                }
                else
                {
                    _attendancesRepository.AddAttendances(attendances);
                }
                
                TempData["message"] = "Attendance Saved";
            }

            return RedirectToAction("Index");
        }
    }
}
