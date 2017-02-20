using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("ProcessStepList")]
    public class ProcessStepList
    {
        [Key]
        public int StepID { get; set; }
        public string StepName { get; set; }
        public string StepDisplayName { get; set; }
        public string K2StepName { get; set; }
        public string K2StepFQN { get; set; }
        public string Desc { get; set; }
        public int ProcessID { get; set; }
        public string WorkList { get; set; }
    }
}