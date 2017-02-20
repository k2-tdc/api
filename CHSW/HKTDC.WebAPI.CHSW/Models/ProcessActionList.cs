using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("ProcessActionList")]
    public class ProcessActionList
    {
        [Key]
        public int ActionID { get; set; }
        public int StepID { get; set; }
        public string K2ActionName { get; set; }
        public string ActionButtonName { get; set; }
        public string ActionDisplayName { get; set; }
    }
}