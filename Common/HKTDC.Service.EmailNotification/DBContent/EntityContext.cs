using HKTDC.Service.EmailNotification.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTDC.Service.EmailNotification.DBContent
{
    public class EntityContext : DbContext
    {
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
        public virtual DbSet<NotificationProfile> NotificationProfile { get; set; }
        public virtual DbSet<ProcessStepList> ProcessStepList { get; set; }
        public virtual DbSet<ProcessList> ProcessList { get; set; }
        public virtual DbSet<vUser> vUser { get; set; }
        public virtual DbSet<ProcessActivityGroup> ProcessActivityGroup { get; set; }
        public virtual DbSet<ProcessActivityGroupStep> ProcessActivityGroupStep { get; set; }
        public virtual DbSet<AppSetting> AppSetting { get; set; }
        public virtual DbSet<EmailNotificaitonLog> EmailNotificationLog { get; set; }
        public virtual DbSet<EmailNotificationTasks> EmailNotificationTasks { get; set; }
    }
}
