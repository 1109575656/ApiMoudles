using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using DBUtility;

namespace Api.Log
{
    /// <summary>
    ///     请求响应日志
    /// </summary>
    public class ApiRecordLogAttribute : ActionFilterAttribute
    {
        //请求日志
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //系统维护中所有接口都请求失败，返回系统维护中(配置到web.config中)
            //actionContext.Response = actionContext.Request.CreateResponse(new ReturnMessage(ReturnMsgStatuEnum.Maintenance, "系统维护中", new { url = "http:www.baidu.com" }));
            //停用启用某个接口
            var req = HttpContext.Current.Request;
            var requestBody = ""; //Formbody参数（请求参数）
            try
            {
                //1M
                if (req.TotalBytes <= 1024*1024)
                {
                    using (var reader = new StreamReader(req.InputStream))
                    {
                        requestBody += reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                requestBody = ex.Message;
            }
            var customerId = HttpContext.Current.User.Identity.Name; //当前登录用户
            var url = actionContext.Request.RequestUri.AbsoluteUri; //接口绝对路径
            var httpMethod = actionContext.Request.Method.Method; //请求方法 Get/Post
            var ip = req.UserHostAddress; //ip
            var dns = req.UserHostName; //dns
            var version = "1.0"; //版本
            //....等等 可以记录到数据库，txt等
            var path =
                HttpContext.Current.Server.MapPath("~/ApiLog/RequestLog" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            var sb = new StringBuilder();
            sb.AppendLine("请求：");
            sb.AppendLine("当前登录用户：" + (string.IsNullOrEmpty(customerId) ? "未登录" : customerId));
            sb.AppendLine("接口绝对路径：" + url);
            sb.AppendLine("请求方法：" + httpMethod);
            sb.AppendLine("IP：" + ip);
            sb.AppendLine("DNS：" + dns);
            sb.AppendLine("version：" + version);
            ApiHelper.FileWrite(path, sb.ToString());
        }

        //响应日志
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var path =
                HttpContext.Current.Server.MapPath("~/ApiLog/RequestLog" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            var statu = (int) actionExecutedContext.Response.StatusCode; //返回状态码
            if (statu == 500)
            {
                ApiHelper.FileWrite(path, "内部服务器错误");
            }
            else
            {
                var result = actionExecutedContext.Response.Content.ReadAsStringAsync().Result; //响应结果
                var sb = new StringBuilder();
                sb.AppendLine("响应：" + result);
                ApiHelper.FileWrite(path, sb.ToString());
            }
        }
    }
}