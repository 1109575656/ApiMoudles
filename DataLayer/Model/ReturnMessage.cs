using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    /// <summary>
    /// 返回状态码 枚举
    /// </summary>
    public enum ReturnMsgStatuEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success=0,
        /// <summary>
        /// 失败
        /// </summary>
        Failed = 1,
        /// <summary>
        /// 维护中
        /// </summary>
        Maintenance =2
    }
    /// <summary>
    /// 接口返回值
    /// </summary>
    public class ReturnMessage
    {
        /// <summary>
        /// 状态
        /// </summary>
        public ReturnMsgStatuEnum Statu { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 接口返回值
        /// </summary>
        /// <param name="statu">状态码</param>
        /// <param name="remarks">备注</param>
        /// <param name="data">数据</param>
        public ReturnMessage(ReturnMsgStatuEnum statu, string remarks, object data)
        {
            this.Statu = statu;
            this.Remarks = remarks;
            this.Data = data;
        }
        public static ReturnMessage Success(string remarks,object data) {
              return new ReturnMessage(ReturnMsgStatuEnum.Success, remarks, data);
        }
        public static ReturnMessage Failed(string remarks, object data)
        {
            return new ReturnMessage(ReturnMsgStatuEnum.Failed, remarks, data);
        }
        public static ReturnMessage Maintenance(string remarks, object data)
        {
            return new ReturnMessage(ReturnMsgStatuEnum.Maintenance, remarks, data);
        }
    }
}
