using DataAccess.DataContext;
using Domain.Models;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class LogsDbRepository : ILogsRepository
    {
        private AttendanceContext myContext;

        public LogsDbRepository(AttendanceContext _myContext) 
        { 
            myContext = _myContext;
        }

        public void AddLog(Log myLog)
        {
            myContext.Logs.Add(myLog);
            myContext.SaveChanges();
        }

        public IQueryable<Log> LoadLogs()
        {
            return myContext.Logs;
        }
    }
}
