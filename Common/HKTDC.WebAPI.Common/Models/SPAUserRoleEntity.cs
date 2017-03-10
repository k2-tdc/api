using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("SPAUserRoleEntity")]
    public class SPAUserRoleEntity
    {
        [Key]
        public string SPAUserRoleEntityGUID { get; set; }
        public string SPAUserRoleGUID { get; set; }
        public string SPAMenuItemGUID { get; set; }
        public string Remark { get; set; }
    }
}