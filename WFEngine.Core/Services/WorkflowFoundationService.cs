using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFEngine.DataAccess;
using WFEngine.DataAccess.Models;
using WFEngine.Utility.Activity;
using WFEngine.Utility.Models;
using DM = WFEngine.DataAccess.Models;

namespace WFEngine.Core.Services
{
    public class WorkflowFoundationService : BaseService
    {
        public WorkflowFoundationService(WorkflowContext context)
        {
            base._context = context;
        }

        public string Start(string wfId, string listId, string itemId)
        {
            Workflow wf = base._context.Workflows.FirstOrDefault(w => w.WorkflowId == wfId);

            WorkflowModel wfmodel = JsonConvert.DeserializeObject<WorkflowModel>(wf.WorkflowJSON);

            NodeModel node = wfmodel.Nodes.First();
            List<string> users = AssignersActivity.FindUsers(node.Approvers);
            foreach (string user in users)
            {
                DM.Task task = new DM.Task();
                task.Author = "";
                task.Created = DateTime.Now;
                task.ItemId = itemId;
                task.ListId = listId;
                task.NodeId = node.Id;
                task.Status = Utility.TaskStatus.Started;
                task.TaskId = Guid.NewGuid().ToString();
                task.TaskName = node.Name;
                task.TaskUrl = "";
                task.WorkflowId = wf.WorkflowId;
                task.Assigner = user;
                base._context.Tasks.Add(task);

                if (node.IsSendEmail)
                {
                    base.SendEmail(task.Assigner,node.EmailTmpl);
                }
            }

            base._context.SaveChanges();

            return "";
        }

        public string Approve()
        {
            return "";
        }
    }
}
