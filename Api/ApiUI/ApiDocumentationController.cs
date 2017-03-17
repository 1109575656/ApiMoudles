using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using Alan.WebApiDoc;
using Newtonsoft.Json;

namespace Api.ApiUI
{
    /// <summary>
    ///     接口文档
    /// </summary>
    /// <note>用于本页面的接口信息获取</note>
    /// <permission>匿名用户</permission>
    [RoutePrefix("Api/ApiDocumentation")]
    public class ApiDocumentationController : ApiController
    {
        private List<DocumentModel.MemberNode> GetDocs
        {
            get
            {
                var docModels = HttpContext.Current.Cache.Get("_ApiDocumentModels") as List<DocumentModel.MemberNode>;
                if (docModels == null)
                {
                    var doc =
                        DocumentModel.GetDocument(HttpContext.Current.Server.MapPath("~/App_Data/Api.XML"))
                            .membersNode.Members;
                    HttpContext.Current.Cache.Add("_ApiDocumentModels", doc, null, DateTime.MaxValue,
                        TimeSpan.FromDays(1),
                        CacheItemPriority.Default, null);
                    docModels = doc;
                }
                return docModels;
            }
        }

        private List<ApiDescriptionModel> GetApis
        {
            get
            {
                var apiModels = HttpContext.Current.Cache.Get("_ApiDescriptionModel") as List<ApiDescriptionModel>;
                if (apiModels == null)
                {
                    var doc = GetDocs;
                    var apis = ApiDescriptionModel.GetAllApis();

                    var relatives =
                        GlobalConfiguration.Configuration.Services.GetApiExplorer()
                            .ApiDescriptions.Where(i => i.RelativePath.ToLower().Contains("toppro"))
                            .ToList();

                    var docCtrls = doc.Where(d => d.IsType).ToList();
                    var apiCtrls = apis.Select(a => a.ControllerName).Distinct().ToList();

                    apis.ForEach(api =>
                    {
                        //if (!String.IsNullOrWhiteSpace(api.AddressInAssembly) && api.AddressInAssembly.Contains("UserController.Change"))
                        //{
                        //    var breakpoint = "";
                        //}
                        api.BindDocModel(doc);
                        api.Parameters.ForEach(para =>
                        {
                            var t = para.ParaType;
                            if (t != null)
                            {
                                try
                                {
                                    var instance = Activator.CreateInstance(t);
                                    para.Format = JsonConvert.SerializeObject(instance);
                                }
                                catch (Exception ex)
                                {
                                    para.Format = ex.Message;
                                }
                            }
                        });
                    });
                    HttpContext.Current.Cache.Add("_ApiDescriptionModel", apis, null, DateTime.MaxValue,
                        TimeSpan.FromDays(1),
                        CacheItemPriority.Default, null);
                    apiModels = apis;
                }
                return apiModels.OrderBy(a => a.ControllerName).ToList();
            }
        }

        public object Get()
        {
            var apis = GetApis
                .Select(a => a.ControllerName)
                .Distinct()
                .Select(ctrlName => Get(ctrlName)).ToList();
            return apis;
        }

        public object Get(string id)
        {
            var apis = GetApis.Where(api => api.ControllerName == id).ToList();

            var controller =
                GetDocs.FirstOrDefault(c => c.IsType && c.Name.EndsWith(string.Format(".{0}Controller", id)));
            return new {apis, controller};
        }

        [HttpGet]
        [Route("ThrowException")]
        public HttpResponseMessage ThrowException()
        {
            throw new Exception("ThrowException");
            return Request.CreateResponse();
        }
    }
}