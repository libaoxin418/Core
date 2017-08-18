using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WFEngine.DataAccess.Models
{
    public class Workflow
    {
        [Key]
        public string WorkflowId { get; set; }
        public int ItemId { get; set; }
        public string ListId { get; set; }
        [MaxLength(500)]
        public string WorkflowName { get; set; }

        [MaxLength(300)]
        public string Author { get; set; }

        public DateTime Created { get; set; }

        public string WorkflowJSON { get; set; }
    }
}
