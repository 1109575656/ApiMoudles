using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLayer.Model;
using BusinessLayer.Repository;
using System.Linq.Expressions;

namespace YuPenApi.Controllers
{
    public  class BaseApiControl<TModel, IRepository> : ApiController
        where TModel : class
        where IRepository : IGenericRepository<TModel>
    {
        /// <summary>
        /// 接口当前仓储
        /// </summary>
        protected IRepository Repository { get; set; }


        /// <summary>
        /// 当前用户ID，NULL为未登陆
        /// </summary>
        public string IdentityId
        {
            get { return System.Web.HttpContext.Current.User.Identity.Name; }
        }

        /// <summary>
        /// 实例化仓储
        /// </summary>
        /// <param name="rep"></param>
        protected BaseApiControl(IRepository rep)
        {
            this.Repository = rep;
        }
    }
}