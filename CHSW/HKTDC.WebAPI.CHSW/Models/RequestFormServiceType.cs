using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("RequestFormServiceType")]
    public class RequestFormServiceType
    {
        [Key]
        public System.Guid ServiceGUID { get; set; }
        public int FormGUID { get; set; }
        public string ServiceTypeGUID { get; set; }
        public string IsChecked { get; set; }
        public string ServiceTypeValue { get; set; }
        public string Remark { get; set; }
    }
}