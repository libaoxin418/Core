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

        /// <summary>
        /// 关联工作流和列表
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="listId"></param>
        /// <returns></returns>
        public string Associate(string wfId, string listId)
        {
            WorkflowAssociation association = new WorkflowAssociation();
            association.ListId = listId;
            association.WorkflowId = wfId;

            base._context.WorkflowAssociations.Add(association);
            base._context.SaveChanges();

            return "";
        }

        /// <summary>
        /// 启动工作流，创建实例并且启动一个任务
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="listId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public string Start(string wfId, string listId, string itemId)
        {
            Workflow wf = base._context.Workflows.FirstOrDefault(w => w.WorkflowId == wfId);

            //创建实例
            WorkflowInstance wInstance = new WorkflowInstance();
            wInstance.InstanceId = Guid.NewGuid().ToString();
            wInstance.ListId = listId;
            wInstance.ItemId = itemId;
            wInstance.WorkflowId = wfId;
            wInstance.WorkflowJSON = wf.WorkflowJSON;
            wInstance.Status = Utility.InstanceStatus.Started;

            base._context.WorkflowInstances.Add(wInstance);

            //启动任务
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
                task.InstanceId = wInstance.InstanceId;
                task.Assigner = user;
                base._context.Tasks.Add(task);

                if (node.IsSendEmail)
                {
                    base.SendEmail(task.Assigner, node.EmailTmpl);
                }
            }

            base._context.SaveChanges();

            return "";
        }

        /// <summary>
        /// 审批一个节点并生成下一个节点
        /// </summary>
        /// <param name="wfId"></param>
        /// <param name="nodeId"></param>
        /// <param name="assigner"></param>
        /// <returns></returns>
        public string Approve(string insId, string nodeId, string assigner, int outcome)
        {
            WorkflowInstance wi = base._context.WorkflowInstances.FirstOrDefault(w => w.InstanceId == insId);

            WorkflowModel wfmodel = JsonConvert.DeserializeObject<WorkflowModel>(wi.WorkflowJSON);

            NodeModel node = wfmodel.Nodes.FirstOrDefault(n => n.Id == nodeId);

            //完成当前任务
            DM.Task task = base._context.Tasks.FirstOrDefault(t => t.Assigner == assigner && t.InstanceId == insId && t.NodeId == nodeId);
            task.CompletedDate = DateTime.Now;
            task.Status = Utility.TaskStatus.Completed;
            task.OutCome = outcome;

            //更新上下文或者变量



            //生成新任务
            ButtonModel btnModel = node.Buttons[outcome];


            int nextNodeIndex = btnModel.NextNode;
            NodeModel nextNode = wfmodel.Nodes.FirstOrDefault(n => n.Index == nextNodeIndex);

            if (nextNode != null)
            {
                List<string> users = AssignersActivity.FindUsers(nextNode.Approvers);
                foreach (string user in users)
                {
                    DM.Task newTask = new DM.Task();
                    newTask.Author = "";
                    newTask.Created = DateTime.Now;
                    newTask.ItemId = "";
                    newTask.ListId = "";
                    newTask.NodeId = node.Id;
                    newTask.Status = Utility.TaskStatus.Started;
                    newTask.TaskId = Guid.NewGuid().ToString();
                    newTask.TaskName = node.Name;
                    newTask.TaskUrl = "";
                    newTask.InstanceId = insId;
                    newTask.Assigner = user;

                    base._context.Tasks.Add(newTask);
                }
            }
            else {

                wi.Status = Utility.InstanceStatus.Completed;
            }


            if (node.IsSendEmail)
            {
                base.SendEmail(task.Assigner, node.EmailTmpl);
            }

            base._context.SaveChanges();


            return "";
        }
    }
}
