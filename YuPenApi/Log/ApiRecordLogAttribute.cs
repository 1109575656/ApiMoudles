using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using DataLayer.Model;

namespace YuPenApi.Log
{
    /// <summary>
    /// 请求响应日志
    /// </summary>
    public class ApiRecordLogAttribute : ActionFilterAttribute
    {
        //请求日志
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //系统维护中所有接口都请求失败，返回系统维护中(配置到web.config中)
            //actionContext.Response = actionContext.Request.CreateResponse(new ReturnMessage(ReturnMsgStatuEnum.Maintenance, "系统维护中", new { url = "http:www.baidu.com" }));
            //停用启用某个接口
            HttpRequest req = HttpContext.Current.Request;
            var requestBody = ""; //Formbody参数（请求参数）
            try
            {
                //1KB
                if (req.TotalBytes <= 1024 * 1024 * 10)
                {
                    using (StreamReader reader = new StreamReader(req.InputStream))
                    {
                        requestBody += reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                requestBody = ex.Message;
            }
            var customerId = System.Web.HttpContext.Current.User.Identity.Name; //当前登录用户
            var url = actionContext.Request.RequestUri.AbsoluteUri; //接口绝对路径
            var httpMethod = actionContext.Request.Method.Method; //请求方法 Get/Post
            //....等等 可以记录到数据库，txt等
            string version = "1.0"; //版本
        }
         //响应日志
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var statu = (int)actionExecutedContext.Response.StatusCode; //返回状态码
            string result = actionExecutedContext.Response.Content.ReadAsStringAsync().Result; //响应结果
        }
    }
}