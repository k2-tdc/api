using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    public class ProcessList
    {
        public string ProcessName { get; set; }
        public string ProcessDisplayName { get; set; }
        public string K2ProcessName { get; set; }
        public string K2ProcessFQN { get; set; }
        public string Desc { get; set; }
        [Key]
        public int ProcessID { get; set; }
    }
}