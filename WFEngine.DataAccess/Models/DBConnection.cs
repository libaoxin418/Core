using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WFEngine.DataAccess.Models
{
    public class DBConnection
    {
        [Key]
        [MaxLength(50)]
        public string ConnectionId { get; set; }

        [MaxLength(255)]
        public string AppId { get; set; }

        [MaxLength(255)]
        public string ConnectionString { get; set; }
    }
}
