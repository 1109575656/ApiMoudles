﻿using DBUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace Api.Log
{
    public class ExceptionLogAttribute : ExceptionFilterAttribute
    { 
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //先在Api中建立LogError文件夹
            string path = HttpContext.Current.Server.MapPath("~/LogError/LogError" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("===============================================================================================================================");
            sb.AppendLine("时间：" + DateTime.Now + "-------------------------------------------");
            sb.AppendLine("ErrorMessage：" + actionExecutedContext.Exception.Message);
            sb.AppendLine("ErrorSource：" + actionExecutedContext.Exception.Source);
            sb.AppendLine("异常方法：" + actionExecutedContext.Exception.TargetSite);
            sb.AppendLine("ErrorStackTrace：" + actionExecutedContext.Exception.StackTrace);
            ApiHelper.FileWrite(path, sb.ToString());
        }
    }
}