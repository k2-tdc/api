using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTDC.Service.EmailNotification.Models
{
    [Table("ProcessActivityGroupStep")]
    public class ProcessActivityGroupStep
    {
        [Key]
        public int ActivityGroupStepID { get; set; }
        public int ProcessID { get; set; }
        public int ActivityGroupID { get; set; }
        public int StepID { get; set; }
    }
}
