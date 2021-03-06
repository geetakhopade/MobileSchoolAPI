﻿using MobileSchoolAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileSchoolAPI.BUSINESSLAYER
{
    public class GetTeacherInfoBusiness
    {
        
        public object getTeacherInfo(int empcode, int UserId,string Password)
        {
            SchoolMainContext db = new ConcreateContext().GetContext(UserId, Password);
            var result = db.VW_EMPLOYEE.Where(r => r.ID == empcode && r.UserId == UserId).FirstOrDefault();

            if (result == null)
            {
                return new Error() { IsError = true, Message = "User Not Found" };
            }
            else
            {
                return result;
            }
       
            
        }
        public object getTeacherLogo(int empcode, int UserId,string Password)
        {
            SchoolMainContext db = new ConcreateContext().GetContext(UserId, Password);
            var result = db.VW_EMPLOYEE.Where(r => r.ID == empcode && r.UserId == UserId).FirstOrDefault();

            if (result == null)
            {
                return result;
            }
            else
            {
                return result.IMAGEPATH;
            }


        }
    }
}