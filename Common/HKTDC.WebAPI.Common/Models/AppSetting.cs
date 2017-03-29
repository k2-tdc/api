using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("AppSetting")]
    public class AppSetting
    {
        [Key]
        public int SettingID { get; set; }
        public string Category { get; set; }
        public string AppKey { get; set; }
        public string AppValue { get; set; }
        public string Remark { get; set; }
    }
}