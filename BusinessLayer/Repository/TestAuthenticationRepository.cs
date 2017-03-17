using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using BusinessLayer.IRepository;
using BusinessLayer.RequestModel;
using DataLayer.Model;
using DBUtility;
using Newtonsoft.Json;

namespace BusinessLayer.Repository
{
    public class TestAuthenticationRepository : ITestAuthenticationRepository
    {
        public ReturnMessage SignIn(SignInModel req)
        {
            if (req == null)
            {
                return new ReturnMessage(ReturnMsgStatuEnum.Failed, "参数有误或加密有误！", "");
            }
            if (req.LoginName != "1109575656" || req.Password != "1109575656")
            {
                 return new ReturnMessage(ReturnMsgStatuEnum.Failed, "账号密码有误！", "");
            }
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
        2, new Random().Next().ToString(), DateTime.Now, DateTime.Now.AddDays(1), true, JsonConvert.SerializeObject(req));
                string cookieValue = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue);
                cookie.HttpOnly = true;
                cookie.Secure = FormsAuthentication.RequireSSL;
                HttpContext context = HttpContext.Current;
                if (context == null)
                    throw new InvalidOperationException();
                //  写入Cookie
                context.Response.Cookies.Remove(cookie.Name);
                context.Response.Cookies.Add(cookie);
                return new ReturnMessage(ReturnMsgStatuEnum.Success, "登录成功", "");
            }


        public ReturnMessage SignOut()
        {
            FormsAuthentication.SignOut();
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Secure = FormsAuthentication.RequireSSL;
                cookie.Values.Clear();
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            return new ReturnMessage(ReturnMsgStatuEnum.Success, "退出成功", "");
        }
    }
    
}
