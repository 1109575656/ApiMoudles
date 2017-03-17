using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Api.Helper;
using BusinessLayer.IRepository;
using BusinessLayer.Repository;
using BusinessLayer.ViewModel;
using DataLayer.Model;

namespace Api.Controllers
{
    /// <summary>
    /// 测试DES对称加密
    /// </summary>
    [RoutePrefix("V1/TestSymmetric")]
    public class TestSymmetricController : BaseApiControl<Cj_Customer, ITestRepository>
    {
        public TestSymmetricController() : base(new TestRepository()) { }
        /// <summary>
        /// 测试Post请求（对称加密）
        /// </summary>
        /// <param name="test">{"Id":1,"Name":"123"}</param>
        /// <returns></returns>
        [Route("SymEncrypt")]
        [HttpPost]
        [AllowAnonymous]
        public ReturnMessage SymEncrypt(Decrypt<SymEncryptModel> test)
        {
            return this.Repository.Test(test.DecryptModel);
        }
    }
}
