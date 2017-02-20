using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTDC.Service.EmailNotification.Models
{
    [Table("ProcessList")]
    public class ProcessList
    {
        [Key]
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string ProcessDisplayName { get; set; }
        public string K2ProcessName { get; set; }
        public string K2ProcessFQN { get; set; }
        public string Desc { get; set; }
        public Nullable<int> Flag { get; set; }
    }
}
