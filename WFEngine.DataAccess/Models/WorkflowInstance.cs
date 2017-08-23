using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WFEngine.Utility;

namespace WFEngine.DataAccess.Models
{
    public class WorkflowInstance
    {
        [Key]
        [MaxLength(50)]
        public string InstanceId { get; set; }

        [MaxLength(50)]
        public string WorkflowId { get; set; }

        [MaxLength(50)]
        public string ItemId { get; set; }

        [MaxLength(50)]
        public string ListId { get; set; }

        public string WorkflowJSON { get; set; }

        public InstanceStatus Status { get; set; }
    }
}
