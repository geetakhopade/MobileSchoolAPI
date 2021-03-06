﻿using MobileSchoolAPI.Models;
using MobileSchoolAPI.ParamModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileSchoolAPI.BusinessLayer
{
    public class GetSubjectListBusiness
    {
       


        public object GetSubjectList(ParamDIVISIONWISESUBJECT objdiv)
        {
            try
            {
                SchoolMainContext db = new ConcreateContext().GetContext(objdiv.userid, objdiv.password);
                var SubjectList = db.VIEWDIVISIONWISESUBJECTs.Where(r => r.DIVISIONID == objdiv.divisionid &&  r.DISPLAY == 1 && r.UserId==objdiv.userid).ToList();
                if (SubjectList.Count == 0)
                {
                var StudSubjectList = db.VIEWDIVISIONWISESUBJECTSTUDENTs.Where(r => r.DIVISIONID == objdiv.divisionid &&  r.DISPLAY == 1 && r.UserId==objdiv.userid).ToList();
                    if (StudSubjectList.Count == 0)
                    {
                        return new Error() { IsError = true, Message = "Subject Not Found" };

                    }
                    else
                    {
                        return new DivisionListResult() { IsSuccess = true, SubjectList = StudSubjectList };
                       

                    }



                }
                else
                {
                    return new DivisionListResult() { IsSuccess = true, SubjectList = SubjectList };
                    
                }
            }
            catch (Exception E)
            {
                return new Error()
                {
                    IsError = true,
                    Message = E.Message

                };
            }
        }
    }
}