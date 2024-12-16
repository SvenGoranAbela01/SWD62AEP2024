using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class LogsFileRepository : ILogsRepository
    {
        private string _filename;
        public LogsFileRepository(IConfiguration configuration)
        {
            // Retrieve the filename from appsettings.json
            _filename = configuration["LogsFileName"];
            if (string.IsNullOrEmpty(_filename))
            {
                throw new ArgumentException("Log file path is not configured in appsettings.json.");
            }
        }

        public void AddLog(Log myLog)
        {
            // 1. Load Logs from the file as a list
            var listOfExistingLogs = LoadLogs().ToList();

            //2. add the new log to that list
            listOfExistingLogs.Add(myLog);

            //3. save back the entire list to the file
            string contents = JsonConvert.SerializeObject(listOfExistingLogs);
            System.IO.File.WriteAllText(_filename, contents);
        }

        public IQueryable<Log> LoadLogs()
        {
            //json { 'tmessage" : "adkjdlfakjdtfkajsdf", "timestamp":2024-12-16, ...}

            // 1. does the file exist?
            // 2. if no, return an empty Log list
            // 3. if yes, load the entire contents into a string
            // 4. deserialize the read string Into a List of Log

            if (System.IO.File.Exists(_filename) == false)
            {
                return new List<Log>().AsQueryable();
            }

            String contents = System.IO.File.ReadAllText(_filename);

            //using (var file = System.IO.File.OpenText(_filename))
            //{
            //    contents = file.ReadToEnd();
            //}

            var listOfLogs = JsonConvert.DeserializeObject<List<Log>>(contents);
            return listOfLogs.AsQueryable();
        }
    }
}
