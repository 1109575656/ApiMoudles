using Api.BaseController;
using BusinessLayer.IRepository;
using BusinessLayer.Repository;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Controllers
{
    /// <summary>
    /// 测试BaseController
    /// </summary>
    [RoutePrefix("V1/TestBase")]
    public class TestBaseController : BaseApiControl<Customer, ICustomerRepository>
    {
        public TestBaseController():base(new CustomerRepository()) {}
        /// <summary>
        /// 测试查询
        /// </summary>
        /// <returns></returns>
        [Route("TestSelect")]
        [HttpGet]
        public ReturnMessage TestSelect()
        {
            return this.Repository.TestSelect();
        }
        /// <summary>
        /// 测试添加
        /// </summary>
        /// <returns></returns>
        [Route("TestAdd")]
        [HttpGet]
        public ReturnMessage TestAdd() {
            return this.Repository.TestAdd();
        }
        /// <summary>
        /// 测试删除
        /// </summary>
        /// <returns></returns>
        [Route("TestDelete")]
        [HttpGet]
        public ReturnMessage TestDelete()
        {
            return this.Repository.TestDelete();
        }
        /// <summary>
        /// 测试修改
        /// </summary>
        /// <returns></returns>
        [Route("TestUpdate")]
        [HttpGet]
        public ReturnMessage TestUpdate()
        {
            return this.Repository.TestUpdate();
        }
    }
}
