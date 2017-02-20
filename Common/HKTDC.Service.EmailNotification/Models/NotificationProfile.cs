using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTDC.Service.EmailNotification.Models
{
    [Table("EmailNotificationProfile")]
    public class NotificationProfile
    {
        [Key]
        public int EmailNotificationProfileID { get; set; }
        public int ProcessID { get; set; }
        public int ActivityGroupID { get; set; }
        public string UserID { get; set; }
        public string EmployeeID { get; set; }
        public bool WeekDay1 { get; set; }
        public bool WeekDay2 { get; set; }
        public bool WeekDay3 { get; set; }
        public bool WeekDay4 { get; set; }
        public bool WeekDay5 { get; set; }
        public bool WeekDay6 { get; set; }
        public bool WeekDay7 { get; set; }
        public bool TimeSlot1 { get; set; }
        public bool TimeSlot2 { get; set; }
        public bool TimeSlot3 { get; set; }
        public bool TimeSlot4 { get; set; }
        public bool TimeSlot5 { get; set; }
        public bool TimeSlot6 { get; set; }
        public bool TimeSlot7 { get; set; }
        public bool TimeSlot8 { get; set; }
        public bool TimeSlot9 { get; set; }
        public bool TimeSlot10 { get; set; }
        public bool TimeSlot11 { get; set; }
        public bool TimeSlot12 { get; set; }
        public bool TimeSlot13 { get; set; }
        public bool TimeSlot14 { get; set; }
        public bool TimeSlot15 { get; set; }
        public bool TimeSlot16 { get; set; }
        public bool TimeSlot17 { get; set; }
        public bool TimeSlot18 { get; set; }
        public bool TimeSlot19 { get; set; }
        public bool TimeSlot20 { get; set; }
        public bool TimeSlot21 { get; set; }
        public bool TimeSlot22 { get; set; }
        public bool TimeSlot23 { get; set; }
        public bool TimeSlot24 { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
