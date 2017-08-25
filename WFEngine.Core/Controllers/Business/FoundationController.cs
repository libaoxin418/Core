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
                result.Code = ResultStatus.Success;
                result.Data = wf.Start(wfId, listId, itemId);
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
        /// 审批工作流
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="nodeid"></param>
        /// <param name="assigner"></param>
        /// <param name="outcome"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Approve")]
        public HttpResponseMessage Approve(string instanceId, string nodeid, string assigner, int outcome)
        {
            OpenApiResult<string> result = new OpenApiResult<string>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                result.Code = ResultStatus.Success;
                result.Data = wf.Approve(instanceId, nodeid, assigner, outcome);
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
        /// 强制终止工作流，终止正在运行的流程。适应有权限人的人特殊处理
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
                result.Code = ResultStatus.Success;
                result.Data = wf.Terminate(instanceId);
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
        /// 挂起工作流
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Suspend")]
        public HttpResponseMessage Suspend(string instanceId)
        {
            OpenApiResult<string> result = new OpenApiResult<string>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                result.Code = ResultStatus.Success;
                result.Data = wf.Suspend(instanceId);
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
        /// 恢复工作流
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Resume")]
        public HttpResponseMessage Resume(string instanceId)
        {
            OpenApiResult<string> result = new OpenApiResult<string>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                result.Code = ResultStatus.Success;
                result.Data = wf.Resume(instanceId);
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
        /// 工作流回退
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Return")]
        public HttpResponseMessage Return(string instanceId, string nodeid, int outcome)
        {
            OpenApiResult<string> result = new OpenApiResult<string>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                result.Code = ResultStatus.Success;
                result.Data = wf.Return(instanceId, nodeid, outcome);
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
        /// 工作流加签
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("NodeAdd")]
        public HttpResponseMessage NodeAdd(string instanceId, string nodeid, int outcome, bool prev = false)
        {
            OpenApiResult<string> result = new OpenApiResult<string>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                result.Code = ResultStatus.Success;
                result.Data = wf.NodeAdd(instanceId, nodeid, outcome, prev);
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
        /// 取消工作流,工作流第一级还没审批可以取消。只要有审批就不能取消
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Cancel")]
        public HttpResponseMessage Cancel(string instanceId)
        {
            OpenApiResult<string> result = new OpenApiResult<string>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                result.Code = ResultStatus.Success;
                result.Data = wf.Cancel(instanceId);
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
        [Route("TaskCollection")]
        public HttpResponseMessage TaskCollection(string user)
        {
            OpenApiResult<IEnumerable<DM.Task>> result = new OpenApiResult<IEnumerable<DM.Task>>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                result.Code = ResultStatus.Success;
                result.Data = wf.QueryAllTask(user);
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
        [Route("Task")]
        public HttpResponseMessage Task(string taskId)
        {
            OpenApiResult<DM.Task> result = new OpenApiResult<DM.Task>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                result.Code = ResultStatus.Success;
                result.Data = wf.QueryTask(taskId);
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
        [Route("HistoryCollection")]
        public HttpResponseMessage HistoryCollection(string instanceId)
        {
            OpenApiResult<IEnumerable<DM.Task>> result = new OpenApiResult<IEnumerable<DM.Task>>();

            try
            {
                WorkflowFoundationService wf = new WorkflowFoundationService(_context);
                result.Code = ResultStatus.Success;
                result.Data = wf.QueryAllHistory(instanceId);
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