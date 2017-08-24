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
using DM = WFEngine.DataAccess.Models;

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

        /// <summary>
        /// 获取任务待办
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Tasks")]
        public HttpResponseMessage Tasks(string user)
        {
            OpenApiResult<IEnumerable<DM.Task>> result = new OpenApiResult<IEnumerable<DM.Task>>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                result.Code = ResultStatus.Success;
                result.Data = wf.QueryTask(user);
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
        /// 获取历史记录
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("History")]
        public HttpResponseMessage History(string instanceId)
        {
            OpenApiResult<string> result = new OpenApiResult<string>();

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
        /// 强制终止工作流
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Terminate")]
        public HttpResponseMessage Terminate(string instanceId)
        {
            OpenApiResult<string> result = new OpenApiResult<string>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                wf.Terminate(instanceId);
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