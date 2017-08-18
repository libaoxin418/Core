using System;
using System.Collections.Generic;
using System.Text;
using WFEngine.Utility;

namespace WFEngine.DataAccess.Models
{
    public class Task
    {
        public string TaskId { get; set; }

        public string TaskName { get; set; }

        public string Author { get; set; }

        public DateTime Created { get; set; }

        public TaskStatus Status { get; set; }

        public string TaskUrl { get; set; }

        public string WorkflowId { get; set; }

        public string NodeId { get; set; }

        public int ItemId { get; set; }

        public string ListId { get; set; }
    }
}
