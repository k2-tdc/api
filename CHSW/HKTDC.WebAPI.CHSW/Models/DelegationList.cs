using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("DelegationList")]
    public class DelegationList
    {
        public string DelegationType { get; set; }
        public string FromUser_USER_ID { get; set; }
        [Column("FromUser_EMPLOYEE#")]
        public string FromUser_EMPLOYEE { get; set; }
        public string FromUser_FULL_NAME { get; set; }
        public string ToUser_USER_ID { get; set; }
        [Column("ToUser_EMPLOYEE#")]
        public string ToUser_EMPLOYEE { get; set; }
        public string ToUser_FULL_NAME { get; set; }
        public string Enabled { get; set; }
        public string Remark { get; set; }
        public string CreatedBy_USER_ID { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy_USER_ID { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        [Key]
        public int DelegationID { get; set; }
        public Nullable<int> ProcessID { get; set; }
        public Nullable<int> StepID { get; set; }
    }
}