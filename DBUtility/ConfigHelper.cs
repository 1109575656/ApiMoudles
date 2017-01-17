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
        /// <summary>
        /// 获取配置文件指定节点
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        public static string GetNodeByName(string nodeKey) {
            return ConfigurationManager.AppSettings[nodeKey];
        }
    }
}
