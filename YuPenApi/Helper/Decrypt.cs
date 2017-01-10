using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using DBUtility;

namespace Api.Helper
{
    /// <summary>
    /// 解密
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Decrypt<T>
    {
        //密钥
        public static readonly string DesKey = "a!s@d#f$";
        public T DecryptModel {
            get
            {
                if (string.IsNullOrEmpty(DecryptT))
                {
                    return default(T);
                }
                else
                {
                    var json = ApiHelper.DecryptDES(this.DecryptT, DesKey);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                }
            }
        }

        public string DecryptT { get; set; }


        
    }
}