using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("ProcessWorkerRule")]
    public class ProcessWorkerRule
    {
        [Key]
        public int WorkerRuleID { get; set; }
        public int ProcessID { get; set; }
        public string RuleCode { get; set; }
        public string RuleName { get; set; }
        public string WorkerType { get; set; }
        public int Score { get; set; }
        public string Summary { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}