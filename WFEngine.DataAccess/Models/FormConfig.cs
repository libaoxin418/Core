using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WFEngine.DataAccess.Models
{
    public class FormConfig
    {
        [Key]
        [MaxLength(50)]
        public string FormId { get; set; }

        [MaxLength(200)]
        public string AppId { get; set; }


    }
}
