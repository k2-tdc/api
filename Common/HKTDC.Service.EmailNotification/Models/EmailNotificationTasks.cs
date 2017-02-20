using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTDC.Service.EmailNotification.Models
{
    [Table("EmailNotificationTasks")]
    public class EmailNotificationTasks
    {
        [Key]
        public int EmailNotificationTasksID { set; get; }
        public int ProcInstID { get; set; }
        public string ProcessName { get; set; }
        public int ActInstID { get; set; }
        public int ActivityGroupID { get; set; }
        public int StepID { get; set; }
        public string ActivityName { get; set; }
        public string ActionOwnerUserID { get; set; }
        public string ActionOwnerEmployeeID { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedOn { get; set; }
        public int FormID { get; set; }
        public string IsEmailed { get; set; }
    }
}
