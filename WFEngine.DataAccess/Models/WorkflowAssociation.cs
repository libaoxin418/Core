using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WFEngine.DataAccess.Models
{
    public class WorkflowAssociation
    {
        [MaxLength(50)]
        public string WorkflowId { get; set; }

        [MaxLength(50)]
        public string ListId { get; set; }
    }
}
