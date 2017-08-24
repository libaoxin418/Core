using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WFEngine.DataAccess.Models
{
    public class Workflow
    {
        [Key]
        [StringLength(50)]
        public string WorkflowId { get; set; }

        [StringLength(500)]
        public string WorkflowName { get; set; }

        [StringLength(300)]
        public string Author { get; set; }

        public DateTime Created { get; set; }

        public string WorkflowJSON { get; set; }
    }
}
