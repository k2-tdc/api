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
    public class AllTaskService : CommonService
    {
        private EntityContext Db = new EntityContext();
        
        /// <summary>
        /// Function used for Worklist Based on the search condition
        /// </summary>
        /// <param name="ReferID">ReferenceID (Optional)</param>
        /// <param name="CStat">Status (Optional)</param>
        /// <param name="FDate">FromDate (Optional)</param>
        /// <param name="TDate">ToDate (Optional)</param>
        /// <param name="Appl">Applicant Name (Optional)</param>
        /// <param name="UserId">UserId for checking authentication</param>
        /// <param name="Type">Task Owner</param>
        ///  SP  : K2_WorkList  
        /// <returns></returns> 
        /// <summary>        
        public List<ChkFrmStatus> GetWorklist(string ReferID, string CStat, string FDate, string TDate, string UserId, string SUser, string ProsIncId, int offset = 0, int limit = 999999, string sort = null, string applicant = null, string applicantEmpNo = null)
        {
            string proinsid = "";
            WorkflowFacade workfacade = new WorkflowFacade();
            List<WorklistItem> worklist = workfacade.GetWorklistItemsByProcess(UserId);
            List<WorklistItem> sharedworklist = new List<WorklistItem>();

            if (worklist.Count() > 0)
            {
                proinsid = String.Join(",", worklist.Select(P => P.ProcInstID)).TrimEnd(',');
            }
            if (SUser != null)
            {
                worklist = new List<WorklistItem>();
                sharedworklist = workfacade.GetWorklistItemsByProcess(SUser);
                var sharePermit = (from a in Db.DelegationLists
                                   join b in Db.DelegationProcess on a.ActivityGroupID equals b.GroupID into ps
                                   from b in ps.DefaultIfEmpty()
                                   where a.DelegationType == "Sharing"
                                        && a.FromUser_UserID == SUser
                                        && a.ToUser_UserID == UserId
                                   select b.K2StepName
                                   ).ToList();
                foreach (var i in sharedworklist)
                {
                    if (sharePermit.Contains(i.ActivityName))
                    {
                        worklist.Add(i);
                    }
                }
                // worklist.AddRange(sharedworklist);
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
                        sqlp[1].Value = CStat;
                    if (!string.IsNullOrEmpty(FDate))
                        sqlp[2].Value = FDate;
                    if (!string.IsNullOrEmpty(TDate))
                        sqlp[3].Value = TDate;
                    if (!string.IsNullOrEmpty(SUser))
                        sqlp[4].Value = SUser;
                    if (!string.IsNullOrEmpty(UserId))
                        sqlp[5].Value = UserId;
                    if (!string.IsNullOrEmpty(applicant)) {
                        sqlp[11].Value = applicant;
                    }
                    if (!string.IsNullOrEmpty(applicantEmpNo))
                    {
                        sqlp[12].Value = applicantEmpNo;
                    }

                    if (string.IsNullOrEmpty(ProsIncId))
                    {
                        if (worklist.Count > 0)
                            sqlp[7].Value = String.Join(",", worklist.Select(P => P.ProcInstID)).TrimEnd(',');
                    }
                    else
                    {
                        sqlp[7].Value = ProsIncId;
                    }

                    StatusList = Db.Database.SqlQuery<CheckStatus>("exec [K2_WorkList] @ReferID,@CStatus,@FDate,@TDate,@SUser,@UserId,@TOwner,@ProcIncId,@offset,@limit,@sort,@applicant,@applicantEmpNo", sqlp).ToList();

                    //foreach (int FormID in StatusList.Select(P => P.FormID).Distinct())
                    foreach (var request in StatusList.DistinctBy(P => P.ProcInstID))
                    {

                        ChkFrmStatus status = new ChkFrmStatus();
                        //var request = StatusList.Where(P => P.FormID == FormID).FirstOrDefault();
                        if (!string.IsNullOrEmpty(request.ProcInstID))
                        {
                            int procid = 0;
                            procid = Convert.ToInt32(request.ProcInstID);
                            if (worklist.Select(P => P.ProcInstID).Contains(procid))
                            {
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
                                status.ProcInstID = request.ProcInstID;
                                status.DisplayStatus = request.DisplayStatus;
                                status.LastUser = request.LastUser;
                                status.ActionTakerFullName = request.ActionTakerFullName;
                                status.ITSApproverFullName = request.ITSApproverFullName;

                                //if (!string.IsNullOrEmpty(request.ProcInstID))
                                //{

                                if (!proinsid.Split(',').Contains(request.ProcInstID))
                                {
                                    status.ProcessUrl = worklist.Where(p => p.ProcInstID == procid).Select(P => P.Data).FirstOrDefault() + "&SharedBy=" + SUser;
                                    status.Type = "Sharing";
                                }
                                else
                                    status.ProcessUrl = worklist.Where(p => p.ProcInstID == procid).Select(P => P.Data).FirstOrDefault();
                                //}
                                //status.ProcessUrl = worklist.Where(p => p.ProcInstID == procid).Select(P => P.Data).FirstOrDefault();
                                status.SN = worklist.Where(p => p.ProcInstID == procid).Select(P => P.SN).FirstOrDefault();
                                var actions = worklist.Where(p => p.ProcInstID == procid).Select(P => P.Actions).FirstOrDefault();
                                if (actions != null)
                                    status.actions = actions.Select(P => P.Name).ToList();
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
                                            foreach (var ThirsLevelService in StatusList.Where(P => P.FormID == request.FormID && P.ProcInstID == request.ProcInstID && P.MMenu == FirstLevelService.MMenu && P.SubMenu == SecondLevelService.SubMenu).DistinctBy(P => P.SSubMenu))
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
        
        public List<Review> GetWorklistDetails(string UserId, string ProsIncId, string SN, string RefID)
        {
            List<Review> WorkListItem = new List<Review>();
            try
            {
                WorkflowFacade workfacade = new WorkflowFacade();
                List<WorklistItem> worklist = workfacade.GetWorklistItemsByProcess(UserId);
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
                        //item.actions = actions.Select(P => P.Name).ToList();
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
                        var item = GetRequestDetails(RefID, UserId, ProsIncId, "Task").FirstOrDefault();
                        WorkListItem.Add(item);
                    }
                    else
                    {
                        throw new System.ArgumentException("The user has no enough permission to open this item");
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