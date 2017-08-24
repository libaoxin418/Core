using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WFEngine.DataAccess.Models
{
    public class List
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public DateTime Created { get; set; }

        public string Author { get; set; }
    }
}
