using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("ProcessWorkerRuleSetting")]
    public class ProcessWorkerRuleSetting
    {
        public int WorkerRuleID { get; set; }
        [Key]
        public int WorkerRuleSettingID { get; set; }
        public int Template { get; set; }
        public int Score { get; set; }
        public string Nature { get; set; }
        public int? isInclude { get; set; }
        public int isDefault { get; set; }
        public string OwnerUserID { get; set; }
        public string OwnerEmployeeID { get; set; }
        public string OwnerFullName { get; set; }
        public string UserID { get; set; }
        public string UserEmployeeID { get; set; }
        public string UserFullName { get; set; }
        public int? UserMinGradeLevel { get; set; }
        public int? UserMaxGradeLevel { get; set; }
        public int? UserGroupID { get; set; }
        public string UserDepartment { get; set; }
        public string UserTeam { get; set; }
        public string UserTeamFilter { get; set; }
        public string WorkerID { get; set; }
        public string WorkerEmployeeID { get; set; }
        public string WorkerFullName { get; set; }
        public int? WorkerMinGradeLevel { get; set; }
        public int? WorkerMaxGradeLevel { get; set; }
        public int? WorkerGroupID { get; set; }
        public int? WorkerOrgChartLevel { get; set; }
        public string OtherCriteria { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string Summary { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByEmployeeID { get; set; }
        public string CreatedByFullName { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedByUserID { get; set; }
        public string ModifiedByEmployeeID { get; set; }
        public string ModifiedByFullName { get; set; }
    }
}