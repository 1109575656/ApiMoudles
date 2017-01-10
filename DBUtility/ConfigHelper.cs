using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtility
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public class ConfigHelper
    {
        public static string SSOPath
        {
            get
            {
                return ConfigurationManager.AppSettings["SSOPath"];
            }
        }
        public static string Domain
        {
            get
            {
                return ConfigurationManager.AppSettings["Domain"];
            }
        }
    }
}
