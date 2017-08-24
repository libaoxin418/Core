using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WFEngine.DataAccess.Models
{
    public class Item
    {
        [MaxLength(50)]
        public string ItemId { get; set; }

        [MaxLength(50)]
        public string FieldName { get; set; }

        [MaxLength(50)]
        public string FieldDispName { get; set; }


        [MaxLength(300)]
        public string FieldValue { get; set; }
    }
}
