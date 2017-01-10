using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ViewModel
{
    /// <summary>
    /// 非对称加密 model
    /// </summary>
    public class AsymEncryptModel
    {
        public string Name { get; set; }

        public int Age { get; set; }
        /// <summary>
        /// 时间戳（判断是否过期）
        /// </summary>
        public long Timestamp { get; set; }
        /// <summary>
        /// 签名（对摘要进行私钥加密）
        /// </summary>
        public string Sign { get; set; }
    }
}
