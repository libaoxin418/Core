using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WFEngine.Utility;

namespace WFEngine.DataAccess.Models
{
    public class Task
    {
        [MaxLength(50)]
        public string TaskId { get; set; }

        [MaxLength(50)]
        public string InstanceId { get; set; }

        [MaxLength(300)]
        public string TaskName { get; set; }

        [MaxLength(50)]
        public string Author { get; set; }

        public DateTime Created { get; set; }

        public DateTime CompletedDate { get; set; }

        public TaskStatus Status { get; set; }

        [MaxLength(300)]
        public string TaskUrl { get; set; }

        [MaxLength(50)]
        public string NodeId { get; set; }

        [MaxLength(50)]
        public string ItemId { get; set; }

        [MaxLength(50)]
        public string ListId { get; set; }

        [MaxLength(50)]
        public string Assigner { get; set; }

        public int OutCome { get; set; }
    }
}
