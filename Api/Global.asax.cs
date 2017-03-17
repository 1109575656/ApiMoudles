using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using Api.Log;

namespace Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //异常日志
            GlobalConfiguration.Configuration.Filters.Add(new ExceptionLogAttribute());
            //请求响应日志
            GlobalConfiguration.Configuration.Filters.Add(new ApiRecordLogAttribute());
        }
    }
}