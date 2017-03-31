using aZaaS.Compact.Framework.Workflow;
using HKTDC.WebAPI.CHSW.Controllers;
using HKTDC.WebAPI.CHSW.DBContext;
using HKTDC.WebAPI.CHSW.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Services
{
    public class ApprovalTaskService : CommonService
    {
        private EntityContext Db = new EntityContext();

        public List<ChkFrmStatus> GetApproveList(string ReferID, string CStat, string FDate, string TDate, string UserId, string SUser, string ProsIncId, int offset = 0, int limit = 999999, string sort = null, string applicant = null, string applicantEmpNo = null)
        {
            string proinsid = "";
            WorkflowFacade workfacade = new WorkflowFacade();
            //List<WorklistItem> worklist = workfacade.GeWorklistItemsByProcessAction(UserId, "Approval");
            //worklist.AddRange(workfacade.GeWorklistItemsByProcessAction(UserId, "ITSApproval"));
            string[] approvalStatus = { "Approval", "ITSApproval" };
            List<WorklistItem> tmpWorklist = workfacade.GetWorklistItemsByProcess(UserId);
            List<WorklistItem> worklist = tmpWorklist.Where(P => approvalStatus.Contains(P.ActivityName)).ToList();
            if (worklist.Count() > 0)
            {
                proinsid = String.Join(",", worklist.Select(P => P.ProcInstID)).TrimEnd(',');
            }


            if (SUser != null)
            {
                worklist = new List<WorklistItem>();
                List<WorklistItem> sharedworklist = workfacade.GetWorklistItemsByProcess(SUser);
                var sharePermit = (from a in Db.DelegationLists
                                   join b in Db.DelegationProcess on a.ActivityGroupID equals b.GroupID into ps
                                   from b in ps.DefaultIfEmpty()
                                   where a.DelegationType == "Sharing"
                                        && a.FromWorkerID == SUser
                                        && a.ToWorkerID == UserId
                                   select b.K2StepName
                                   ).ToList();
                worklist.AddRange(sharedworklist.Where(P => approvalStatus.Contains(P.ActivityName) && sharePermit.Contains(P.ActivityName)).ToList());
                //List<WorklistItem> sharedworklist = workfacade.GeWorklistItemsByProcessAction(SUser, "Approval");
                //worklist.AddRange(sharedworklist);
            }
            List<CheckStatus> StatusList = new List<CheckStatus>();
            List<ChkFrmStatus> FormRequests = new List<ChkFrmStatus>();
            if (worklist.Count() > 0)
            {
                try
                {
                    SqlParameter[] sqlp = {
                        new SqlParameter ("ReferID", DBNull.Value ),
                        new SqlParameter("CStatus",DBNull.Value),
                        new SqlParameter("FDate", DBNull.Value),
                        new SqlParameter("TDate",DBNull.Value),
                        new SqlParameter("SUser", DBNull.Value ),
                        new SqlParameter("userid",DBNull.Value),
                        new SqlParameter("TOwner",DBNull.Value),
                        new SqlParameter("ProcIncId",DBNull.Value),
                        new SqlParameter("offset", offset),
                        new SqlParameter("limit", limit),
                        new SqlParameter("sort", sort),
                        new SqlParameter("applicant", DBNull.Value),
                        new SqlParameter("applicantEmpNo", DBNull.Value)
                    };

                    if (!string.IsNullOrEmpty(ReferID))
                        sqlp[0].Value = ReferID;

                    if (!string.IsNullOrEmpty(CStat))
                    {
                        sqlp[1].Value = CStat;
                    }
                    else
                    {
                        sqlp[1].Value = String.Join(",", approvalStatus).TrimEnd(',');
                    }
                    //sqlp[1].Value = "Approval";

                    if (!string.IsNullOrEmpty(FDate))
                        sqlp[2].Value = FDate;
                    if (!string.IsNullOrEmpty(TDate))
                        sqlp[3].Value = TDate;
                    if (!string.IsNullOrEmpty(SUser))
                        sqlp[4].Value = SUser;
                    if (!string.IsNullOrEmpty(UserId))
                        sqlp[5].Value = UserId;
                    if (!string.IsNullOrEmpty(applicant))
                    {
                        sqlp[11].Value = applicant;
                    }
                    if (!string.IsNullOrEmpty(applicantEmpNo))
                    {
                        sqlp[12].Value = applicantEmpNo;
                    }

                    if (string.IsNullOrEmpty(ProsIncId))
                    {
                        if (worklist.Count() > 0)
                            sqlp[7].Value = String.Join(",", worklist.Select(P => P.ProcInstID)).TrimEnd(',');
                    }
                    else
                    {
                        sqlp[7].Value = ProsIncId;
                    }

                    StatusList = Db.Database.SqlQuery<CheckStatus>("exec [K2_WorkList] @ReferID,@CStatus,@FDate,@TDate,@SUser,@UserId,@TOwner,@ProcIncId,@offset,@limit,@sort,@applicant,@applicantEmpNo", sqlp).ToList();

                    //foreach (var request in StatusList.DistinctBy(P => P.FormID))
                    foreach (var request in StatusList.DistinctBy(P => P.ProcInstID))
                    {
                        ChkFrmStatus status = new ChkFrmStatus();
                        //var request = StatusList.Where(P => P.FormID == FormID).FirstOrDefault();
                        //status.FormID = FormID;
                        status.FormID = request.FormID;
                        status.ReferenceID = request.ReferenceID;
                        status.FormStatus = request.FormStatus;
                        status.SubmittedOn = request.SubmittedOn;
                        status.ApplicantUserId = request.ApplicantUserID;
                        status.ApplicantEMP = request.ApplicantEMP;
                        status.ApplicantFNAME = request.ApplicantFNAME;
                        status.ApproverEmp = request.ApproverEMP;
                        status.ApproverFNAME = request.ApproverFNAME;
                        status.ProcInstID = request.ProcInstID;
                        status.DisplayStatus = request.DisplayStatus;
                        status.LastUser = request.LastUser;
                        status.ActionTakerFullName = request.ActionTakerFullName;
                        status.ITSApproverFullName = request.ITSApproverFullName;

                        int procid = 0;
                        if (!string.IsNullOrEmpty(request.ProcInstID))
                        {
                            procid = Convert.ToInt32(request.ProcInstID);
                            if (!proinsid.Split(',').Contains(request.ProcInstID))
                            {
                                status.ProcessUrl = worklist.Where(p => p.ProcInstID == procid).Select(P => P.Data).FirstOrDefault() + "&SharedBy=" + SUser;
                                status.Type = "Sharing";
                            }
                            else
                                status.ProcessUrl = worklist.Where(p => p.ProcInstID == procid).Select(P => P.Data).FirstOrDefault();
                        }


                        status.SN = worklist.Where(p => p.ProcInstID == procid).Select(P => P.SN).FirstOrDefault();
                        /*var actions = worklist.Where(p => p.ProcInstID == procid).Select(P => P.Actions).FirstOrDefault();
                        if(actions != null)
                        status.actions = actions.Select(P => P.Name).ToList();*/

                        if (request.Permission != "2")
                        {
                            List<ServiceLevel1> Level1lst = new List<ServiceLevel1>();
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
                        }
                        FormRequests.Add(status);
                    }
                }
                catch (Exception ex)
                {
                    FormRequests = null;
                    throw ex;
                }
            }
            return FormRequests;
        }

        public List<Review> GetApproveDetails(string UserId, string ProsIncId, string SN)
        {
            List<Review> WorkListItem = new List<Review>();
            try
            {
                WorkflowFacade workfacade = new WorkflowFacade();
                List<WorklistItem> worklist = workfacade.GetWorklistItemsByProcess(UserId);
                //List<WorklistItem> worklist = workfacade.GeWorklistItemsByProcessAction(UserId, "Approval");
                //worklist.AddRange(workfacade.GeWorklistItemsByProcessAction(UserId, "ITSApproval"));
                if (worklist.Where(P => P.SN == SN).Count() > 0)
                {
                    //string ReferID = Db.RequestFormMasters.Where(P => P.ProcInstID == ProsIncId).Select(P => P.ReferenceID).FirstOrDefault().ToString();
                    string ReferID = (from a in Db.RequestFormMasters
                                      join b in Db.RequestFormTaskActioners on a.FormID equals b.FormID into ps
                                      from b in ps.DefaultIfEmpty()
                                      where (a.ProcInstID == ProsIncId || b.ProcInstID.ToString() == ProsIncId)
                                      select a.ReferenceID).FirstOrDefault().ToString();

                    if (!string.IsNullOrEmpty(ReferID))
                    {
                        int procid = 0;
                        procid = Convert.ToInt32(ProsIncId);
                        var item = GetRequestDetails(ReferID, UserId, ProsIncId, "Task").FirstOrDefault();

                        var actions = worklist.Where(p => p.ProcInstID == procid).Select(P => P.Actions).FirstOrDefault();
                        /*item.actions = actions.Select(P => P.Name).ToList();*/
                        var items = actions.Select(P => P.Name).ToList();
                        item.actions = (from a in Db.ProcessStepLists
                                        join b in Db.ProcessActionLists on a.StepID equals b.StepID
                                        where (a.StepName == item.FormStatus)
                                        && items.Contains(b.K2ActionName)
                                        select new ProcessActionListDTO
                                        {
                                            ActionID = b.ActionID,
                                            Action = b.K2ActionName,
                                            ButtonName = b.ActionButtonName
                                        }).ToList();

                        // Approver return to Applicant, Prepare = Applicant, no return to preparer button
                        if (item.FormStatus == "Return")
                        {
                            if (item.PreparerUserID == item.ApplicantUserID)
                            {
                                var returnToPreparerBtn = item.actions.Where(p => p.Action == "Rework").FirstOrDefault();
                                item.actions.Remove(returnToPreparerBtn);
                            }
                        }

                        WorkListItem.Add(item);
                    }
                }
                else
                {
                    List<CheckDelegationUser> record = new List<CheckDelegationUser>();
                    SqlParameter[] sqlp = {
                     new SqlParameter ("ProcInstId", ProsIncId),
                     new SqlParameter("SUser", UserId)};

                    record = Db.Database.SqlQuery<CheckDelegationUser>("exec [K2_checkSharedUser] @ProcInstId,@SUser", sqlp).ToList();
                    if (record.Count() > 0)
                    {
                        string ReferID = (from a in Db.RequestFormMasters
                                          join b in Db.RequestFormTaskActioners on a.FormID equals b.FormID into ps
                                          from b in ps.DefaultIfEmpty()
                                          where (a.ProcInstID == ProsIncId || b.ProcInstID.ToString() == ProsIncId)
                                          select a.ReferenceID).FirstOrDefault().ToString();

                        var item = GetRequestDetails(ReferID, UserId, ProsIncId, "Task").FirstOrDefault();
                        string permission = "";
                        foreach(var r in record)
                        {
                            if (!string.IsNullOrEmpty(r.Permission))
                            {
                                if (r.Permission == "1")
                                    permission = "1";
                                else
                                {
                                    if (permission != "1")
                                    {
                                        permission = r.Permission;
                                    }
                                }
                            }
                        }
                        if(permission == "2")
                        {
                            item.Attachments = null;
                            item.RequestList = null;
                            item.Justification = null;
                            item.EDeliveryDate = null;
                            item.DurationOfUse = null;
                            item.EstimatedCost = null;
                            item.BudgetProvided = null;
                            item.RequestCC = null;
                            item.Remark = null;
                        }
                        item.Permission = permission;
                        WorkListItem.Add(item);
                    }
                    else
                    {
                        //throw new System.ArgumentException("The user has no enough permission to open this item");
                        return null;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return WorkListItem;
        }
    }
}