using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("RequestFormMaster")]
    public class RequestFormMaster
    {
        public string ProcInstID { get; set; }
        public string ReferenceID { get; set; }
        public string FormStatus { get; set; }
        public string PreparerUserID { get; set; }
        public string PreparerEmployeeID { get; set; }
        public string PreparerFullName { get; set; }
        public string ApplicantUserID { get; set; }
        public string ApplicantEmployeeID { get; set; }
        public string ApplicantFullName { get; set; }
        public string ApproverUserID { get; set; }
        public string ApproverEmployeeID { get; set; }
        public string ApproverFullName { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> SubmittedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string Justification { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public string DurationOfUse { get; set; }
        public Nullable<decimal> EstimatedCost { get; set; }
        public string BudgetProvided { get; set; }
        public string BudgetSum { get; set; }
        public string SubmittedTo { get; set; }
        public string Remark { get; set; }
        [Key]
        public int FormID { get; set; }
        public string PreparerDeptName { get; set; }
        public string ApplicantDeptName { get; set; }
        public string ApproverDeptName { get; set; }
    }
}