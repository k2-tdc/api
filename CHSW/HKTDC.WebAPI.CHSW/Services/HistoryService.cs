using HKTDC.WebAPI.CHSW.DBContext;
using HKTDC.WebAPI.CHSW.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Services
{
    public class HistoryService : BaseService
    {
        private EntityContext Db = new EntityContext();

        public List<ChkFrmStatus> getApprovalList(string userid, string employeeid, string applicant, string approvalStartDate, string approvalEndDate, string status, string refid, string createStartDate, string createEndDate, string keyword)
        {
            List<ApproverHistory> rawHistory = new List<ApproverHistory>();
            List<ChkFrmStatus> historyList = new List<ChkFrmStatus>();
            string queryParameter = "@userid,@applicantEmpNo,@applicant,@approvalStart,@approvalEnd,@status,@refid,@createStart,@createEnd,@keyword";
            SqlParameter[] sqlp =
            {
                new SqlParameter("userid", userid),
                new SqlParameter("applicantEmpNo", DBNull.Value),
                new SqlParameter("applicant", DBNull.Value),
                new SqlParameter("approvalStart", DBNull.Value),
                new SqlParameter("approvalEnd", DBNull.Value),
                new SqlParameter("status", DBNull.Value),
                new SqlParameter("refid", DBNull.Value),
                new SqlParameter("createStart", DBNull.Value),
                new SqlParameter("createEnd", DBNull.Value),
                new SqlParameter("keyword", DBNull.Value)
            };
            if (!string.IsNullOrEmpty(employeeid))
            {
                sqlp[1].Value = employeeid;
            }
            if (!String.IsNullOrEmpty(applicant))
            {
                sqlp[2].Value = applicant;
            }
            if (!String.IsNullOrEmpty(approvalStartDate))
            {
                sqlp[3].Value = approvalStartDate;
            }
            if (!String.IsNullOrEmpty(approvalEndDate))
            {
                sqlp[4].Value = approvalEndDate;
            }
            if (!String.IsNullOrEmpty(status))
            {
                sqlp[5].Value = status;
            }
            if (!String.IsNullOrEmpty(refid))
            {
                sqlp[6].Value = refid;
            }
            if (!String.IsNullOrEmpty(createStartDate))
            {
                sqlp[7].Value = createStartDate;
            }
            if (!String.IsNullOrEmpty(createEndDate))
            {
                sqlp[8].Value = createEndDate;
            }
            if (!String.IsNullOrEmpty(keyword))
            {
                sqlp[9].Value = keyword;
            }
            rawHistory =  Db.Database.SqlQuery<ApproverHistory>("exec [K2_ApproverHistory] " + queryParameter, sqlp.ToArray()).ToList();
            
            foreach (var request in rawHistory.DistinctBy(p => p.ProcInstID))
            {
                ChkFrmStatus tmpStatus = new ChkFrmStatus();
                tmpStatus.FormID = request.FormID;
                tmpStatus.ProcInstID = request.ProcInstID.ToString();
                tmpStatus.ReferenceID = request.ReferenceID;
                tmpStatus.FormStatus = request.FormStatus;
                tmpStatus.SubmittedOn = request.CommentedOn;
                tmpStatus.ApplicantEMP = request.ApplicantEmployeeID;
                tmpStatus.ApplicantFNAME = request.ApplicantFullName;
                tmpStatus.ApproverEmp = request.ApproverEmployeeID;
                tmpStatus.ApproverFNAME = request.ApproverFullName;
                tmpStatus.DisplayStatus = request.DisplayStatus;
                tmpStatus.LastUser = request.LastUser;

                tmpStatus.ActionTakerFullName = request.ActionTakerFullName;
                tmpStatus.ITSApproverFullName = request.ITSApproverFullName;
                tmpStatus.ApplicantUserId = request.ApplicantUserId;

                List<ServiceLevel1> Level1lst = new List<ServiceLevel1>();
                foreach (var FirstLevelService in rawHistory.Where(P => P.ProcessLogID == request.ProcessLogID && P.ProcInstID == request.ProcInstID).Select(P => new { P.MMenu, P.MMenuGUID }).Distinct())
                {
                    ServiceLevel1 Level1 = new ServiceLevel1();
                    Level1.Name = FirstLevelService.MMenu;
                    Level1.GUID = FirstLevelService.MMenuGUID;
                    List<ServiceLevel2> Level2lst = new List<ServiceLevel2>();
                    foreach (var SecondLevelService in rawHistory.Where(P => P.ProcessLogID == request.ProcessLogID && P.ProcInstID == request.ProcInstID  && P.MMenu == FirstLevelService.MMenu).DistinctBy(P => P.SubMenu))
                    {
                        ServiceLevel2 Level2 = new ServiceLevel2();
                        Level2.Name = SecondLevelService.SubMenu;
                        Level2.GUID = SecondLevelService.SubMenuGUID;
                        Level2.SValue = SecondLevelService.ServiceTypeValue;
                        List<ServiceLevel3> Level3lst = new List<ServiceLevel3>();
                        foreach (var ThirsLevelService in rawHistory.Where(P => P.ProcessLogID == request.ProcessLogID && P.ProcInstID == request.ProcInstID  && P.MMenu == FirstLevelService.MMenu && P.SubMenu == SecondLevelService.SubMenu).OrderBy(P => P.ServiceGUID))
                        {
                            ServiceLevel3 Level3 = new ServiceLevel3();
                            Level3.Name = ThirsLevelService.SSubMenu;
                            Level3.GUID = ThirsLevelService.SSubMenuGUID;
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

                if (Level1lst.Count > 0)
                    tmpStatus.RequestList = Level1lst;
                else
                    tmpStatus.RequestList = null;
                historyList.Add(tmpStatus);
            }
            return historyList;
        }

        public List<Review> GetHistoryDetails(string ReferID, string UserId, string ProInstID)
        {
            List<RequestReview> MList = new List<RequestReview>();
            List<CC> CClist = new List<CC>();
            List<Review> FormRequests = new List<Review>();
            try
            {
                SqlParameter[] sqlp = {
                    new SqlParameter ("ReferID",ReferID ),
                    new SqlParameter ("UserId",UserId),
                    new SqlParameter ("Type","History" ),
                    new SqlParameter ("ProInstID", DBNull.Value)};
                if (!string.IsNullOrEmpty(ProInstID))
                    sqlp[3].Value = ProInstID;
                MList = Db.Database.SqlQuery<RequestReview>("exec [K2_Review_new] @ReferID,@UserId,@Type,@ProInstID", sqlp).ToList();
                SqlParameter[] sqlp1 = {
                     new SqlParameter ("ReferID",ReferID ),
                    new SqlParameter ("UserId",UserId ) ,
                    new SqlParameter ("Type","Review" )};
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
                        foreach (var FirstLevelService in MList.Where(P => P.FormID == FormGUID).Select(P => new { P.MMenu, P.MMenuGUID }).Distinct())
                        {
                            if (!string.IsNullOrEmpty(FirstLevelService.MMenu))
                            {
                                ServiceLevel1 Level1 = new ServiceLevel1();
                                Level1.Name = FirstLevelService.MMenu;
                                Level1.GUID = new Guid(FirstLevelService.MMenuGUID);
                                List<ServiceLevel2> Level2lst = new List<ServiceLevel2>();
                                var s = MList.Where(P => P.FormID == FormGUID && P.MMenu == FirstLevelService.MMenu).Distinct();
                                foreach (var SecondLevelService in MList.Where(P => P.FormID == FormGUID && P.MMenu == FirstLevelService.MMenu).DistinctBy(P => P.SubMenu))
                                {
                                    ServiceLevel2 Level2 = new ServiceLevel2();
                                    Level2.Name = SecondLevelService.SubMenu;
                                    Level2.GUID = new Guid(SecondLevelService.SubMenuGUID);
                                    Level2.SValue = SecondLevelService.ServiceTypeValue;
                                    List<ServiceLevel3> Level3lst = new List<ServiceLevel3>();
                                    foreach (var ThirsLevelService in MList.Where(P => P.FormID == FormGUID && P.MMenu == FirstLevelService.MMenu && P.SubMenu == SecondLevelService.SubMenu).OrderBy(P => P.ServiceGUID))
                                    {
                                        ServiceLevel3 Level3 = new ServiceLevel3();
                                        Level3.Name = ThirsLevelService.SSubMenu;
                                        Level3.GUID = new Guid(ThirsLevelService.ServiceTypeGUID);
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