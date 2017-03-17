using DBUtility;
using Newtonsoft.Json;

namespace Api.Helper
{
    /// <summary>
    ///     解密
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Decrypt<T>
    {
        //密钥
        public static readonly string DesKey = "a!s@d#f$";

        public T DecryptModel
        {
            get
            {
                if (string.IsNullOrEmpty(DecryptT))
                {
                    return default(T);
                }
                var json = ApiHelper.DecryptDES(DecryptT, DesKey);
                if (json.Equals(DecryptT))
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
        /// <summary>
        /// 服务器解析所需Key
        /// </summary>
        public string DecryptT { get; set; }
    }
}