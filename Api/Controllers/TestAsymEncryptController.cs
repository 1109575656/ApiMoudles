using System;
using System.Collections.Generic;
using System.Web.Http;
using Api.BaseController;
using BusinessLayer.IRepository;
using BusinessLayer.Repository;
using BusinessLayer.ViewModel;
using DataLayer.Model;
using DBUtility;

namespace Api.Controllers
{
    /// <summary>
    ///  非对称加密
    /// </summary>
    [RoutePrefix("V1/Test")]
    [Authorize]
    public class TestAsymEncryptController : TestBaseApiControl<ITestAuthenticationRepository>
    {
        private readonly string privateKey =
            @"<RSAKeyValue><Modulus>trb7takt7C/wZALmO4Yy17yzjd6/MxCzOSYfBd0dHK6L1SYEgzhGldkSA4+sUeYwn3xqZe8vvRc8dzV0xsD/FtUQTpTrH7wnSgBmQKZ5UxdFwNIZWHWcR9YK43ilkA/2siRiQKNFLOPsOF0zKC5u+ir19bcQX2s1J1sVzImId/E=</Modulus><Exponent>AQAB</Exponent><P>5gaoGDbMYaSyAbkhYBW9FPv0EyxHw3c0AMEBTCDwrSTjrSil7svqBCXQrzwEFs2u+aICpR7yxAsd/kpQTdH+aw==</P><Q>y1i0b0nQWS4zp5B0FrggMaLSIZD3j4FUCb013T8gMJoob+hqXuxCtDxUk8Wa2HGwBxfjWoYAYkPeTGsoPtFCEw==</Q><DP>vdLydwEJyu6B44AmdceawS1m70eUdU7ywEiGTI/GbexKYwRvYtAub3vRajrp2POmGOXErwUKLBRMjSRAfufzvw==</DP><DQ>pbp9DDqnoRdjqAy2YJHeQzYFdq/05DOub2WTUeeR76qkjFhq4URDNSv6bpldk0xM/+r7NBsEkxHnSncHTPM1mw==</DQ><InverseQ>BF9Wtqn8sn4dJH9qSWQbWb7SFyexmAd8IewHQLW49GgXT3Ch/BLYjeIaI3XeieSPKfZXmddfz0+nbYf2RcNasQ==</InverseQ><D>DNgaI7AL4WGRVYZ6ps6NPmsueBejezR+VNMgNSpRBJYkkExG3u6Sz6/du1BbPbqfymZVmGrTAUjj4EFqvxoMFHFY2/sjdKggRZl2E3abaAHN6yoqsj2kIzRs5CQCb6mZDdvP0kUBT3TAFQ2vU8WUVCUI6dXsAEVrpLVXADBz8I0=</D></RSAKeyValue>";

        private string publicKey =
            @"<RSAKeyValue><Modulus>trb7takt7C/wZALmO4Yy17yzjd6/MxCzOSYfBd0dHK6L1SYEgzhGldkSA4+sUeYwn3xqZe8vvRc8dzV0xsD/FtUQTpTrH7wnSgBmQKZ5UxdFwNIZWHWcR9YK43ilkA/2siRiQKNFLOPsOF0zKC5u+ir19bcQX2s1J1sVzImId/E=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        public TestAsymEncryptController() : base(new TestAuthenticationRepository())
        {
        }

        /// <summary>
        ///     测试非对称加密
        /// </summary>
        /// <param name="req">{"Name":"张三","Age":"12","Timestamp":"636196450812777599","Sign":"eAEgBKbAAG0L8aLRNt0QVcgR/uBQ2+UgPr9hJFyeWhE5OWuGfSZEV696um2uJs7qjzS2umlBUF9TYx2lphI6/VUjiSBqlTX0b8kfL83d/5wzmM/r5GIGkReRZUoYQnwn8Be1FK+k8H2dMtrUuOepp43KSTYRGmfMW9+3RXoM410="}</param>
        /// <returns></returns>
        [Route("AsymEncrypt")]
        [HttpPost]
        [AllowAnonymous]
        public ReturnMessage AsymEncrypt(AsymEncryptModel req)
        {
            //加密事例：
            //string publicKey = @"<RSAKeyValue><Modulus>trb7takt7C/wZALmO4Yy17yzjd6/MxCzOSYfBd0dHK6L1SYEgzhGldkSA4+sUeYwn3xqZe8vvRc8dzV0xsD/FtUQTpTrH7wnSgBmQKZ5UxdFwNIZWHWcR9YK43ilkA/2siRiQKNFLOPsOF0zKC5u+ir19bcQX2s1J1sVzImId/E=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            //DateTime dt = DateTime.Now;
            //string name = "张三";
            //int age = 12;
            //long _timestamp = dt.Ticks;
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("Name", name);
            //dic.Add("Age", age.ToString());
            //dic.Add("Timestamp", _timestamp.ToString());
            //string md5EncryptStr = EncryptHelper.MessageDigest(dic); //获得摘要
            //string rsaEncrypt = EncryptHelper.RSAEncrypt(publicKey, md5EncryptStr); //rsa加密
            if (req == null)
            {
                return new ReturnMessage(ReturnMsgStatuEnum.Failed, "", null);
            }
            //时间戳转换为datetime
            var requestTime = new DateTime(req.Timestamp);
            //判断请求是否过期---假设过期时间是20秒
            var addReqTime = requestTime.AddSeconds(60);
            //if (addReqTime < new DateTime(2017,1,10,11,37,42))
            if (addReqTime < DateTime.Now)
            {
                return new ReturnMessage(ReturnMsgStatuEnum.Failed, "请求超时！", null);
            }
            //使用公钥解密
            var decryptStr = ApiHelper.RSADecrypt(privateKey, req.Sign);
            var parameters = new Dictionary<string, string>();
            parameters.Add("Name", req.Name);
            parameters.Add("Age", req.Age.ToString());
            parameters.Add("Timestamp", req.Timestamp.ToString());
            var sign = ApiHelper.MessageDigest(parameters);
            //验签
            if (!decryptStr.Equals(sign))
            {
                return new ReturnMessage(ReturnMsgStatuEnum.Failed, "验签失败！", null);
            }
            return new ReturnMessage(ReturnMsgStatuEnum.Success, "验签成功！参数正确", null);
        }
    }
}