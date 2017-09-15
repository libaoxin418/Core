using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFEngine.DataAccess;
using WFEngine.DataAccess.Models;
using WFEngine.Utility.Models;

namespace WFEngine.Core.Services
{
    public class WorkflowDesignerService : BaseService
    {
        public WorkflowDesignerService(WorkflowContext context)
        {
            base._context = context;
        }

        public string Add(WorkflowModel model)
        {
            Workflow wfmodel = new Workflow();
            wfmodel.WorkflowId = model.WorkflowId;
            wfmodel.Author = model.Author;
            wfmodel.Created = model.Created;
            wfmodel.WorkflowName = model.WorkflowName;
            wfmodel.WorkflowJSON = JsonConvert.SerializeObject(model);

            base._context.Add(wfmodel);
            base._context.SaveChanges();

            return string.Empty;
        }

        /// <summary>
        /// 只保存邮件模板
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public string SaveEmailTemplate(string contents)
        {
            return string.Empty;
        }

        /// <summary>
        /// 邮件模板和工作流简历关联关系
        /// </summary>
        /// <returns></returns>
        public string BindEmailWorkflow()
        {
            return string.Empty;
        }

        /// <summary>
        /// 只保存脚本代码
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public string SaveScript(string contents, ScriptMode mode)
        {
            return string.Empty;
        }

        /// <summary>
        /// 脚本和工作流建立关系
        /// </summary>
        /// <returns></returns>
        public string BindScriptWorkflow()
        {
            return string.Empty;
        }
    }
}
