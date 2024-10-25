using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GroupsRepository
    {
        private AttendanceContext myContext;

        //Constructor Injection
        public GroupsRepository(AttendanceContext _myContext)
        {
            myContext = _myContext;
        }

        public IQueryable<Group> getGroups()
        {
            return myContext.Groups;
        }
    }
}
