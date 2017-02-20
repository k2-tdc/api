using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("ProcessLog")]
    public class ProcessLog
    {
        public int ProcInstID { get; set; }
        public string ProcessName { get; set; }
        public int ActInstID { get; set; }
        public string ActivityName { get; set; }
        public string ActionResult { get; set; }
        public string ActionOwnerUserID { get; set; }
        public string ActionOwnerEmployeeID { get; set; }
        public string ActionOwnerFullName { get; set; }
        public string ActionTakerUserID { get; set; }
        public string ActionTakerEmployeeID { get; set; }
        public string ActionTakerFullName { get; set; }
        public string ActionTakerComment { get; set; }
        public Nullable<System.DateTime> CommentedOn { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string Remark { get; set; }
        [Key]
        public int ProcessLogID { get; set; }
        public Nullable<int> FormID { get; set; }
        public string ActionOwnerDeptName { get; set; }
        public string ActionTakerDeptName { get; set; }
    }
}