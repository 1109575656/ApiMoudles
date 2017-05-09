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
        /// 测试添加
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpGet]
        public ReturnMessage Add() {
            return this.Repository.Add();
        }
    }
}
