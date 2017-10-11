using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WFEngine.DataAccess;
using System.Net.Http;
using WFEngine.Utility.Models;
using Newtonsoft.Json;
using System.Text;

namespace WFEngine.Core.Controllers.Designer
{
    [Route("api/form/designer")]
    public class FormDesignerController : Controller
    {
        private readonly WorkflowContext _context;

        public FormDesignerController(WorkflowContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取指定APP的数据库连接字符串
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DBConnections")]
        public HttpResponseMessage DBConnections(string appId)
        {
            OpenApiResult<List<String>> result = new OpenApiResult<List<String>>();

            try
            {

            }
            catch (Exception ex)
            {
                result.Code = ResultStatus.Error;
                result.Message = ex.Message;
            }

            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(result),
                Encoding.GetEncoding("UTF-8"), "application/json")
            };

            return responseMessage;
        }

        /// <summary>
        /// 获取指定APP的Tables
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Tables")]
        public HttpResponseMessage Tables(string appId, string connectionId)
        {
            OpenApiResult<List<String>> result = new OpenApiResult<List<String>>();

            try
            {

            }
            catch (Exception ex)
            {
                result.Code = ResultStatus.Error;
                result.Message = ex.Message;
            }

            HttpResponseMessage responseMessage = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(result),
                Encoding.GetEncoding("UTF-8"), "application/json")
            };

            return responseMessage;
        }
    }
}