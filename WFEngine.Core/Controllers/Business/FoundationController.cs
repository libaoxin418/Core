using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using WFEngine.Utility.Models;
using Newtonsoft.Json;
using System.Text;
using WFEngine.Core.Services;
using WFEngine.DataAccess;

namespace WFEngine.Core.Controllers.Business
{
    [Produces("application/json")]
    [Route("api/workflow")]
    public class FoundationController : Controller
    {
        private readonly WorkflowContext _context;

        public FoundationController(WorkflowContext context)
        {
            _context = context;
        }


        /// <summary>
        /// 启动工作流
        /// </summary>
        /// <param name="wfId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Start")]
        public HttpResponseMessage Start(string wfId, string listId, string itemId)
        {
            OpenApiResult<string> result = new OpenApiResult<string>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                wf.Start(wfId, listId, itemId);
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