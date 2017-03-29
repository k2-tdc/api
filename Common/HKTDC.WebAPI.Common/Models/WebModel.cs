using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    public class WebModel
    {
    }
    public class ExceptionProcessing
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
    }
    public class Menus
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public List<Submenu> Menu { get; set; }
        public bool IsAdmin { get; set; }
        public string RoleType { get; set; }
    }
    public class Submenu
    {
        public string Name { get; set; }
        public string Mlink { get; set; }
        public string Scount { get; set; }
        public List<SSubMenu> sumenu { get; set; }
        public string MenuId { get; set; }
    }
    public class SSubMenu
    {
        public string Name { get; set; }
        public string Mlink { get; set; }
        public string Scount { get; set; }
        public string MenuId { get; set; }
    }

    //***************** Stroed Procedure Return Type  *****************
    public class MenuList
    {
        public string MMenu { get; set; }
        public string SubMenu { get; set; }
        public string SSubMenu { get; set; }
        public string DOrder { get; set; }
        public string UserName { get; set; }
        public string User_ID { get; set; }
        public string EMPLOYEENO { get; set; }
        public string Menulink { get; set; }
        public string Scount { get; set; }
        public string RoleName { get; set; }
        public string MenuId { get; set; }
    }
    public class ProcessListDTO
    {
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string ProcessDisplayName { get; set; }
        public Nullable<int> Flag { get; set; }
    }
    public class ProcessStepListDTO
    {
        public int StepID { get; set; }
        public int ProcessID { get; set; }
        public string StepName { get; set; }
        public string StepDisplayName { get; set; }
    }
    public class EmailTemplateDTO
    {
        public int EmailTemplateID { get; set; }
        public int ProcessId { get; set; }
        public int ActivityGroupId { get; set; }
        public string ProcessName { get; set; }
        public string pName { get; set; }
        public string StepName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool Enabled { get; set; }
    }
    public class NotificationProfileDTO
    {
        public int EmailNotificationProfileID { get; set; }
        public string ProcessName { get; set; }
        public string StepName { get; set; }
        public string UserID { get; set; }
        public string UserFullName { get; set; }
        public string EmployeeID { get; set; }
        public bool WeekDay1 { get; set; }
        public bool WeekDay2 { get; set; }
        public bool WeekDay3 { get; set; }
        public bool WeekDay4 { get; set; }
        public bool WeekDay5 { get; set; }
        public bool WeekDay6 { get; set; }
        public bool WeekDay7 { get; set; }
        public int TimeSlot { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string Remark { get; set; }
    }
    public class NotificationProfileDetailDTO
    {
        public int EmailNotificationProfileID { get; set; }
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public int StepID { get; set; }
        public string UserID { get; set; }
        public string EmployeeID { get; set; }
        public bool WeekDay1 { get; set; }
        public bool WeekDay2 { get; set; }
        public bool WeekDay3 { get; set; }
        public bool WeekDay4 { get; set; }
        public bool WeekDay5 { get; set; }
        public bool WeekDay6 { get; set; }
        public bool WeekDay7 { get; set; }
        public int TimeSlot { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string Remark { get; set; }
    }
    public class UserDTO
    {
        public string UserID { get; set; }
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
    }

    public class UserRole
    {
        public string RoleName { get; set; }
    }

    public class Applicant
    {
        public string RuleCode { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string WorkerId { get; set; }
        public string WorkerFullName { get; set; }
        public int IsDefault { get; set; }
    }
    public class ApplicantDetails
    {
        public string Title { get; set; }
        public string Office { get; set; }
        public string Depart { get; set; }
    }
    public class Reference
    {
        public string ReferenceID { get; set; }
        public string ReferenceName { get; set; }
    }
    public class CommonSettingsDTO
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
    }
    public class ChkFrmStatus
    {
        public int FormID { get; set; }
        public int ProcessId { get; set; }
        public string ProcInstID { get; set; }
        public string ReferenceID { get; set; }
        public string FormStatus { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string ApplicantUserId { get; set; }
        public string ApplicantEMP { get; set; }
        public string ApplicantFNAME { get; set; }
        public string ApproverEmp { get; set; }
        public string ApproverFNAME { get; set; }
        public List<string> actions { get; set; }
        public string ProcessUrl { get; set; }
        public string SN { get; set; }
        public string ActionTakerFullName { get; set; }
        //public string ActionTakerStatus { get; set; }      
        public string ITSApproverFullName { get; set; }
        //public string ITSApproverStatus { get; set; }
        public string DisplayStatus { get; set; }
        public string LastUser { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }

        public List<ServiceLevel1> RequestList { get; set; }
    }
    public class ServiceLevel1
    {
        public Guid GUID { get; set; }
        public string Name { get; set; }
        public List<ServiceLevel2> Level2 { get; set; }
    }
    public class ServiceLevel2
    {
        public Guid GUID { get; set; }
        public string Name { get; set; }
        public string SValue { get; set; }
        public List<ServiceLevel3> Level3 { get; set; }
    }
    public class ServiceLevel3
    {
        public Guid GUID { get; set; }
        public string Name { get; set; }
        public string SValue { get; set; }
        public string Approver { get; set; }
        public string ActionTaker { get; set; }
        public int? Enabled { get; set; }
        public int ControlFlag { get; set; }
        public string Placeholder { get; set; }
    }
    public class CheckStatus
    {
        public int FormID { get; set; }
        public string ProcInstID { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string ReferenceID { get; set; }
        public string FormStatus { get; set; }
        public string ApplicantFNAME { get; set; }
        public string ApproverEMP { get; set; }
        public string ApproverFNAME { get; set; }
        public string MMenu { get; set; }
        public string SubMenu { get; set; }
        public string ServiceTypeValue { get; set; }
        public string DisplayOrder { get; set; }
        public string ApplicantEMP { get; set; }
        public string ServiceTypeGUID { get; set; }
        public string SSubMenu { get; set; }
        public int ServiceGUID { get; set; }
        public string ApplicantUserID { get; set; }
        public string MMenuGUID { get; set; }
        public string SubMenuGUID { get; set; }
        public string SSubMenuGUID { get; set; }
        public string ActionTakerFullName { get; set; }
        public string ActionTakerStatus { get; set; }
        public string ITSApproverFullName { get; set; }
        public string ITSApproverStatus { get; set; }
        public string DisplayStatus { get; set; }
        public string LastUser { get; set; }
        public int ControlFlag { get; set; }
    }
    public class EmailTemplateDetailDTO
    {
        public int EmailTemplateID { get; set; }
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public int ActivityGroupID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool Enabled { get; set; }
    }
    public class Dept
    {
        public string DeptName { get; set; }
        public string DeptCode { get; set; }
    }
    public class UserRoleDTO
    {
        public string UserRoleGUID { get; set; }
        public string Role { get; set; }
        public string Desc { get; set; }
    }
    public class UserRoleDetailDTO
    {
        public string UserRoleGUID { get; set; }
        public string Role { get; set; }
        public string Desc { get; set; }
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public List<UserRoleMemberDTO> Member { get; set; }
    }
    public class UserRoleMemberDTO
    {
        public string UserRoleMemberGUID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> ExpiryDate { get; set; }
    }
    public class UserRoleMemberDetailDTO
    {
        public string UserRoleGUID { get; set; }
        public string UserRoleMemberGUID { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }
        public string TypeVal { get; set; }
        public Nullable<DateTime> ExpiryDate { get; set; }
    }
    public class UserPermissionSP
    {
        public string RolePermissionGUID { get; set; }
        public string Process { get; set; }
        public string Permission { get; set; }
        public string Role { get; set; }
    }
    public class UserPermissionDTO
    {
        public string[] RolePermissionGUID { get; set; }
        public string Process { get; set; }
        public string Permission { get; set; }
        public string Role { get; set; }
    }
    public class UserPermissionMenuItemDTO
    {
        public string MenuItemGUID { get; set; }
        public string MenuItemName { get; set; }
    }
    public class UserPermissionDetailDTO
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string MenuItemGUID { get; set; }
        public List<UserPermissionDetailUserRoleDTO> Role { get; set; }
    }
    public class UserPermissionDetailUserRoleDTO
    {
        public string RolePermissionGUID { get; set; }
        public string UserRoleGUID { get; set; }
        public string Role { get; set; }
    }
    public class WorkerRuleDTO
    {
        public int WorkerRuleId { get; set; }
        public string Code { get; set; }
        public string Worker { get; set; }
        public string Summary { get; set; }
        public int Score { get; set; }
        public string ProcessName { get; set; }
    }
    public class WorkerRuleDetailDTO
    {
        public int WorkerRuleId { get; set; }
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string ProcessDisplayName { get; set; }
        public string Code { get; set; }
        public string Worker { get; set; }
        public string WorkerType { get; set; }
        public string Summary { get; set; }
        public string Remark { get; set; }
        public int Score { get; set; }
        public List<WorkerRuleRuleListDTO> Rules { get; set; }
    }
    public class WorkerRuleRuleListDTO
    {
        public int WorkerRuleSettingId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set;}
        public string Summary { get; set; }
        public int Score { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
    }
    public class WorkerRuleRuleDTO
    {
        public int TemplateID { get; set; }
        public string Description { get; set; }
        public int DefaultScore { get; set; }
    }
    public class WorkerRuleNatureDTO
    {
        public int NatureID { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public int Enabled { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
    public class WorkerRuleGradingDTO
    {
        public int Grade { get; set; }
        public string Description { get; set; }
    }
    public class WorkerRulePriorityDTO
    {
        public string PriorityID { get; set; }
        public string Description { get; set; }
    }
    public class WorkerRuleDepartmentDTO
    {
        public string DepartmentCode { get; set; }
        public string Department { get; set; }
    }
    public class WorkerRuleCriteriaDTO
    {
        public string CriteriaGroup { get; set; }
        public int CriteriaID { get; set; }
        public string Criteria { get; set; }
    }
    public class WorkerRuleOrgChartDTO
    {
        public int LevelID { get; set; }
        public int LevelNo { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public int Enabled { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
    public class WorkerRuleUserGroupDTO
    {
        public int GroupID { get; set; }
        public string Description { get; set; }
        public int Enabled { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
    public class WorkerRuleTeamDTO
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class WorkerRuleTeamFilterDTO
    {
        public int FilterID { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public int Enabled { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
    public class WorkerRuleRuleDetailDTO
    {
        public int WorkerRuleId { get; set; }
        public int WorkerSettingId { get; set; }
        public int Rule { get; set; }
        public int Nature { get; set; }
        public int Score { get; set; }
        public string UserId { get; set; }
        public string UserId1 { get; set; }
        public string UserId2 { get; set; }
        public int? LevelNo { get; set; }
        public int? GroupID { get; set; }
        public int? GroupID1 { get; set; }
        public int? Grade1 { get; set; }
        public int? Grade2 { get; set; }
        public string Team { get; set; }
        public int? TeamFilter { get; set; }
        public int Priority { get; set; }
        public int? Grade3 { get; set; }
        public int? Grade4 { get; set; }
        public string Department { get; set; }
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }
        public string OtherCriteria { get; set; }
        public int[] Criteria { get; set; }
        public string Remark { get; set; }
        public List<FormAttachment> Reference { get; set; }
    }
    public class FormAttachment
    {
        public System.Guid AttachmentGUID { get; set; }
        public string FileName { get; set; }
        public int? FormID { get; set; }
        public string UploadedByDeptName { get; set; }
        public string UploadedByEmployeeID { get; set; }
        public string UploadedByFullName { get; set; }
        public string UploadedByUserID { get; set; }
        public DateTime? UploadedDate { get; set; }
    }
    public class DelegationDTO
    {
        public int DelegationID { get; set; }
        public string User { get; set; }
        public string Workflow { get; set; }
        public string Task { get; set; }
        public string Department { get; set; }
        public string DelegateTo { get; set; }
        public string Action { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class DelegationActionDTO
    {
        public string Value { get; set; }
        public string Key { get; set; }
    }
    public class DelegationDetailDTO
    {
        public int DelegationID { get; set; }
        public string UserID { get; set; }
        public int ProcessID { get; set; }
        public int? TaskID { get; set; }
        public string Dept { get; set; }
        public string DelegateUserID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Action { get; set; }
        public string Remark { get; set; }
        public string Permission { get; set; }
        public string ProcessName { get; set; }
    }
}