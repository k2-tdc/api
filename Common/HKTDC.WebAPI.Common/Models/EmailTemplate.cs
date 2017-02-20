using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("EmailTemplate")]
    public class EmailTemplate
    {
        [Key]
        public int EmailTemplateID { get; set; }
        public int ProcessID { get; set; }
        public int ActivityGroupID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool Enabled { get; set; }
    }
}