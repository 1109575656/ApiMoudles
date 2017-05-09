using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DBUtility
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public  class ApiHelper
    {
        #region RSA加密解密
        /// <summary>
        /// 生成私钥和公钥
        /// </summary>
        public static void CreateKey() {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            string publicPrivate = provider.ToXmlString(true); // 获得私钥
            string publicOnly = provider.ToXmlString(false); // 只获得公钥 
        }
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publicKeyXml"></param>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string publicKeyXml, string plainText)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(publicKeyXml); // 使用公钥初始化对象
            byte[] plainData = Encoding.Default.GetBytes(plainText);
            byte[] encryptedData = provider.Encrypt(plainData, true);
            return Convert.ToBase64String(encryptedData);
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privateKeyXml"></param>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public static string RSADecrypt(string privateKeyXml, string encryptedText)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(privateKeyXml); // 使用公/私钥对初始化对象
            byte[] encryptedData = Convert.FromBase64String(encryptedText);
            byte[] plainData = provider.Decrypt(encryptedData, true);
            string plainText = Encoding.Default.GetString(plainData);
            return plainText;
        }
        #endregion

        #region MD5
        //对参数散列加密（消息摘要），返回摘要值
        public static string MessageDigest(Dictionary<string, string> parameters)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var p in parameters)
            {
                sb.Append(p.Key).Append(p.Value);
            }
            return GetMD5(sb.ToString());
        }
        //md5散列加密
        private static string GetMD5(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            try
            {
                var sb = new StringBuilder(32);
                var md5 = System.Security.Cryptography.MD5.Create();
                var output = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                for (int i = 0; i < output.Length; i++)
                    sb.Append(output[i].ToString("X").PadLeft(2, '0'));
                return sb.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region DES加密解密字符串
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string key)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key);
                byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="key">解密密钥，要求8位</param>
        /// <returns></returns>
        public static string DecryptDES(string decryptString, string key)
        {
            try
            {
                //默认密钥向量
                byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] rgbKey = Encoding.UTF8.GetBytes(key);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        #endregion

        #region 此处用来判断是否为合格的身份证号 (CheckIDCard18)
        public static string IDCard15To18(string oldIDCard)
        {
            int iS = 0;

            //加权因子常数
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //校验码常数
            string LastCode = "10X98765432";
            //新身份证号
            string newIDCard;

            newIDCard = oldIDCard.Substring(0, 6);
            //填在第6位及第7位上填上‘1’，‘9’两个数字
            newIDCard += "19";

            newIDCard += oldIDCard.Substring(6, 9);

            //进行加权求和
            for (int i = 0; i < 17; i++)
            {
                iS += int.Parse(newIDCard.Substring(i, 1)) * iW[i];
            }

            //取模运算，得到模值
            int iY = iS % 11;
            //从LastCode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。
            newIDCard += LastCode.Substring(iY, 1);
            return newIDCard;
        }
        public static bool CheckIDCard18(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return false;
            }
            if (Id.Length != 18)
            {
                return false;
            }
            long n = 0;

            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {

                return false;//数字验证

            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {

                return false;//省份验证

            }

            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");

            DateTime time = new DateTime();

            if (DateTime.TryParse(birth, out time) == false)
            {

                return false;//生日验证

            }

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');

            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');

            char[] Ai = Id.Remove(17).ToCharArray();

            int sum = 0;

            for (int i = 0; i < 17; i++)
            {

                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());

            }

            int y = -1;

            Math.DivRem(sum, 11, out y);

            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {

                return false;//校验码验证

            }

            return true;//符合GB11643-1999标准

        }
        public static bool CheckIDCard15(string Id)
        {

            long n = 0;

            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {

                return false;//数字验证

            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {

                return false;//省份验证

            }

            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");

            DateTime time = new DateTime();

            if (DateTime.TryParse(birth, out time) == false)
            {

                return false;//生日验证

            }

            return true;//符合15位身份证标准

        }

        #endregion

        #region  Xml帮助方法
        /// <summary>
        /// 根据XML文件相对路径和节点名称获取节点值
        /// </summary>
        /// <param name="relativeFile">文件相对路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public static string GetXmlValueByFilePath(string relativeFile, string nodeName)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string absolutePath = System.Web.HttpContext.Current.Server.MapPath(relativeFile);
                xmlDoc.Load(absolutePath);
                XmlNode root = xmlDoc.SelectSingleNode(nodeName);
                return root == null ? "" : root.InnerText;
            }
            catch (Exception)
            {
                return "";
            }
        }

        #endregion

        #region 加密字符串 MD5
        #region 利用 MD5 加密算法加密字符串
        /// <summary>
        /// 利用 MD5 加密算法加密字符串
        /// </summary>
        /// <param name="src">字符串源串</param>
        /// <returns>返加MD5 加密后的字符串</returns>
        public static string ComputeMD5(string src)
        {
            //将密码字符串转化成字节数组
            byte[] byteArray = GetByteArray(src);

            //计算 MD5 密码
            byteArray = (new MD5CryptoServiceProvider().ComputeHash(byteArray));

            //将字节码转化成字符串并返回
            return BitConverter.ToString(byteArray);
        }

        /// <summary>
        /// 将指定串加密为不包含中杠的MD5值
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <param name="isupper">返回值的大小写(true大写,false小写)</param>
        /// <returns></returns>
        public static string ComputeMD5(string str, bool isupper)
        {
            string md5str = ComputeMD5(str);
            if (isupper)
                return md5str.ToUpper();
            return md5str.ToLower();
        }
        #endregion

        #region 将字符串翻译成字节数组
        /// <summary>
        /// 将字符串翻译成字节数组
        /// </summary>
        /// <param name="src">字符串源串</param>
        /// <returns>字节数组</returns>
        private static byte[] GetByteArray(string src)
        {
            byte[] byteArray = new byte[src.Length];

            for (int i = 0; i < src.Length; i++)
            {
                byteArray[i] = Convert.ToByte(src[i]);
            }

            return byteArray;
        }
        #endregion

        #region MD5string

        //public static string MD5string(string str)
        //{
        //    return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        //}

        //public static string MD5string(string str, bool isupper)
        //{
        //    string md5string = MD5string(str);
        //    if (isupper)
        //        return md5string.ToUpper();
        //    else
        //        return md5string.ToLower();
        //}

        #endregion

        #endregion

        #region SQL注入过滤
        /// <summary>
        ///SQL注入过滤
        /// </summary>
        /// <param name="InText">要进行过滤的字符串</param>
        /// <returns>如果参数存在不安全字符,则返回true</returns>
        public static bool SqlFilter2(string InText)
        {
            string word = "exec|insert|select|delete|update|chr|mid|master|or|truncate|char|declare|join";
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 将指定的str串执行sql关键字过滤并返回
        /// </summary>
        /// <param name="str">要过滤的字符串</param>
        /// <returns></returns>
        public static string SqlFilter(string str)
        {
            return str.Replace("'", "").Replace("&#39;", "").Replace("--", "").Replace("&", "").Replace("/*", "").Replace(";", "").Replace("%", "");
        }

        /// <summary>
        /// 将指定的串列表执行sql关键字过滤并以[,]号分隔返回
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string SqlFilters(params string[] strs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string str in strs)
            {
                sb.Append(SqlFilter(str) + ",");
            }
            if (sb.Length > 0)
                return sb.ToString().TrimEnd(',');
            return "";
        }

        public static bool ProcessSqlStr(string Str)
        {
            bool ReturnValue = false;
            try
            {
                if (Str != "")
                {
                    string SqlStr = "&#39;|insert|select*|and'|or'|insertinto|deletefrom|altertable|update|createtable|createview|dropview|createindex|dropindex|createprocedure|dropprocedure|createtrigger|droptrigger|createschema|dropschema|createdomain|alterdomain|dropdomain|);|select@|declare@|print@|char(|select";
                    string[] anySqlStr = SqlStr.Split('|');
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.IndexOf(ss) >= 0)
                        {
                            ReturnValue = true;
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }


        #endregion

        #region MD5string

        public static string MD5string(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }

        public static string MD5string(string str, bool isupper)
        {
            string md5string = MD5string(str);
            if (isupper)
                return md5string.ToUpper();
            else
                return md5string.ToLower();
        }

        #endregion

        #region 写入文件
        /// <summary>
        /// 写入文件,不存在则创建文件
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <param name="content">写入内容</param>
        /// <param name="IsAppend">true：追加 false：覆盖</param>
        /// <returns>true：成功 false：失败</returns>
        public static bool FileWrite(string fileName, string content, bool IsAppend = true)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, IsAppend, Encoding.Default))
                {
                    sw.Write(content);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
