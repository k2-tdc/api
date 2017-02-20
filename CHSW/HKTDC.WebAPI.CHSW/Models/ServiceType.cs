using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("ServiceType")]
    public class ServiceType
    {
        [Key]
        public System.Guid ServiceTypeGUID { get; set; }
        public string ParentServiceTypeGUID { get; set; }
        public string ServiceTypeLevel { get; set; }
        public string ServiceTypeName { get; set; }
        public string ID { get; set; }
        public string ApproverRuleCode { get; set; }
        public string ActionTakerRuleCode { get; set; }
        public Nullable<int> Enabled { get; set; }
        public int DisplayOrder { get; set; }
        public int ControlFlag { get; set; }
        public string Placeholder { get; set; }
    }
}