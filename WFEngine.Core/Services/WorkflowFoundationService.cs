using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WFEngine.DataAccess;
using WFEngine.DataAccess.Models;
using WFEngine.Utility;
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

            return "Associate";
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
                task.ExpireDate = DateTime.Now.AddMinutes(node.Expire.Minutes);

                List<ParameterField> parameters = node.TaskName.GetParameter();
                task.TaskName = this.GetFieldValue(node.TaskName, parameters, wfmodel.DataSource);

                //获取task url路径
                if (string.IsNullOrEmpty(node.TaskUrl))
                {
                    task.TaskUrl = string.Format("/forms/approve.html?taskId={0}&listId={1}&itemId={2}", task.TaskId, task.ListId, task.ItemId);
                }
                else
                {
                    List<ParameterField> taskUrlParameters = node.TaskUrl.GetParameter();
                    string taskUrl = this.GetFieldValue(node.TaskUrl, taskUrlParameters, wfmodel.DataSource);

                    task.TaskUrl = string.Format("{0}&taskId={1}", taskUrl, task.TaskId);
                }


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
            //获取实例
            WorkflowInstance wi = base._context.WorkflowInstances.FirstOrDefault(w => w.InstanceId == insId);

            //通过实例获取工作流数据
            WorkflowModel wfmodel = JsonConvert.DeserializeObject<WorkflowModel>(wi.WorkflowJSON);

            NodeModel node = wfmodel.Nodes.FirstOrDefault(n => n.Id == nodeId);

            //完成当前任务
            DM.Task task = base._context.Tasks.FirstOrDefault(t => t.Assigner == assigner && t.InstanceId == insId && t.NodeId == nodeId);
            task.CompletedDate = DateTime.Now;
            task.Status = Utility.TaskStatus.Completed;
            task.OutCome = outcome;

            //如果不是会签删除其他任务
            if (!node.Countersign.Enable)
            {
                IQueryable<DM.Task> removeTasks = base._context.Tasks.Where(t => t.InstanceId == insId && t.NodeId == nodeId && t.Assigner == assigner);
                base._context.Tasks.RemoveRange(removeTasks);
            }

            //更新上下文或者变量


            int nextNodeIndex = 0;
            //判断会签结果，只能通过或者拒绝，根据会签结果判断下一个工作流走向
            if (node.Countersign.Enable)
            {
                //获取总任务数和完成数
                int allCount = base._context.Tasks.Count(t => t.InstanceId == insId && t.NodeId == nodeId);
                int completedCount = base._context.Tasks.Count(t => t.InstanceId == insId && t.NodeId == nodeId && t.Status == Utility.TaskStatus.Completed);

                if (allCount == completedCount)
                {
                    int approvalCount = base._context.Tasks.Count(t => t.InstanceId == insId && t.NodeId == nodeId && t.OutCome == 1);

                    //计算通过率
                    double rate = approvalCount / allCount;

                    IEnumerable<CountersignRateModel> nextNodes = node.Countersign.NextNodes.OrderByDescending(n => n.Rate);
                    foreach (var nn in nextNodes)
                    {
                        if (rate > nn.Rate)
                        {
                            //获取下一节点Id
                            nextNodeIndex = nn.NodeId;
                            break;
                        }
                    }
                }
            }
            else
            {
                ButtonModel btnModel = node.Buttons[outcome];
                nextNodeIndex = btnModel.NextNode;
            }



            //生成新任务
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

                    //获取参数，并根据参数源获取参数值
                    List<ParameterField> parameters = node.TaskName.GetParameter();
                    task.TaskName = this.GetFieldValue(node.TaskName, parameters, wfmodel.DataSource);

                    newTask.TaskUrl = "";
                    newTask.InstanceId = insId;
                    newTask.Assigner = user;
                    newTask.ExpireDate = DateTime.Now.AddMinutes(node.Expire.Minutes);

                    base._context.Tasks.Add(newTask);
                }
            }
            else
            {

                wi.Status = Utility.InstanceStatus.Completed;
            }


            if (node.IsSendEmail)
            {
                base.SendEmail(task.Assigner, node.EmailTmpl);
            }

            base._context.SaveChanges();


            return "";
        }

        /// <summary>
        /// 动态添加工作流节点，修改工作流实例。
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="nodeId"></param>
        /// <param name="outCome"></param>
        /// <param name="prev"></param>
        /// <returns></returns>
        internal string NodeAdd(string instanceId, string nodeId, int outCome,bool prev)
        {
            //获取实例
            WorkflowInstance wi = base._context.WorkflowInstances.FirstOrDefault(w => w.InstanceId == instanceId);

            //通过实例获取工作流数据
            WorkflowModel wfmodel = JsonConvert.DeserializeObject<WorkflowModel>(wi.WorkflowJSON);

            NodeModel node = wfmodel.Nodes.FirstOrDefault(n => n.Id == nodeId);

            NodeModel modelAdd = new NodeModel();




            return "";
        }

        /// <summary>
        /// 工作流回退
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="nodeid"></param>
        internal string Return(string instanceId, string nodeId,int outCome)
        {
            //获取实例
            WorkflowInstance wi = base._context.WorkflowInstances.FirstOrDefault(w => w.InstanceId == instanceId);

            //通过实例获取工作流数据
            WorkflowModel wfmodel = JsonConvert.DeserializeObject<WorkflowModel>(wi.WorkflowJSON);

            NodeModel node = wfmodel.Nodes.FirstOrDefault(n => n.Id == nodeId);

            ButtonModel btnModel = node.Buttons[outCome];
            int nextNodeIndex = btnModel.NextNode;




            return "";
        }

        /// <summary>
        /// 工作流恢复
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        internal string Resume(string instanceId)
        {
            //获取实例
            WorkflowInstance wi = base._context.WorkflowInstances.FirstOrDefault(w => w.InstanceId == instanceId);

            //更新instance状态
            wi.Status = InstanceStatus.InProgress;

            base._context.SaveChanges();

            return "";
        }

        /// <summary>
        /// 工作流挂起，用于到期自动审批
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        internal string Suspend(string instanceId)
        {
            //获取实例
            WorkflowInstance wi = base._context.WorkflowInstances.FirstOrDefault(w => w.InstanceId == instanceId);

            //更新instance状态
            wi.Status = InstanceStatus.Suspended;

            base._context.SaveChanges();

            return "";
        }

        /// <summary>
        /// 取消工作流，只有没人审批的时候才能取消
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        internal string Cancel(string instanceId)
        {
            //获取实例
            WorkflowInstance wi = base._context.WorkflowInstances.FirstOrDefault(w => w.InstanceId == instanceId);

            IQueryable<DM.Task> tasksCompleted = base._context.Tasks.Where(t => t.Status == Utility.TaskStatus.Completed);

            if (tasksCompleted.Count() > 0)
            {
                //已经有任务开始的不能取消
            }
            else
            {
                //更新task状态
                IQueryable<DM.Task> tasks = base._context.Tasks.AsQueryable<DM.Task>();
                foreach (var task in tasks)
                {
                    task.Status = Utility.TaskStatus.Cancelled;
                }

                //更新instance状态
                wi.Status = InstanceStatus.Cancelled;
            }

            base._context.SaveChanges();

            return "";
        }

        /// <summary>
        /// 强制终止工作流
        /// </summary>
        /// <param name="instanceId"></param>
        internal string Terminate(string instanceId)
        {
            //获取实例,标注为已被终止
            WorkflowInstance wi = base._context.WorkflowInstances.FirstOrDefault(w => w.InstanceId == instanceId);
            wi.Status = InstanceStatus.Terminated;

            //获取所有没有完成任务，标注为任务被取消
            IQueryable<DM.Task> cancelTasks = base._context.Tasks.Where(t => t.InstanceId == instanceId && t.Status == Utility.TaskStatus.Started);

            foreach (var task in cancelTasks)
            {
                task.Status = Utility.TaskStatus.Cancelled;
            }

            base._context.SaveChanges();

            return "";
        }

        /// <summary>
        /// 获取所有的任务列表（代办列表）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<DM.Task> QueryAllTask(string user)
        {
            IQueryable<DM.Task> tasks = base._context.Tasks.Where(t => t.Assigner == user && t.Status == Utility.TaskStatus.Started);
            return tasks;
        }

        /// <summary>
        /// 查询单条任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        internal DM.Task QueryTask(string taskId)
        {
            DM.Task task = base._context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
            return task;
        }

        /// <summary>
        /// 获取工作流审批历史记录
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        internal IEnumerable<DM.Task> QueryAllHistory(string instanceId)
        {
            IQueryable<DM.Task> tasks = base._context.Tasks.Where(t => t.InstanceId == instanceId && t.Status == Utility.TaskStatus.Completed).OrderBy(o => o.CompletedDate);
            return tasks;
        }

        /// <summary>
        /// 获取带参数的字段值
        /// </summary>
        /// <param name="content"></param>
        /// <param name="parameters"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        private string GetFieldValue(string content, List<ParameterField> parameters, Dictionary<string, string> dataSource)
        {
            foreach (var parameter in parameters)
            {
                string source = dataSource.FirstOrDefault(k => k.Key == parameter.ReplaceField).Value;

                string fieldValue = string.Empty;
                switch (parameter.Type)
                {
                    case "API":
                        fieldValue = this.GetFieldValueByApi(source, parameter.Name);
                        break;
                    case "DB":
                        fieldValue = this.GetFieldValueByDb(source, parameter.Name);
                        break;
                }

                content = content.Replace(parameter.ReplaceField, fieldValue);
            }

            return content;
        }

        /// <summary>
        /// 通过业务数据库获取字段数据
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetFieldValueByDb(string dataSource, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 通过api获取字段数据
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetFieldValueByApi(string dataSource, string name)
        {
            HttpClient client = new HttpClient();
            string value = client.GetStringAsync(dataSource + "?Parameter=" + name).Result;
            return value;
        }

    }
}
