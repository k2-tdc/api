using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTDC.Service.EmailNotification.Models
{
    class WebModel
    {
    }

    class NotificationList
    {
        public int EmailNotificationTasksID { get; set; }
        public int ProcIntID { get; set; }
        public int ActIntID { get; set; }
        public string ActionOwnerUserID { get; set; }
        public string ActionOwnerEmployeeID { get; set; }
        public int FormID { get; set; }
        public int ProcessID { get; set; }
        public int StepID { get; set; }
        public int StepOrGroupID { get; set; }
        public Nullable<int> ProfileID { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public int TemplateID { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }

    class MailInfo
    {
        public string TaskID { get; set; }
        public string RefID { get; set; }
        public string Content { get; set; }
    }
}
