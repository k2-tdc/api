using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTDC.Service.EmailNotification.Models
{
    [Table("ProcessActivityGroup")]
    public class ProcessActivityGroup
    {
        public int ProcessID { get; set; }
        [Key]
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupType { get; set; }
    }
}
