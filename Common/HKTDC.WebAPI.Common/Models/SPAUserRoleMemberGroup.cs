using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("SPAUserRoleMemberGroup")]
    public class SPAUserRoleMemberGroup
    {
        public string SPAUserRoleGUID { get; set; }
        [Key]
        public string SPAUserRoleMemberGroupGUID { get; set; }
        public string GroupCode { get; set; }
        public string GroupType { get; set; }
        public int Deleted { get; set; }
        public Nullable<DateTime> ExpiryDate { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public string ModifiedByUserID { get; set; }
        public string ModifiedByEmployeeID { get; set; }
        public string ModifiedByFullName { get; set; }
    }
}