using System.Web;
using System.Web.Http;
using BusinessLayer.BaseRepository;

namespace Api.BaseController
{
    public class BaseApiControl<TModel, IRepository> : ApiController
        where TModel : class
        where IRepository : IGenericRepository<TModel>
    {
        /// <summary>
        ///     实例化仓储
        /// </summary>
        /// <param name="rep"></param>
        protected BaseApiControl(IRepository rep)
        {
            Repository = rep;
        }

        /// <summary>
        ///     接口当前仓储
        /// </summary>
        protected IRepository Repository { get; set; }

        /// <summary>
        ///     当前用户ID，NULL为未登陆
        /// </summary>
        public string IdentityId
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }
    }
}