using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class LogsRepository
    {
        private AttendanceContext myContext;

        public LogsRepository(AttendanceContext _myContext) 
        { 
            myContext = _myContext;
        }

        public void AddLog(Log myLog)
        {
            myContext.Logs.Add(myLog);
            myContext.SaveChanges();
        }

    }
}
