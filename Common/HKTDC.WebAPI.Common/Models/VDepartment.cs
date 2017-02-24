using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("vDepartment")]
    public class VDepartment
    {
        [Key]
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
    }
}