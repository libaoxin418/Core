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
using Microsoft.AspNetCore.Cors;
using WFEngine.Core.Core;

namespace WFEngine.Core.Controllers.Designer
{
    [Route("api/workflow/Designer")]
    public class DesignerController : Controller
    {
        /// <summary>
        /// 添加工作流
        /// </summary>
        /// <param name="wfConfig"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public HttpResponseMessage Add([FromForm]string wfConfig)
        {
            OpenApiResult<string> result = new OpenApiResult<string>();

            try
            {
                WorkflowModel model = JsonConvert.DeserializeObject<WorkflowModel>(wfConfig);
                WorkflowDesigner wd = new WorkflowDesigner();
                wd.Add(model);
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