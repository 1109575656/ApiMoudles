﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using YuPenApi.Log;

namespace YuPenApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //异常日志
            GlobalConfiguration.Configuration.Filters.Add(new ExceptionLogAttribute()); 
            //请求响应日志
            GlobalConfiguration.Configuration.Filters.Add(new ApiRecordLogAttribute());
        }
    }
}