using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WFEngine.Utility.Models
{
    public class WorkflowModel
    {
        [Key]
        public string WorkflowId { get; set; }

        [MaxLength(500)]
        public string WorkflowName { get; set; }

        [MaxLength(300)]
        public string Author { get; set; }

        public DateTime Created { get; set; }

        /// <summary>
        /// 工作流上下文变量
        /// </summary>
        public Dictionary<string, object> WorkflowVariable { get; set; }

        public List<NodeModel> Nodes { get; set; }

        /// <summary>
        /// 当前节点
        /// </summary>
        public int CurrentNode { get; set; }

        public int ItemId { get; set; }
        public string ListId { get; set; }
    }
}
