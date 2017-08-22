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
    public class WorkflowDesignerService: BaseService
    {
        public WorkflowDesignerService(WorkflowContext context)
        {
            base._context = context;
        }

        public string Add(WorkflowModel model)
        {
            Workflow wfmodel = new Workflow();
            wfmodel.WorkflowId = model.WorkflowId;
            wfmodel.ListId = model.ListId;
            wfmodel.ItemId = model.ItemId;
            wfmodel.Author = model.Author;
            wfmodel.Created = model.Created;
            wfmodel.WorkflowName = model.WorkflowName;
            wfmodel.WorkflowJSON = JsonConvert.SerializeObject(model);

            base._context.Add(wfmodel);
            base._context.SaveChanges();

            return string.Empty;
        }
    }
}
