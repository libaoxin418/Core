using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WFEngine.DataAccess.Models
{
    public class NodeRegister
    {
        [Key]
        [MaxLength(50)]
        public string RegisterId { get; set; }

        [MaxLength(50)]
        public string WorkflowId { get; set; }

        public string RegisterContents { get; set; }
    }
}
