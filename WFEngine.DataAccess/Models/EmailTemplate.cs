using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WFEngine.DataAccess.Models
{
    public class EmailTemplate
    {
        [MaxLength(50)]
        public string TmplId { get; set; }

        [MaxLength(50)]
        public string WorkflowId { get; set; }

        [MaxLength(50)]
        public string NodeId { get; set; }


        public DateTime Created { get; set; }

        [MaxLength(50)]
        public string Author { get; set; }

        public string Contents { get; set; }
    }
}
