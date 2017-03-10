using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("SPAUserRoleProcess")]
    public class SPAUserRoleProcess
    {
        [Key]
        public int SPAUserRoleProcessID { get; set; }
        public string SPAUserRoleGUID { get; set; }
        public int ProcessID { get; set; }
    }
}