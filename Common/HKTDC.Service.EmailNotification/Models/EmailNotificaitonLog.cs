using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTDC.Service.EmailNotification.Models
{
    [Table("EmailNotificationLog")]
    public class EmailNotificaitonLog
    {
        [Key]
        public int EmailNotificationLogID { get; set; }
        public int EmailNotificationTasksID { get; set; }
        public Nullable<int> EmailNotificationProfileID { get; set; }
        public int EmailTemplateID { get; set; }
        public string Recipient { get; set; }
        public DateTime LoggedOn { get; set; }
        public string ErrorMessage { get; set; }
    }
}
