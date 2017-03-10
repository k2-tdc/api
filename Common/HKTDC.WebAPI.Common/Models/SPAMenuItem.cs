using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("SPAMenuItem")]
    public class SPAMenuItem
    {
        [Key]
        public string SPAMenuItemGUID { get; set; }
        public string SPAMenuGroupGUID { get; set; }
        public string ItemName { get; set; }
        public int DisplayOrder { get; set; }
        public string IsDefault { get; set; }
        public string Remark { get; set; }
        public string Menulink { get; set; }
    }
}