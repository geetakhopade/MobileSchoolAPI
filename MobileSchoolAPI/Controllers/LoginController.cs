﻿using MobileSchoolAPI.BusinessLayer;
using MobileSchoolAPI.BUSINESSLAYER;
using MobileSchoolAPI.Models;
using MobileSchoolAPI.ParamModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MobileSchoolAPI.Controllers
{
    public class LoginController : ApiController
    {
		// GET api/values
		 
		/// <summary>	
		/// To Confirm Login UserName and Password
		/// If passed then Json object return else Error message 
		/// </summary>
		/// <param name="UserName"></param>
		/// <param name="Password"></param>
		/// <returns></returns>

		[HttpPost]
		public object Confirm([FromBody]ParamLogin userLogin)
		{
            try
            {
                string TeacherBaseUrl = "";
                string StudentBaseUrl = "";

                LoginManager objLogin = new LoginManager();
                var logindetail = objLogin.GetLoginDetails(userLogin);
                if (logindetail == null)
                    return new Error() { IsError = true, Message = "User Name & Password is Incorrect" };
                else
                {
                    if (logindetail.UserType == "STUDENT")
                    {
                        STUDENTINFO_BUSINESS StudBL = new STUDENTINFO_BUSINESS();
                        var result = StudBL.getStudLogo(int.Parse(logindetail.EmpCode),Convert.ToInt16( logindetail.UserId),logindetail.Password);
                        if (result == null)
                        {
                        }
                        else
                        {
                            logindetail.IMAGEPATH = (string)result;
                        }
                        if (logindetail.UserName.StartsWith("NKV"))
                        {
                            StudentBaseUrl = ConfigurationManager.AppSettings["NkvsBaseUrlStudent"];
                        }
                        else if(logindetail.UserName.StartsWith("SXS"))
                        {
                            StudentBaseUrl = ConfigurationManager.AppSettings["StxavierBaseUrlStudent"];
                        }
                        logindetail.BaseURL = StudentBaseUrl;
                    }
                    else
                    {
                        GetTeacherInfoBusiness TeacherBL = new GetTeacherInfoBusiness();
                        var result=TeacherBL.getTeacherLogo(int.Parse(logindetail.EmpCode),Convert.ToInt16( logindetail.UserId),logindetail.Password);
                        if (result==null)
                        {
                        }
                        else
                        {
                            logindetail.IMAGEPATH = (string)result;
                        }
                        if (logindetail.UserName.StartsWith("NKV"))
                        {
                            TeacherBaseUrl = ConfigurationManager.AppSettings["NkvsBaseUrlTeacher"];

                        }
                        else if(logindetail.UserName.StartsWith("SXS"))
                        {
                            TeacherBaseUrl = ConfigurationManager.AppSettings["StxavierBaseUrlTeacher"];
                        }
                        logindetail.BaseURL = TeacherBaseUrl;
                    }
                    DeviceBusinessLayer objDeviceBl = new DeviceBusinessLayer();
                    ParamDevice PDeviceObj = new ParamDevice();
                    PDeviceObj.UserId = (int)logindetail.UserId;
                    PDeviceObj.DeviceId = userLogin.DeviceId;
                    PDeviceObj.DeviceType = userLogin.DeviceType;
                    objDeviceBl.SaveDevice(PDeviceObj,logindetail.Password);

                    //logindetail.DeviceId = userLogin.DeviceId;
                    //logindetail.DeviceType = userLogin.DeviceType;

                    return logindetail;
                }
            }
            catch (Exception ex)
            {
                return new Error() { IsError = true, Message = ex.Message };
            }
		}
	}
}
