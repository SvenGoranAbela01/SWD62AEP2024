﻿using DataAccess.DataContext;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AttendancesRepository
    {
        private AttendanceContext _attendanceContext;

        public AttendancesRepository( AttendanceContext attendanceContext)
        {
            _attendanceContext = attendanceContext;
        }

        public void AddAttendance(Attendance a)
        {
            a.Timestamp = DateTime.Now;

            _attendanceContext.Attendances.Add(a);
            _attendanceContext.SaveChanges();
        }

        public void AddAttendances(List<Attendance> attendances)
        {
            var currentTime = DateTime.Now; // time is taken once

            foreach (var a in attendances)
            {
                a.Timestamp = currentTime;
                a.SubjectFK = a.SubjectFK.Trim();
                _attendanceContext.Attendances.Add(a);
            }

            _attendanceContext.SaveChanges();
        }

        public IQueryable<Attendance> GetAttendances(DateTime date, string groupCode, string subjectCode)
        {
            return _attendanceContext.Attendances.Where(x =>
            x.Timestamp.Day == date.Day 
            && x.Timestamp.Month == date.Month
            && x.Timestamp.Year == date.Year
            && x.SubjectFK == subjectCode
            && x.Student.GroupFK == groupCode
            );
        }

        public IQueryable<Attendance> GetAttendances()
        {
            return _attendanceContext.Attendances;
        }

        public void UpdateAttendances(List<Attendance> attendances)
        {
            foreach (var a in attendances)
            {
                var oldRecord = GetAttendances().SingleOrDefault(x => x.Id == a.Id);
                oldRecord.Present = a.Present;
            }
            _attendanceContext.SaveChanges();
        }
    }

}
