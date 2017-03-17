using System.Web.Http;
using Api.BaseController;
using Api.Helper;
using BusinessLayer.IRepository;
using BusinessLayer.Repository;
using BusinessLayer.RequestModel;
using DataLayer.Model;

namespace Api.Controllers
{
    /// <summary>
    ///     测试身份认证和授权
    /// </summary>
    [RoutePrefix("V1/TestAuthentication")]
    public class TestAuthenticationController : TestBaseApiControl<ITestAuthenticationRepository>
    {
        /// <summary>
        ///     实现
        /// </summary>
        public TestAuthenticationController() : base(new TestAuthenticationRepository())
        {
        }

        /// <summary>
        ///     登陆（DES对称加密，密钥：a!s@d#f$）
        /// </summary>
        /// <param name="req">{"LoginName":"1109575656","Password":"1109575656"} 加密后：{"DecryptT":"NkZSq7z1Q7HmOcyEyFBAwweysI/HDm/aQt+8R+cYpxZY4Zrj5W4qh3YatzB7B7yp78Z0OHpFvHE="}</param>
        /// <returns></returns>
        [Route("SignIn")]
        [HttpPost]
        public ReturnMessage SignIn(Decrypt<SignInModel> req)
        {
            return Repository.SignIn(req.DecryptModel);
        }

        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <returns></returns>
        [Route("SignIn")]
        [HttpGet]
        [Authorize]
        public ReturnMessage SignOut()
        {
            return Repository.SignOut();
        }
    }
}