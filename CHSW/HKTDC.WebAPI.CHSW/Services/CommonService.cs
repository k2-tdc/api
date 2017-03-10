using HKTDC.WebAPI.CHSW.DBContext;
using HKTDC.WebAPI.CHSW.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Services
{
    public class CommonService : BaseService
    {
        private EntityContext Db = new EntityContext();

        /// <summary>
        /// Function used for check status & DraftList Based on the search condition
        /// </summary>
        /// <param name="ReferID">ReferenceID (Optional)</param>
        /// <param name="CStat">Status (Optional)</param>
        /// <param name="FDate">FromDate (Optional)</param>
        /// <param name="TDate">ToDate (Optional)</param>
        /// <param name="Appl">Applicant Name (Optional)</param>
        /// <param name="UserId">UserId for checking authentication</param>
        /// <param name="Type">Review or DraftEdit</param>
        ///  SP  : K2_CheckStatus  
        /// <returns></returns> 
        /// <summary>        
        public List<ChkFrmStatus> GetRequestList(string ReferID, string CStat, string FDate, string TDate, string Appl, string UserId, string Type, string EmployeeId, int offset = 0, int limit = 999999, string sort = null)
        {
            List<CheckStatus> StatusList = new List<CheckStatus>();
            List<ChkFrmStatus> FormRequests = new List<ChkFrmStatus>();

            try
            {
                SqlParameter[] sqlp = {
                     new SqlParameter ("ReferID", DBNull.Value ),
                      new SqlParameter("CStatus",DBNull.Value),
                         new SqlParameter("FDate", DBNull.Value),
                         new SqlParameter("TDate",DBNull.Value),
                    new SqlParameter("Applicant", DBNull.Value ),
                     new SqlParameter("UserId",UserId),
                     new SqlParameter("Type",Type),
                    new SqlParameter("EmployeeId",EmployeeId),
                    new SqlParameter("offset", offset),
                    new SqlParameter("limit", limit),
                    new SqlParameter("sort", sort)};
                if (!string.IsNullOrEmpty(ReferID))
                    sqlp[0].Value = ReferID;
                if (!string.IsNullOrEmpty(CStat))
                    sqlp[1].Value = CStat;
                if (!string.IsNullOrEmpty(FDate))
                    sqlp[2].Value = FDate;
                if (!string.IsNullOrEmpty(TDate))
                    sqlp[3].Value = TDate;
                if (!string.IsNullOrEmpty(Appl))
                    sqlp[4].Value = Appl;
                StatusList = Db.Database.SqlQuery<CheckStatus>("exec [K2_CheckStatus_new] @ReferID,@CStatus,@FDate,@TDate,@Applicant,@UserId,@Type,@EmployeeId,@offset,@limit,@sort", sqlp).ToList();

                foreach (var request in StatusList.DistinctBy(p => new { p.ProcInstID, p.FormID }))
                {
                    ChkFrmStatus status = new ChkFrmStatus();
                    //var request = StatusList.Where(P => P.FormID == FormID).FirstOrDefault();
                    status.FormID = request.FormID;
                    status.ProcInstID = request.ProcInstID;
                    status.ReferenceID = request.ReferenceID;
                    status.FormStatus = request.FormStatus;
                    status.SubmittedOn = request.SubmittedOn;
                    status.ApplicantUserId = request.ApplicantUserID;
                    status.ApplicantEMP = request.ApplicantEMP;
                    status.ApplicantFNAME = request.ApplicantFNAME;
                    status.ApproverEmp = request.ApproverEMP;
                    status.ApproverFNAME = request.ApproverFNAME;
                    status.DisplayStatus = request.DisplayStatus;
                    status.LastUser = request.LastUser;

                    status.ActionTakerFullName = request.ActionTakerFullName;
                    //status.ActionTakerStatus = request.ActionTakerStatus;
                    status.ITSApproverFullName = request.ITSApproverFullName;
                    //status.ITSApproverStatus = request.ITSApproverStatus;

                    List<ServiceLevel1> Level1lst = new List<ServiceLevel1>();
                    //foreach (string FirstLevelService in StatusList.Where(P => P.FormID == FormID).Select(P => P.MMenu).Distinct())
                    foreach (var FirstLevelService in StatusList.Where(P => P.FormID == request.FormID && P.ProcInstID == request.ProcInstID).Select(P => new { P.MMenu, P.MMenuGUID }).Distinct())
                    {
                        if (!string.IsNullOrEmpty(FirstLevelService.MMenu))
                        {
                            ServiceLevel1 Level1 = new ServiceLevel1();
                            Level1.Name = FirstLevelService.MMenu;
                            Level1.GUID = new Guid(FirstLevelService.MMenuGUID);
                            List<ServiceLevel2> Level2lst = new List<ServiceLevel2>();
                            foreach (var SecondLevelService in StatusList.Where(P => P.FormID == request.FormID && P.ProcInstID == request.ProcInstID && P.MMenu == FirstLevelService.MMenu).DistinctBy(P => P.SubMenu))
                            {
                                ServiceLevel2 Level2 = new ServiceLevel2();
                                Level2.Name = SecondLevelService.SubMenu;
                                Level2.GUID = new Guid(SecondLevelService.SubMenuGUID);
                                Level2.SValue = SecondLevelService.ServiceTypeValue;
                                List<ServiceLevel3> Level3lst = new List<ServiceLevel3>();
                                foreach (var ThirsLevelService in StatusList.Where(P => P.FormID == request.FormID && P.ProcInstID == request.ProcInstID && P.MMenu == FirstLevelService.MMenu && P.SubMenu == SecondLevelService.SubMenu).OrderBy(P => P.ServiceGUID))
                                {

                                    ServiceLevel3 Level3 = new ServiceLevel3();
                                    Level3.Name = ThirsLevelService.SSubMenu;
                                    Level3.GUID = new Guid(ThirsLevelService.SSubMenuGUID);
                                    Level3.SValue = ThirsLevelService.ServiceTypeValue;
                                    Level3.ControlFlag = ThirsLevelService.ControlFlag;
                                    Level3lst.Add(Level3);
                                }
                                if (Level3lst.Count > 0)
                                {
                                    Level2.Level3 = Level3lst;
                                    //Level2.GUID = Guid.Empty;   // To return Null Value.
                                    Level2.SValue = null;
                                }
                                else
                                { Level2.Level3 = null; }
                                Level2lst.Add(Level2);
                            }
                            if (Level2lst.Count > 0)
                                Level1.Level2 = Level2lst;
                            else
                                Level1.Level2 = null;
                            Level1lst.Add(Level1);
                        }
                    }
                    if (Level1lst.Count > 0)
                        status.RequestList = Level1lst;
                    else
                        status.RequestList = null;
                    FormRequests.Add(status);
                }

            }
            catch (Exception ex)
            {
                FormRequests = null;
                throw ex;
            }
            return FormRequests;
        }

        /// <summary>
        /// Function used for Edit & review page for the single Request
        /// </summary>
        /// <param name="ReferID">ReferenceID</param>
        /// <param name="UserId">UserId for checking authentication</param>
        /// <param name="Type">Review or DraftEdit</param>
        /// SP  : K2_Review , K2_ReviewCC 
        /// <returns></returns>
        public List<Review> GetRequestDetails(string ReferID, string UserId, string ProInstID, string Type)
        {
            List<RequestReview> MList = new List<RequestReview>();
            List<CC> CClist = new List<CC>();
            List<Review> FormRequests = new List<Review>();
            try
            {

                SqlParameter[] sqlp = {
                    new SqlParameter ("ReferID",ReferID ),
                    new SqlParameter ("UserId",UserId),
                    new SqlParameter ("Type",Type ),
                    new SqlParameter ("ProInstID", DBNull.Value)};
                if (!string.IsNullOrEmpty(ProInstID))
                    sqlp[3].Value = ProInstID;
                MList = Db.Database.SqlQuery<RequestReview>("exec [K2_Review_new] @ReferID,@UserId,@Type,@ProInstID", sqlp).ToList();
                SqlParameter[] sqlp1 = {
                     new SqlParameter ("ReferID",ReferID ),
                    new SqlParameter ("UserId",UserId ) ,
                    new SqlParameter ("Type",Type )};
                CClist = Db.Database.SqlQuery<CC>("exec [K2_ReviewCC] @ReferID,@UserId,@Type", sqlp1).ToList();

                if (MList != null)
                {
                    foreach (int FormGUID in MList.Select(P => P.FormID).Distinct())
                    {
                        Review Rev = new Review();
                        var request = MList.Where(P => P.FormID == FormGUID).FirstOrDefault();
                        Rev.FormID = FormGUID;
                        Rev.ProcInstID = request.ProcInstID;
                        Rev.ReferenceID = request.ReferenceID;
                        Rev.FormStatus = request.FormStatus;
                        Rev.CreatedOn = request.CreatedOn;
                        Rev.PreparerEMP = request.PreparerEMP;
                        Rev.PreparerUserID = request.PreparerUserID;
                        Rev.PreparerFNAME = request.PreparerFNAME;
                        Rev.ApplicantEMP = request.ApplicantEMP;
                        Rev.ApplicantUserID = request.ApplicantUserID;
                        Rev.ApplicantFNAME = request.ApplicantFNAME;
                        Rev.DEPT = request.DEPT;
                        Rev.Title = request.Title;
                        Rev.Location = request.Location;
                        Rev.ApproverEmp = request.ApproverEMP;
                        Rev.ApproverUserID = request.ApproverUserID;
                        Rev.ApproverFNAME = request.ApproverFNAME;
                        Rev.Justification = request.Justification;
                        Rev.EDeliveryDate = request.EDeliveryDate;
                        Rev.DurationOfUse = request.DurationOfUse;
                        Rev.EstimatedCost = request.EstimatedCost;
                        Rev.BudgetProvided = request.BudgetProvided;
                        Rev.BudgetSum = request.BudgetSum;
                        Rev.Remark = request.Remark;
                        Rev.ActionTakerUserID = request.ActionTakerUserID;
                        Rev.ActionTakerFullName = request.ActionTakerFullName;
                        //Rev.ActionTakerStatus = request.ActionTakerStatus;
                        Rev.ITSApproverUserID = request.ITSApproverUserID;
                        Rev.ITSApproverFullName = request.ITSApproverFullName;
                        //Rev.ITSApproverStatus = request.ITSApproverStatus;
                        //Rev.Status = request.Status;
                        Rev.DisplayStatus = request.DisplayStatus;
                        Rev.ActionTakerServiceType = request.ActionTakerServiceType;
                        List<Employee> CC1 = new List<Employee>();
                        foreach (string mm in CClist.Where(k => k.Type == "CC").Select(k => k.UID).Distinct())
                        {
                            CC1.Add(new Employee { USERID = mm, FULLNAME = CClist.Where(k => k.Type == "CC" && k.UID == mm).Select(k => k.UName).FirstOrDefault() });
                        }
                        if (CC1.Count > 0)
                            Rev.RequestCC = CC1;
                        else
                            Rev.RequestCC = null;
                        AttachFile Attach = new AttachFile();
                        List<string> FileNames = new List<string>();
                        FileNames = CClist.Where(k => k.Type == "Attachment").Select(k => k.UID).Distinct().ToList();
                        if (FileNames.Count > 0)
                            Attach.FileName = FileNames;
                        else
                            Attach.FileName = null;
                        Rev.Attachment = Attach;
                        Rev.ProcessLog = Db.ProcessLogs.Where(P => P.FormID == Rev.FormID && (P.ProcInstID.ToString() == Rev.ProcInstID || P.ProcInstID.ToString() == ProInstID) && P.ActionResult != "Forwarded").ToList();
                        foreach (var plog in Rev.ProcessLog)
                        {
                            //ProcessActionList pa = Db.ProcessActionLists.Where(P => P.K2ActionName == plog.ActionResult).FirstOrDefault();
                            var pa = (from a in Db.ProcessStepLists
                                      join b in Db.ProcessActionLists on a.StepID equals b.StepID into ps
                                      from b in ps.DefaultIfEmpty()
                                      where a.K2StepName == plog.ActivityName && b.K2ActionName == plog.ActionResult
                                      select b.ActionDisplayName
                                      ).FirstOrDefault();
                            if (pa != null)
                            {
                                plog.ActionResult = pa;
                            }
                        }

                        Rev.Attachments = Db.RequestFormAttachments.Where(P => P.FormID == Rev.FormID).ToList();
                        List<ServiceLevel1> Level1lst = new List<ServiceLevel1>();
                        //foreach (string FirstLevelService in MList.Where(P => P.FormID == FormGUID).Select(P => P.MMenu).Distinct())
                        foreach (var FirstLevelService in MList.Where(P => P.FormID == FormGUID).Select(P => new { P.MMenu, P.MMenuGUID, P.ServiceGUID }).Distinct())
                        {
                            if (!string.IsNullOrEmpty(FirstLevelService.MMenu))
                            {
                                ServiceLevel1 Level1 = new ServiceLevel1();
                                Level1.Name = FirstLevelService.MMenu;
                                Level1.ServiceGUID = FirstLevelService.ServiceGUID;
                                Level1.GUID = new Guid(FirstLevelService.MMenuGUID);
                                List<ServiceLevel2> Level2lst = new List<ServiceLevel2>();
                                var s = MList.Where(P => P.FormID == FormGUID && P.MMenu == FirstLevelService.MMenu).Distinct();
                                foreach (var SecondLevelService in MList.Where(P => P.FormID == FormGUID && P.MMenu == FirstLevelService.MMenu).DistinctBy(P => P.SubMenu))
                                {
                                    ServiceLevel2 Level2 = new ServiceLevel2();
                                    Level2.Name = SecondLevelService.SubMenu;
                                    Level2.GUID = new Guid(SecondLevelService.SubMenuGUID);
                                    Level2.ServiceGUID = SecondLevelService.ServiceGUID;
                                    Level2.SValue = SecondLevelService.ServiceTypeValue;
                                    List<ServiceLevel3> Level3lst = new List<ServiceLevel3>();
                                    foreach (var ThirsLevelService in MList.Where(P => P.FormID == FormGUID && P.MMenu == FirstLevelService.MMenu && P.SubMenu == SecondLevelService.SubMenu).OrderBy(P => P.ServiceGUID))
                                    {
                                        ServiceLevel3 Level3 = new ServiceLevel3();
                                        Level3.Name = ThirsLevelService.SSubMenu;
                                        Level3.GUID = new Guid(ThirsLevelService.ServiceTypeGUID);
                                        Level3.ServiceGUID = ThirsLevelService.ServiceGUID;
                                        if (!string.IsNullOrEmpty(ThirsLevelService.SSubMenu))
                                        {
                                            Level3.SValue = ThirsLevelService.ServiceTypeValue;
                                            Level3.Approver = ThirsLevelService.ApproverRuleCode;
                                            Level3.ActionTaker = ThirsLevelService.ActionTakerRuleCode;
                                            Level3.ControlFlag = ThirsLevelService.ControlFlag;
                                            Level3.Enabled = ThirsLevelService.Enabled;
                                            Level3.Placeholder = ThirsLevelService.Placeholder;
                                        }
                                        else
                                        {
                                            Level3.SValue = null;
                                        }
                                        Level3lst.Add(Level3);
                                    }
                                    if (Level3lst.Count > 0)
                                    {
                                        Level2.Level3 = Level3lst;
                                        //Level2.GUID = Guid.Empty;   // To return Null Value.
                                        Level2.SValue = null;
                                    }
                                    else
                                    { Level2.Level3 = null; }
                                    Level2lst.Add(Level2);
                                }
                                if (Level2lst.Count > 0)
                                    Level1.Level2 = Level2lst;
                                else
                                    Level1.Level2 = null;
                                Level1lst.Add(Level1);
                            }
                        }
                        if (Level1lst.Count > 0)
                            Rev.RequestList = Level1lst;
                        else
                            Rev.RequestList = null;
                        FormRequests.Add(Rev);
                    }
                }
                else
                {
                    throw new System.ArgumentException("The user has no enough permission to open this item");
                }
            }
            catch (Exception ex)
            {
                FormRequests = null;
                throw ex;
            }
            return FormRequests;
        }
    }
}