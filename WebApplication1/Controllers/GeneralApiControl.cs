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

namespace WebApplication1.Controllers
{
    public partial class GeneralApiControl<TModel, IRepository> : ApiController
        where TModel : class
        where IRepository : IGenericRepository<TModel>
    {
        /// <summary>
        /// 接口当前仓储
        /// </summary>
        public IRepository repository;

        /// <summary>
        /// 当前用户ID，NULL为未登陆
        /// </summary>
        public string UserId
        {
            get { return System.Web.HttpContext.Current.User.Identity.Name; }
        }

        /// <summary>
        /// 实例化仓储
        /// </summary>
        /// <param name="rep"></param>
        public GeneralApiControl(IRepository rep)
        {
            this.repository = rep;
        }
    }
}