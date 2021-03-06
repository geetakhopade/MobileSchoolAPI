﻿using MobileSchoolAPI.Models;
using MobileSchoolAPI.ParamModel;
using MobileSchoolAPI.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileSchoolAPI.BusinessLayer
{
    public class UserWiseAttendance
    {
        
        public object GetAttendanceByUser(ParamDateWiseAttendance obj)
        {
            try
            {
                SchoolMainContext db = new ConcreateContext().GetContext(obj.UserId, obj.Password);
                var usertype= db.VW_GET_USER_TYPE.Where(r => r.UserId == obj.UserId ).ToList();
                if(usertype.Count()==0)
                {
                    return new AttendanceResult() { IsSuccess = false, UserWiseAttendanceList = "User Not Found" };
                }

                if (usertype[0].UserType == "STUDENT")
                {
                    var checkattendace = db.VIewAttendaceClasswiseChecks.Where(r => r.UserId == obj.UserId && r.ATTEDANCEDATE == obj.AttendanceDate && r.DISPLAY == 1 && r.EDUCATIONYEAR == "2018-2019" && r.ACADEMICYEAR == "2018-2019").ToList();
                    if (checkattendace.Count() == 0)
                    {
                        return  new AttendanceResult() { IsSuccess = true, UserWiseAttendanceList = "Status : Attendance Is Not Marked By Class Teacher For This Date" };
                    }
                    else
                    { 
                        var StudentAttendance = db.VWATTENDANCEBYDATESTUDENTs.Where(r => r.UserId == obj.UserId && r.ATTEDANCEDATE == obj.AttendanceDate).ToList();
                        if (StudentAttendance.Count() == 0)
                        {
                            return new AttendanceResult() { IsSuccess = true, UserWiseAttendanceList = "Status : Present" };
                            //EMPLOYEE logic
                            // return new Error() { IsError = true, Message = "Attendance not found" };
                        }
                        else
                        {
                            return new AttendanceResult() { IsSuccess = true, UserWiseAttendanceList = "Status : Absent" };
                        }
                    }

                }
                else
                {
                    var EMPATTENDANCE = db.VWATTENDANCEEMPLOYEEs.Where(r => r.UserId == obj.UserId && r.ATTEDANCEDATE == obj.AttendanceDate && r.DISPLAY == 1).ToList();
                    if (usertype[0].UserType == "CLASS TEACHER")

                    {
                        if (EMPATTENDANCE.Count() == 0)
                        {
                            return new AttendanceResult() { IsSuccess = true, UserWiseAttendanceList = "Status : Attendance Not Completed" };

                        }
                        else
                        {
                            return new AttendanceResult() { IsSuccess = true, UserWiseAttendanceList = "Status : Attendance Completed" };
                        }
                    }
                    else
                    {
                        return new AttendanceResult()
                        {
                            IsSuccess = true,
                            UserWiseAttendanceList = "Status : User is not class Teacher"

                        };
                    }
                }
                           
                
            }
            catch(Exception e)
            {
                return new Error() { IsError = true, Message = e.Message };

            }
        }
    }
}