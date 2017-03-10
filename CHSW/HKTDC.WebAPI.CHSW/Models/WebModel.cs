using aZaaS.Compact.Framework.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    public class WebModel
    {
    }
    public class ProcessMenu
    {
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string ProcessDisplayName { get; set; }
        public string IsDefault { get; set; }
        public string RoleType { get; set; }
        public string UserId { get; set; }
        public string FULL_NAME { get; set; }
    }
    public class Menus
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public List<Submenu> Menu { get; set; }
        public List<ProcessMenu> PList { get; set; }
    }
    public class Submenu
    {
        public string Name { get; set; }
        public string Mlink { get; set; }
        public string Scount { get; set; }
        public List<SSubMenu> sumenu { get; set; }
    }
    public class SSubMenu
    {
        public string Name { get; set; }
        public string Mlink { get; set; }
        public string Scount { get; set; }
    }
    public class ServiceLevel1
    {
        public Guid GUID { get; set; }
        public string Name { get; set; }
        public int ServiceGUID { get; set; }
        public List<ServiceLevel2> Level2 { get; set; }
    }
    public class ServiceLevel2
    {
        public Guid GUID { get; set; }
        public string Name { get; set; }
        public int ServiceGUID { get; set; }
        public string SValue { get; set; }
        public List<ServiceLevel3> Level3 { get; set; }
    }
    public class ServiceLevel3
    {
        public Guid GUID { get; set; }
        public string Name { get; set; }
        public int ServiceGUID { get; set; }
        public string SValue { get; set; }
        public string Approver { get; set; }
        public string ActionTaker { get; set; }
        public int? Enabled { get; set; }
        public int ControlFlag { get; set; }
        public string Placeholder { get; set; }
    }
    public class Employee
    {
        public string USERID { get; set; }
        public string FULLNAME { get; set; }
        public string EMPLOYEEID { get; set; }
    }
    public class Reference
    {
        public string ReferenceID { get; set; }
        public string ReferenceName { get; set; }
    }
    public class ApplicantDetails
    {
        public string Title { get; set; }
        public string Office { get; set; }
        public string Depart { get; set; }
    }
    public class ChkFrmStatus
    {
        public int FormID { get; set; }
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
    public class Review
    {
        public int FormID { get; set; }
        public string ProcInstID { get; set; }
        public string ReferenceID { get; set; }
        public string FormStatus { get; set; }
        public string CreatedOn { get; set; }
        public string PreparerEMP { get; set; }
        public string PreparerUserID { get; set; }
        public string PreparerFNAME { get; set; }
        public string ApplicantEMP { get; set; }
        public string ApplicantUserID { get; set; }
        public string ApplicantFNAME { get; set; }
        public string DEPT { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Justification { get; set; }
        public string EDeliveryDate { get; set; }
        public string DurationOfUse { get; set; }
        public string EstimatedCost { get; set; }
        public string BudgetProvided { get; set; }
        public string BudgetSum { get; set; }
        public string ApproverEmp { get; set; }
        public string ApproverUserID { get; set; }
        public string ApproverFNAME { get; set; }
        public string Remark { get; set; }
        public List<ProcessActionListDTO> actions { get; set; }
        public List<ServiceLevel1> RequestList { get; set; }
        public List<Employee> RequestCC { get; set; }
        public List<ProcessLog> ProcessLog { get; set; }
        public List<RequestFormAttachment> Attachments { get; set;}
        public AttachFile Attachment { get; set; }
        public string ActionTakerUserID { get; set; }
        public string ActionTakerFullName { get; set; }
        //public string ActionTakerStatus { get; set; }
        public string ITSApproverUserID { get; set; }
        public string ITSApproverFullName { get; set; }
        //public string ITSApproverStatus { get; set; }
        //public string Status { get; set; }
        public string DisplayStatus { get; set; }
        public System.Guid? ActionTakerServiceType { get; set; }
    }
    public class AttachFile
    {
        public List<string> FileName { get; set; }
    }
    //***************** Stroed Procedure Return Type  *****************
    public class Applicant
    {
        public string RuleCode { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string WorkerId { get; set; }
        public string WorkerFullName { get; set; }
        public int IsDefault { get; set; }
    }
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
    public partial class RequestReview
    {
        public int FormID { get; set; }
        public string ProcInstID { get; set; }
        public string ReferenceID { get; set; }
        public string FormStatus { get; set; }
        public string EDeliveryDate { get; set; }
        public string DurationOfUse { get; set; }
        public string EstimatedCost { get; set; }
        public string BudgetProvided { get; set; }
        public string BudgetSum { get; set; }
        public string ApproverEMP { get; set; }
        public string ApproverUserID { get; set; }
        public string ApproverFNAME { get; set; }
        public string Remark { get; set; }
        public string CreatedOn { get; set; }
        public string PreparerEMP { get; set; }
        public string PreparerUserID { get; set; }
        public string PreparerFNAME { get; set; }
        public string ApplicantEMP { get; set; }
        public string ApplicantUserID { get; set; }
        public string ApplicantFNAME { get; set; }
        public string DEPT { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Justification { get; set; }
        public int ServiceGUID { get; set; }
        public string ServiceTypeGUID { get; set; }
        public string ServiceTypeValue { get; set; }
        public string SSubMenu { get; set; }
        public string SubMenu { get; set; }
        public string MMenu { get; set; }
        public string DisplayOrder { get; set; }
        public string ApproverRuleCode { get; set; }
        public string ActionTakerRuleCode { get; set; }
        public int Enabled { get; set; }
        public int ControlFlag { get; set; }
        public string MMenuGUID { get; set; }
        public string SubMenuGUID { get; set; }
        public string SSubMenuGUID { get; set; }
        public string ActionTakerUserID { get; set; }
        public string ActionTakerFullName { get; set; }
        //public string ActionTakerStatus { get; set; }
        public string ITSApproverUserID { get; set; }
        public string ITSApproverFullName { get; set; }
       // public string ITSApproverStatus { get; set; }
       // public string Status { get; set; }
        public string DisplayStatus { get; set; }
        public System.Guid? ActionTakerServiceType { get; set; }
        public string Placeholder { get; set; }
    }
    public partial class CC
    {
        public string UID { get; set; }
        public string UName { get; set; }
        public string Type { get; set; }
    }
    public class ExceptionProcessing
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
    }

    public class DelegationDetails
    {
        public string DelegationType { get; set; }
        public string FromUser_USER_ID { get; set; }
        public string FromUser_EMPLOYEE { get; set; }
        public string FromUser_FULL_NAME { get; set; }
        public string ToUser_USER_ID { get; set; }
        public string ToUser_EMPLOYEE { get; set; }
        public string ToUser_FULL_NAME { get; set; }
        public string Enabled { get; set; }
        public string Remark { get; set; }
        public string CreatedBy_USER_ID { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy_USER_ID { get; set; }
        public string ModifiedOn { get; set; }
        public int? DelegationID { get; set; }
        public int? ProcessID { get; set; }
        public int? StepID { get; set; }
        public string ProcessDisplayName { get; set; }
        public string StepDisplayName { get; set; }
    }

    public class ProcessActionListDTO
    {
        public int ActionID { get; set; }
        public string Action { get; set; }
        public string ButtonName { get; set; }
    }

    public class CommonSettingsDTO {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
    }

    public class CheckDelegationUser
    {
        public int FormID { get; set; }
        public string FormStatus { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
    }

    public class ReqeustFormRefId
    {
        public string RefId { get; set; }
    }

    public class VWEmployeeDTO
    {
        public string UserId { get; set; }
        public string UserFullName { get; set; }
    }

    public class CheckUserPermission
    {
        public int Result { get; set; }
    }

    public class Dept
    {
        public string DeptName { get; set; }
        public string DeptCode { get; set; }
    }

    public class ResendDetail
    {
        public bool isSubTask { get; set; }
        public int SubTaskID { get; set; }
        public int ProcInstID { get; set; }
        public string ProcessName { get; set; }
        public int ActInstID { get; set; }
        public string ActivityName { get; set; }
        public string ActionOwnerUserID { get; set; }
        public string ActionOwnerEmployeeID { get; set; }
        public int FormID { get; set; }
    }
}

