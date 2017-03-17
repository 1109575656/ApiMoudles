using System.Web.Http;

namespace Api.BaseController
{
    /// <summary>
    ///     测试父类
    /// </summary>
    /// <typeparam name="TRepository"></typeparam>
    public class TestBaseApiControl<TRepository> : ApiController
        where TRepository : class
    {
        /// <summary>
        ///     实现仓储
        /// </summary>
        /// <param name="req"></param>
        protected TestBaseApiControl(TRepository req)
        {
            Repository = req;
        }

        /// <summary>
        ///     业务类
        /// </summary>
        protected TRepository Repository { get; set; }
    }
}