using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using DataLayer.Model;

namespace Api.Log
{
    /// <summary>
    /// 权限控制
    /// </summary>
    public class AuthorityAttribute : ActionFilterAttribute
    {
        //请求日志
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (System.Web.HttpContext.Current.User.Identity.Name != "1109575656")
            {
                actionContext.Response = actionContext.Request.CreateResponse(new ReturnMessage(ReturnMsgStatuEnum.Failed, "您没有权限访问", null));
            }
        }
    }
}