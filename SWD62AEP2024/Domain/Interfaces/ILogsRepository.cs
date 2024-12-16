using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    //an interface dictates/forces any classes that will implement it to implement these methods
    // its like a contract

    //an interface holds only the method signatures not any implementation

    //an interface cannot be instantiated (nor created as an object)
    public interface ILogsRepository
    {
        void AddLog(Log myLog);
        IQueryable<Log> LoadLogs();
    }
}
