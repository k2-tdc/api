using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("ErrorLog")]
    public class ErrorLog
    {
        [Key]
        public int LogID { get; set; }
        public string LogType { get; set; }
        public String LogPriority { get; set; }
        public string LogUserId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string InnerMessage { get; set; }
        public string ErrorSource { get; set; }
        public Nullable<System.DateTime> LogCreatedDateTime { get; set; }
    }
}


