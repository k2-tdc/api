using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("SPAUserRoleMember")]
    public class SPAUserRoleMember
    {
        [Key]
        public string SPAUserRoleMemberGUID { get; set; }
        public string SPAUserRoleGUID { get; set; }
        public string USER_ID { get; set; }
        [Column("EMPLOYEE#")]
        public string EmployeeNo { get; set; }
        public string FULL_NAME { get; set; }
        public string Remark { get; set; }
    }
}