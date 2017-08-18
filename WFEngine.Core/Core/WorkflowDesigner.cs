using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFEngine.DataAccess;
using WFEngine.DataAccess.Models;
using WFEngine.Utility.Models;

namespace WFEngine.Core.Core
{
    public class WorkflowDesigner
    {
        public string Add(WorkflowModel model)
        {
            using (var context = new DataContext())
            {
                Workflow wfmodel = new Workflow();
                wfmodel.WorkflowId = model.WorkflowId;
                wfmodel.Author = model.Author;
                wfmodel.Created = model.Created;
                wfmodel.WorkflowName = model.WorkflowName;
                wfmodel.WorkflowJSON = JsonConvert.SerializeObject(model);

                context.Add(wfmodel);
                context.SaveChanges();
            }

            return string.Empty;
        }
    }
}
