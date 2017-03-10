using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("SPAMenuGroup")]
    public class SPAMenuGroup
    {
        [Key]
        public string SPAMenuGroupGUID { get; set; }
        public string ParentMenuGroupGUID { get; set; }
        public string SPAMenuMasterGUID { get; set; }
        public string GroupName { get; set; }
        public int DisplayOrder { get; set; }
        public string Desc { get; set; }
    }
}