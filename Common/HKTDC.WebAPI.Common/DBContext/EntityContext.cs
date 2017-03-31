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
        public virtual DbSet<VDepartment> vDepartment { get; set; }
        public virtual DbSet<SPAUserRole> SPAUserRole { get; set; }
        public virtual DbSet<SPAUserRoleEntity> SPAUserRoleEntity { get; set; }
        public virtual DbSet<SPAUserRoleMember> SPAUserRoleMember { get; set; }
        public virtual DbSet<SPAUserRoleMemberGroup> SPAUserRoleMemberGroup { get; set; }
        public virtual DbSet<SPAUserRoleProcess> SPAUserRoleProcess { get; set; }
        public virtual DbSet<SPAMenuGroup> SPAMenuGroup { get; set; }
        public virtual DbSet<SPAMenuItem> SPAMenuItem { get; set; }
        public virtual DbSet<SPAMenuMaster> SPAMenuMaster { get; set; }
        public virtual DbSet<ProcessWorkerRule> ProcessWorkerRule { get; set; }
        public virtual DbSet<ProcessWorkerRuleSetting> ProcessWorkerRuleSetting { get; set; }
        public virtual DbSet<DelegationList> DelegationList { get; set; }
        public virtual DbSet<AppSetting> AppSetting { get; set; }
        public virtual DbSet<SharingList> SharingList { get; set; }
    }
}