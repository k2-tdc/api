using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    public class ApproverHistory
    {
        public int FormID { get; set; }
        public int ProcessLogID { get; set; }
        public int ProcInstID { get; set; }
        public DateTime CommentedOn { get; set; }
        public string ReferenceID { get; set; }
        public string FormStatus { get; set; }
        public string ApplicantFullName { get; set; }
        public string ApplicantEmployeeID { get; set; }
        public string ApproverFullName { get; set; }
        public string ApproverEmployeeID { get; set; }
        public string ActionTakerFullName { get; set; }
        public string ActionTakerEmployeeID { get; set; }
        public string DisplayStatus { get; set; }
        public string ITSApproverFullName { get; set; }
        public string ITSApproverEmployeeID { get; set; }
        public string LastUser { get; set; }
        public int ServiceGUID { get; set; }
        public string MMenu { get; set; }
        public string SubMenu { get; set; }
        public string SSubMenu { get; set; }
        public Guid MMenuGUID { get; set; }
        public Guid SubMenuGUID { get; set; }
        public Guid SSubMenuGUID { get; set; }
        public string ServiceTypeValue { get; set; }
        public int ControlFlag { get; set; }
        public string ApplicantUserId { get; set; }
        public string PreparerFullName { get; set; }
    }
}