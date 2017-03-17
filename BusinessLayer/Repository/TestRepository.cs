using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using BusinessLayer.IRepository;
using BusinessLayer.RequestModel;
using BusinessLayer.ViewModel;
using DataLayer.Model;
using Dapper;
using DBUtility;
using Newtonsoft.Json;

namespace BusinessLayer.Repository
{

    public class TestRepository : GenericRepository<Cj_Customer>, ITestRepository
    {
        public ReturnMessage GetData()
        {
            //连接串   dapper查询返回结果
            IDbConnection conn = new SqlConnection("server=192.168.100.37;database=YuPenDB;User Id=sa;pwd=eage123!@#"); 
            return new ReturnMessage(ReturnMsgStatuEnum.Success, "", conn.Query<dynamic>("select * from ProductCenter"));  //查询的表
            //linq to sql 查询返回结果
            //return new ReturnMessage(ReturnMsgStatuEnum.Success,"",this.Context.ProductCenter.ToList()); 
        }

        public ReturnMessage Test(SymEncryptModel test) 
        {
            if (test == null)
            {
                return new ReturnMessage(ReturnMsgStatuEnum.Failed, "参数不得为空！", null);
            }
            return new ReturnMessage(ReturnMsgStatuEnum.Success, "",test);
        }

        public ReturnMessage SignIn(SignInModel req)
        {
            var customer = this.Context.Cj_Customer.FirstOrDefault(m => m.LoginName == req.LoginName);
            if (customer != null)
            {
                if (customer.LoginPass == ApiHelper.MD5string(req.Password + customer.PassSalt))
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            2, customer.KeyId.ToString(), DateTime.Now, DateTime.Now.AddDays(1), true, JsonConvert.SerializeObject(req));
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
            }
            return new ReturnMessage(ReturnMsgStatuEnum.Failed, "账号密码有误！", "");
        }
    }
}
