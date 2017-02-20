using HKTDC.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.DBContext
{
    public class EntityContext : DbContext
    {
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
        public virtual DbSet<NotificationProfile> NotificationProfile { get; set; }
        public virtual DbSet<ProcessStepList> ProcessStepList { get; set; }
        public virtual DbSet<ProcessList> ProcessList { get; set; }
        public virtual DbSet<vUser> vUser { get; set; }
        public virtual DbSet<ProcessActivityGroup> ProcessActivityGroup { get; set; }
        public virtual DbSet<ProcessActivityGroupStep> ProcessActivityGroupStep { get; set; }
        public virtual DbSet<ProcessRequestFormAttachment> ProcessRequestFormAttachment { get; set; }
    }
}