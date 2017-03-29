using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("CHSW_DelegationProcess")]
    public class DelegationProcess
    {
        public int ProcessID { get; set; }
        [Key]
        public int GroupID { get; set; }
        public string GroupDisplayName { get; set; }
        public string GroupName { get; set; }
        public string StepName { get; set; }
        public string StepDisplayName { get; set; }
        public string K2StepName { get; set; }
    }
}