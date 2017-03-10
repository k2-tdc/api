using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("SPAUserRole")]
    public class SPAUserRole
    {
        [Key]
        public string SPAUserRoleGUID { get; set; }
        public string RoleName { get; set; }
        public string Desc { get; set; }
        public string Remark { get; set; }
        public int ProcessID { get; set; }
    }
}