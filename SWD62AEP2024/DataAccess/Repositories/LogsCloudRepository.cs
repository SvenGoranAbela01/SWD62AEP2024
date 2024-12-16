using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    internal class LogsCloudRepository : ILogsRepository
    {
        public void AddLog(Log myLog)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Log> LoadLogs()
        {
            throw new NotImplementedException();
        }
    }
}
