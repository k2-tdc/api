using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using HKTDC.WebAPI.CHSW.Models;

namespace HKTDC.WebAPI.CHSW.DBContext
{
    public class EntityContext : DbContext
    {
             
        public virtual DbSet<DelegationList> DelegationLists { get; set; }
        public virtual DbSet<ProcessList> ProcessLists { get; set; }
        public virtual DbSet<ProcessLog> ProcessLogs { get; set; }
        public virtual DbSet<ProcessStepList> ProcessStepLists { get; set; }
        public virtual DbSet<ProcessActionList> ProcessActionLists { get; set; }
        public virtual DbSet<RequestFormAttachment> RequestFormAttachments { get; set; }
        public virtual DbSet<RequestFormMaster> RequestFormMasters { get; set; }
        public virtual DbSet<RequestFormServiceType> RequestFormServiceTypes { get; set; }
        public virtual DbSet<RequestFormTaskActioner> RequestFormTaskActioners { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        //public virtual DbSet<SPALoginLog> SPALoginLogs { get; set; }
        //public virtual DbSet<SPAMenuGroup> SPAMenuGroups { get; set; }
        //public virtual DbSet<SPAMenuItem> SPAMenuItems { get; set; }
        //public virtual DbSet<SPAMenuMaster> SPAMenuMasters { get; set; }
        //public virtual DbSet<SPAUserRole> SPAUserRoles { get; set; }
        //public virtual DbSet<SPAUserRoleEntity> SPAUserRoleEntities { get; set; }
        //public virtual DbSet<SPAUserRoleMember> SPAUserRoleMembers { get; set; }
        //public virtual DbSet<WorkerRole> WorkerRoles { get; set; }
        //public virtual DbSet<WorkerRuleMaster> WorkerRuleMasters { get; set; }
        //public virtual DbSet<WorkerRuleSetting> WorkerRuleSettings { get; set; }
        //public virtual DbSet<WorkerRuleSettingDetail> WorkerRuleSettingDetails { get; set; }
        //public virtual DbSet<WorkerRuleSettingReference> WorkerRuleSettingReferences { get; set; }
        //public virtual DbSet<WorkerRuleType> WorkerRuleTypes { get; set; }
        public virtual DbSet<RequestFormCC> RequestFormCCs { get; set; }
        public virtual DbSet<VW_EMPLOYEE> VW_EMPLOYEE { get; set; }
        public virtual DbSet<CommonSettings> CommonSettings { get; set; }
        public virtual DbSet<VDepartment> vDepartment { get; set; }
        public virtual DbSet<MenuItem> MenuItem { get; set; }
    }
    
}