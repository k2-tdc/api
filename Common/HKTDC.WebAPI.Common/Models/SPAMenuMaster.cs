using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("SPAMenuMaster")]
    public class SPAMenuMaster
    {
        public string Name { get; set; }
        public string Remark { get; set; }
        public string IsDefault { get; set; }
        public int ProcessID { get; set; }
        [Key]
        public string SPAMenuMasterGUID { get; set; }
    }
}