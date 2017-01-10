using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLayer.Repository;
using DataLayer.Model;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// 产品中心
    /// </summary>
    [RoutePrefix("Api/ProductCenter")]
    public class ProductCenterController : GeneralApiControl<ProductCenter, IProductCenterRepository>
    {

        public ProductCenterController() : base(new ProductCenterRepository()) { }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [Route("GetData")]
        [HttpGet]
        public ReturnMessage GetData()
        {
            return this.repository.GetData();
        }
    }
}
