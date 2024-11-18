using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class SubjectsRepository
    {
        private AttendanceContext myContext;

        //Constructor Injection
        public SubjectsRepository(AttendanceContext _myContext)
        {
            myContext = _myContext;
        }

        public IQueryable<Subject> getSubjects()
        {
            return myContext.Subjects;
        }

        //note: to practice at home, you can implement all the CRUD functions
        //note: if you would like to consume the C U D, repeat the steps we applied for Student entity
        //note: after the above... implement a SubjectsControtter Index, Create, Update, Detetel
    }
}
