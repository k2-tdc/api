using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("RequestFormTaskActioner")]
    public class RequestFormTaskActioner
    {
        [Key]
        public int RequestFormTaskActionerID { get; set; }
        public int FormID { get; set; }
        public string ActionTakerUserID { get; set; }
        public string ActionTakerEmployeeID { get; set; }
        public string ActionTakerFullName { get; set; }
        public string ActionTakerDeptName { get; set; }
        public string ITSApproverUserID { get; set; }
        public string ITSApproverEmployeeID { get; set; }
        public string ITSApproverFullName { get; set; }
        public string ITSApproverDeptName { get; set; }
        public string Status { get; set; }
        public string ActionTakerStatus { get; set; }
        public string ITSApproverStatus { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public int ProcInstID { get; set; }
        public System.Guid ServiceTypeGUID { get; set; }
    }
}