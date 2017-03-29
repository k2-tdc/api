using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("DelegationList")]
    public class DelegationList
    {
        [Key]
        public int DelegationID { get; set; }
        public string DelegationType { get; set; }
        public int ProcessID { get; set; }
        public Nullable<int> ActivityGroupID { get; set; }
        public string Permission { get; set; }
        public string FromUser_UserID { get; set; }
        public string FromUser_EmployeeID { get; set; }
        public string FromUser_FullName { get; set; }
        public string ToUser_UserID { get; set; }
        public string ToUser_EmployeeID { get; set; }
        public string ToUser_FullName { get; set; }
        public string ToUser_Dept { get; set; }
        public string Enabled { get; set; }
        public string Remark { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByEmployeeID { get; set; }
        public string CreatedByFullName { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public string ModifiedByUserID { get; set; }
        public string ModifiedByEmployeeID { get; set; }
        public string ModifiedByFullName { get; set; }
    }
}