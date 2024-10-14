using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class StudentsRepository
    {
        private AttendanceContext myContext;

        //Constructor Injection
        public StudentsRepository(AttendanceContext _myContext) 
        {
            myContext = _myContext;
        }

        /// <summary>
        /// This method will return the entire list of students in my database
        /// </summary>
        /// <returns></returns>
        public IQueryable<Student> GetStudents()
        {
            return myContext.Students;
        }

        public Student GetStudent(string idcard)
        {
            return myContext.Students.SingleOrDefault(x=>x.IdCard == idcard);
        }

        public void UpdateStudent(Student student) { }
        public void AddStudent(Student student) 
        {
            myContext.Students.Add(student);
            myContext.SaveChanges();
        }
        public void DeleteStudent(string idcard) { }
    }
}
