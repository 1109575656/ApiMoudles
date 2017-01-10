using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace YuPenApi.Log
{
    public class ExceptionLogAttribute : ExceptionFilterAttribute
    { 
        /// <summary>
        /// 写入文件,不存在则创建文件
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <param name="content">写入内容</param>
        /// <param name="IsAppend">true：追加 false：覆盖</param>
        /// <returns>true：成功 false：失败</returns>
        public bool FileWrite(string fileName, string content, bool IsAppend = true)
        {
            ////文件夹是否存在，不存在则创建
            //if (Directory.Exists(HttpContext.Current.Server.MapPath("~/LogError")) == false)//如果不存在就创建file文件夹
            //{
            //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/LogError"));
            //}
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, IsAppend, Encoding.Default))
                {
                    sw.Write(content);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //先在YuPenApi中建立LogError文件夹
            string path = HttpContext.Current.Server.MapPath("~/LogError/LogError" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("===============================================================================================================================");
            sb.AppendLine("时间：" + DateTime.Now + "-------------------------------------------");
            sb.AppendLine("ErrorMessage：" + actionExecutedContext.Exception.Message);
            sb.AppendLine("ErrorSource：" + actionExecutedContext.Exception.Source);
            sb.AppendLine("异常方法：" + actionExecutedContext.Exception.TargetSite);
            sb.AppendLine("ErrorStackTrace：" + actionExecutedContext.Exception.StackTrace);
            this.FileWrite(path, sb.ToString());
        }
    }
}