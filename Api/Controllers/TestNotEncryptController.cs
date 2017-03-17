using System.Web.Http;
using BusinessLayer.ViewModel;
using DataLayer.Model;

namespace Api.Controllers
{
    /// <summary>
    ///     测试不用加密 and 不用登陆 的方法
    /// </summary>
    [RoutePrefix("V1/TestNotEncrypt")]
    public class TestNotEncryptController : ApiController
    {
        //参数不区分大小写
        /// <summary>
        ///  测试Get请求
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TestGetResponse")]
        [AllowAnonymous]
        public ReturnMessage TestGetResponse()
        {
            return new ReturnMessage(ReturnMsgStatuEnum.Success, "成功", "");
        }

        /// <summary>
        ///     测试Post请求多个参数（不用加密）
        /// </summary>
        /// <param name="req">{"Id":"1","Name":"***"}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("TestPostResponse")]
        [AllowAnonymous]
        public ReturnMessage TestPostResponse(SymEncryptModel req)
        {
            return new ReturnMessage(ReturnMsgStatuEnum.Success, "成功", new {req.Id, req.Name});
        }

        /// <summary>
        ///     测试POST一个参数
        /// </summary>
        /// <param name="name">{"name":"xx"}</param>
        /// <returns></returns>
        [HttpPost]
        [Route("TestStringParameter")]
        [AllowAnonymous]
        public ReturnMessage TestStringParameter(dynamic name)
        {
            //web api方法只有一个参数时，默认为fromuri参数。
            //[frombody]：没有key，key为"",value为web api方法参数/string,json...
            return new ReturnMessage(ReturnMsgStatuEnum.Success, name.name.ToString(), name.name);
        }
    }
}