using HKTDC.WebAPI.CHSW.DBContext;
using HKTDC.WebAPI.CHSW.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using Newtonsoft.Json;
using System.Data;
using System.Web.Http;
using System.Net;
using System.Security.Principal;
using HKTDC.WebAPI.CHSW.Controllers;
using System.Data.Entity;
using aZaaS.Compact.Framework.Workflow;

namespace HKTDC.WebAPI.CHSW.Services
{
    public class RequestFormService : CommonService
    {
        private EntityContext Db = new EntityContext();

        /// <summary>
        /// To Save the New Request Details From the related Tables
        /// </summary>
        /// <param name="json">XML Data</param>
        /// SP : K2_EditDRAFT
        /// <returns>Return FormID else Failed</returns>
        public string SaveNewRequest(string json)
        {
            string result = "Failed";
            try
            {
                XmlNode node = JsonConvert.DeserializeXmlNode(json, "Request");

                SqlParameter[] sqlp = {
                     new SqlParameter ("xmlData",node.InnerXml)  // to dend XML data to SP
                };
                var results = Db.Database.SqlQuery<string>("exec [K2_EditDRAFT_new] @xmlData", sqlp).ToList();
                result = results[0].ToString();
                //   ErrorLog(null, "Save req" + result);
            }
            catch (Exception ex)
            {
                result = null;
                throw ex;
            }
            return result;
        }

        public void startWorkFlow(string refId)
        {
            try
            {
                RequestFormMaster RFM = new RequestFormMaster();
                RFM = Db.RequestFormMasters.Where(p => p.ReferenceID == refId).FirstOrDefault();
                if (string.IsNullOrEmpty(RFM.ProcInstID) && RFM.FormStatus != "Draft")
                {

                    WorkflowFacade workfacade = new WorkflowFacade();
                    int PRInstID = workfacade.StartProcessInstance(RFM.FormID, RFM.PreparerUserID, RFM.ReferenceID, RFM.Remark);
                    if (PRInstID > 0)
                    {
                        RFM.ProcInstID = PRInstID.ToString();
                        Db.Entry(RFM).State = EntityState.Modified;
                        Db.SaveChanges();
                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        #region old code
        /// <summary>
        ///  Get the Processlist & Menu Item basd on the login User
        ///  if the ProcessID Empty Take the Default Value.
        /// </summary>
        /// <param name="UserId">UserID</param>
        /// <param name="ProcessId">Process ID</param>
        /// SP  : K2_MMMenu,K2_ProcessList
        /// <returns></returns>
        //public Menus GetMenuItem(string UserId, string ProcessId)
        //{
        //    List<MenuList> MList = new List<MenuList>();
        //    List<Menus> FullMenu = new List<Menus>();
        //    List<ProcessMenu> PList = new List<ProcessMenu>();

        //    Menus returnMenu = new Menus();

        //    try
        //    {
        //        SqlParameter[] sqlp = {
        //             new SqlParameter ("ProcessId",DBNull.Value),
        //             new SqlParameter("EmployeeId",UserId)};
        //        if (!string.IsNullOrEmpty(ProcessId))
        //        {
        //            sqlp[0].Value = ProcessId;
        //        }

        //        MList = Db.Database.SqlQuery<MenuList>("exec [K2_MMMenu] @ProcessId,@EmployeeId", sqlp).ToList();
        //        if (string.IsNullOrEmpty(ProcessId)) // Login Time Process Id Null  and Return Processlist
        //        {
        //            SqlParameter[] sqlp2 = {
        //             new SqlParameter("EmployeeId",UserId)};
        //            PList = Db.Database.SqlQuery<ProcessMenu>("exec [K2_ProcessList] @EmployeeId", sqlp2).ToList();


        //        }

        //        //}

        //        WorkflowFacade workfacade = new WorkflowFacade();
        //        List<WorklistItem> worklist = null;
        //        //foreach (string FirstlevelMenu in MList.Select(P => P.MMenu).Distinct())
        //        foreach (var FirstlevelMenu in MList.DistinctBy(P => P.MMenu))
        //        {
        //            if (!string.IsNullOrEmpty(FirstlevelMenu.MMenu))
        //            {
        //                Menus menus = new Menus();
        //                //menus.UserName = MList.Select(P => P.UserName).Distinct().SingleOrDefault();
        //                menus.UserName = FirstlevelMenu.UserName;
        //                //menus.UserID = MList.Select(P => P.User_ID).Distinct().SingleOrDefault();
        //                menus.UserID = FirstlevelMenu.User_ID;
        //                menus.Name = FirstlevelMenu.MMenu;
        //                menus.EmployeeNo = FirstlevelMenu.EMPLOYEENO;                    
        //                List<Submenu> submenulist = new List<Submenu>();
        //                string Link2 = "";
        //                foreach (string SecondLevelMenu in MList.Where(P => P.MMenu == FirstlevelMenu.MMenu).Select(P => P.SubMenu).Distinct())
        //                {
        //                    if (!string.IsNullOrEmpty(SecondLevelMenu))
        //                    {
        //                        Submenu submenu = new Submenu();
        //                        submenu.Name = SecondLevelMenu;
        //                        if ((MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu).Select(P => P.DOrder).Distinct().FirstOrDefault().Length) == 2)
        //                        {
        //                            Link2 = MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu).Select(P => P.Menulink).Distinct().FirstOrDefault();
        //                            submenu.Scount = MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu).Select(P => P.Scount).Distinct().FirstOrDefault();
        //                            /*if (StringUtil.Equals(submenu.Name, "ALL TASKS"))
        //                            {
        //                                WorkflowFacade workfacade = new WorkflowFacade();
        //                                List<WorklistItem> worklist = workfacade.GetWorklistItemsByProcess(UserId);
        //                                int[] procInstID = worklist.Select(p => p.ProcInstID).ToArray();
        //                                string[] pids = Array.ConvertAll(procInstID, x => x.ToString());
        //                                var count = (from a in Db.RequestFormMasters where (pids.Contains(a.ProcInstID)) select a.FormID).Count();

        //                                submenu.Scount = count.ToString();
        //                            }
        //                            if (StringUtil.Equals(submenu.Name, "APPROVAL TASKS"))
        //                            {
        //                                WorkflowFacade workfacade = new WorkflowFacade();
        //                                List<WorklistItem> worklist = workfacade.GeWorklistItemsByProcessAction(UserId, "Approval");
        //                                worklist.AddRange(workfacade.GeWorklistItemsByProcessAction(UserId, "ITSApproval"));
        //                                submenu.Scount = worklist.Count().ToString();
        //                            }*/
        //                            if (!string.IsNullOrEmpty(Link2))
        //                            {
        //                                submenu.Mlink = Link2;
        //                            }
        //                            else
        //                            {
        //                                submenu.Mlink = "#";
        //                            }
        //                        }
        //                        List<SSubMenu> ssubmenulist = new List<SSubMenu>();
        //                        string Link3 = "";
        //                        foreach (string ThirdLevelMenu in MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu).Select(P => P.SSubMenu).Distinct())
        //                        {
        //                            if (!string.IsNullOrEmpty(ThirdLevelMenu))
        //                            {
        //                                SSubMenu ssubmenu = new SSubMenu();
        //                                ssubmenu.Name = ThirdLevelMenu;
        //                                ssubmenulist.Add(ssubmenu);
        //                                if ((MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu && P.SSubMenu == ThirdLevelMenu).Select(P => P.DOrder).Distinct().FirstOrDefault().Length) == 3)
        //                                {
        //                                    Link3 = MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu && P.SSubMenu == ThirdLevelMenu).Select(P => P.Menulink).Distinct().FirstOrDefault();
        //                                    ssubmenu.Scount = MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu && P.SSubMenu == ThirdLevelMenu).Select(P => P.Scount).Distinct().FirstOrDefault();
        //                                    if (Utility.Equals(ssubmenu.Name, "ALL TASKS"))
        //                                    {
        //                                        if (worklist == null)
        //                                        {
        //                                            worklist = workfacade.GetWorklistItemsByProcess(UserId);
        //                                        }
        //                                        int[] procInstID = worklist.Select(p => p.ProcInstID).ToArray();                                                
        //                                        string[] pids = Array.ConvertAll(procInstID, x => x.ToString());
        //                                        var count = (from a in Db.RequestFormMasters
        //                                                     join b in Db.RequestFormTaskActioners on a.FormID equals b.FormID into ps
        //                                                     from b in ps.DefaultIfEmpty()
        //                                                     where (pids.Contains(a.ProcInstID) || pids.Contains(b.ProcInstID.ToString()))
        //                                                     select a.FormID).Count();

        //                                        ssubmenu.Scount = count.ToString();
        //                                    }
        //                                    if(Utility.Equals(ssubmenu.Name, "APPROVAL TASKS"))
        //                                    {
        //                                        if (worklist == null)
        //                                        {
        //                                            worklist = workfacade.GetWorklistItemsByProcess(UserId);
        //                                            //worklist = workfacade.GeWorklistItemsByProcessAction(UserId, "Approval");
        //                                            //worklist.AddRange(workfacade.GeWorklistItemsByProcessAction(UserId, "ITSApproval"));
        //                                        }
        //                                        string[] approvalStatus = { "Approval", "ITSApproval" };
        //                                        //ssubmenu.Scount = worklist.Where(P => approvalStatus.Contains(P.ActivityName)).Count().ToString();

        //                                        int[] procInstID = worklist.Where(P => approvalStatus.Contains(P.ActivityName)).Select(p => p.ProcInstID).ToArray();
        //                                        string[] pids = Array.ConvertAll(procInstID, x => x.ToString());
        //                                        var count = (from a in Db.RequestFormMasters
        //                                                     join b in Db.RequestFormTaskActioners on a.FormID equals b.FormID into ps
        //                                                     from b in ps.DefaultIfEmpty()
        //                                                     where (pids.Contains(a.ProcInstID) || pids.Contains(b.ProcInstID.ToString()))
        //                                                     select a.FormID).Count();

        //                                        ssubmenu.Scount = count.ToString();
        //                                    }
        //                                    if (!string.IsNullOrEmpty(Link3))
        //                                    {
        //                                        ssubmenu.Mlink = Link3;  //If the 3rd Level link value is Not Null
        //                                    }
        //                                    else
        //                                    {
        //                                        ssubmenu.Mlink = "#";
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        if (ssubmenulist.Count > 0)
        //                            submenu.sumenu = ssubmenulist;
        //                        else
        //                            submenu.sumenu = null;
        //                        if (string.IsNullOrEmpty(submenu.Mlink))
        //                        {
        //                            submenu.Mlink = "#"; //If the 2nd Level link value is Null
        //                        }
        //                        submenulist.Add(submenu);
        //                    }
        //                }
        //                if (submenulist.Count > 0)
        //                    menus.Menu = submenulist;
        //                else
        //                    menus.Menu = null;
        //                if (PList.Count > 0)
        //                    menus.PList = PList;
        //                //FullMenu.Add(menus);
        //                returnMenu = menus;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        returnMenu = null;
        //        throw ex;
        //    }
        //    return returnMenu;
        //}

        ///// <summary>
        ///// Used to return  Selected Applican Details
        ///// VIEW : VW_EMPLOYEE
        ///// </summary>
        ///// <param name="UserId"></param>
        ///// <returns>Retun format ApplicantDetails</returns>
        //public ApplicantDetails GetApplicantDetails(string UserId)
        //{
        //    ApplicantDetails applicant = new ApplicantDetails();
        //    try
        //    {
        //        applicant = Db.VW_EMPLOYEE.ToList().Where(X => X.UserID == UserId).Select(x => new ApplicantDetails
        //        {
        //            Depart = x.DEPT,
        //            Office = x.LocationCode,
        //            Title = x.ROLE
        //        }).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        applicant = null;
        //        throw ex;

        //    }
        //    return applicant;
        //}

        ///// <summary>
        ///// To Get the Applicant Or Recommed by Details 
        ///// </summary>
        ///// <param name="RuleID">IT0005,IT0008</param>
        ///// <param name="UserId">ApporverID</param>
        ///// <param name="WorkId">UserID</param>
        /////  SP  : pProcessWorkerGet (Refer the pProcessWorkerGet SP From  Workflow DataBase )
        ///// <returns>Applicant list</returns>
        //public List<Applicant> GetAllEmployeeDetails(string RuleID, string WorkId, string UserId, string EstCost)
        //{
        //    try
        //    {
        //        List<Applicant> employees = new List<Applicant>();
        //        SqlParameter[] sqlp = {
        //             new SqlParameter ("RuleCode",RuleID ),
        //             new SqlParameter("UserID",DBNull.Value),
        //            new SqlParameter("WorkerID", DBNull.Value),
        //            new SqlParameter("EstCost", DBNull.Value)};
        //        if (!string.IsNullOrEmpty(UserId))
        //        {
        //            sqlp[1].Value = UserId;
        //        }
        //        if (!string.IsNullOrEmpty(WorkId))
        //        {
        //            sqlp[2].Value = WorkId;
        //        }
        //        if (!string.IsNullOrEmpty(EstCost))
        //        {
        //            sqlp[3].Value = EstCost;
        //        }
        //            employees = Db.Database.SqlQuery<Applicant>("exec [pProcessWorkerGet] @RuleCode,@UserID,@WorkerID,@EstCost", sqlp).ToList(); // Refer the pProcessWorkerGet SP From  Workflow DataBase
        //        if (!string.IsNullOrEmpty(EstCost))
        //        {
        //            return employees.DistinctBy(p=>p.WorkerId).ToList();
        //        }
        //        else
        //        {
        //            return employees;
        //        }
                   
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<VWEmployeeDTO> GetAllEmployee()
        //{ 
        //    return Db.VW_EMPLOYEE.Select(p => new VWEmployeeDTO {
        //                UserId = p.UserID,
        //                UserFullName = p.FullName
        //            }).OrderBy(p => p.UserFullName).ToList();
        //}
        
        ///// <summary>
        ///// To get the List of Status
        ///// Table  : ProcessStepList
        ///// </summary>
        ///// <returns></returns>
        //public List<Reference> GetStatusDetails(string task)
        //{

        //    List<Reference> PStatus = new List<Reference>();
        //    try
        //    {
        //        PStatus = Db.ProcessStepLists.Where(x => x.WorkList.Contains(task)).Select(x => new Reference
        //        {
        //            ReferenceID = x.StepName,
        //            ReferenceName = x.StepDisplayName
        //        }).Distinct().ToList(); ;
        //    }
        //    catch (Exception ex)
        //    {
        //        PStatus = null;
        //        throw ex;
        //    }

        //    return PStatus;
        //}
        
        ///// <summary>
        ///// To Return the Process List Details
        ///// SP : K2_ProcessList
        ///// </summary>
        ///// <param name="UserId">Login User ID</param>
        ///// <returns></returns>
        //public List<ProcessMenu> ProcessList(String UserId)
        //{
        //    SqlParameter[] sqlp2 = {
        //                     new SqlParameter("EmployeeId",UserId)};
        //    return Db.Database.SqlQuery<ProcessMenu>("exec [K2_ProcessList] @EmployeeId", sqlp2).ToList();
        //}

        ///// <summary>
        ///// To Return the Process Step List Details
        ///// Tb : ProcessStepLists
        ///// </summary>
        ///// <param name="ProId">ProcessId</param>
        ///// <returns></returns>
        //public List<ProcessStepList> ProcessStepList(int ProId)
        //{
        //    return Db.ProcessStepLists.Where(P => P.ProcessID == ProId && P.ShowInDelegation == "Y").ToList();
        //}

        ///// <summary>
        ///// To Save the New Delegation Details From the related Tables
        ///// </summary>
        ///// <param name="json">XML Data</param>
        ///// SP : K2_SaveDelegation
        ///// <returns>Return FormID else Failed</returns>
        //public string SaveNewDelegation(string json)
        //{
        //    string result = "Failed";
        //    try
        //    {
        //        XmlNode node = JsonConvert.DeserializeXmlNode(json, "Delegation");

        //        SqlParameter[] sqlp = {
        //             new SqlParameter ("xmlData",node.InnerXml)  // to dend XML data to SP
        //        };
        //        var results = Db.Database.SqlQuery<string>("exec [K2_SaveDelegation] @xmlData", sqlp).ToList();
        //        result = results[0].ToString();
        //        //   ErrorLog(null, "Save req" + result);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = null;
        //        throw ex;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// To return the Delegation Details
        ///// </summary>
        ///// <param name="UserId">User Id From the  Login User</param>
        ///// <returns></returns>
        //public List<DelegationDetails> GetAllDelegationDetails(string UserId, string DeleId, string ProId, string StepId, string Type)
        //{
        //    SqlParameter[] sqlp = {
        //             new SqlParameter ("UserId", DBNull.Value ),
        //              new SqlParameter("DeleId",DBNull.Value),
        //                 new SqlParameter("ProId", DBNull.Value),
        //                 new SqlParameter("StepId",DBNull.Value),
        //            new SqlParameter("Type", DBNull.Value )};
        //    if (!string.IsNullOrEmpty(UserId))
        //        sqlp[0].Value = UserId;
        //    if (!string.IsNullOrEmpty(DeleId))
        //        sqlp[1].Value = DeleId;
        //    if (!string.IsNullOrEmpty(ProId))
        //        sqlp[2].Value = ProId;
        //    if (!string.IsNullOrEmpty(StepId))
        //        sqlp[3].Value = StepId;
        //    if (!string.IsNullOrEmpty(Type))
        //        sqlp[4].Value = Type;

        //    return Db.Database.SqlQuery<DelegationDetails>("exec [K2_DelegationList] @UserId,@DeleId,@ProId,@StepId,@Type", sqlp).ToList();
        //}

        ///// <summary>
        ///// Delete the Draft & related data from the DB  & related Attached Files From the UNC Path
        ///// </summary>
        ///// <param name="refid">User Sent reference ID</param>
        ///// <returns> it's successfull sent SUCCESS else failed</returns>
        ///// SP: K2_DeleteDraft
        //public string DeleteDelegationDetails(string DeleId)
        //{
        //    string result = "Failed";
        //    try
        //    {
        //        int Did = Convert.ToInt32(DeleId);
        //        var Delegation = new DelegationList() { DelegationID = Did };
        //        Db.DelegationLists.Attach(Delegation);
        //        Db.DelegationLists.Remove(Delegation);
        //        Db.SaveChanges();
        //        result = "SUCCESS";
        //    }

        //    catch (Exception ex)
        //    {
        //        result = "Failed";
        //        throw ex;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// To Retun the Delegation List Based on the Type
        ///// </summary>
        ///// <param name="Type">Delegation or Sharing</param>
        ///// <returns></returns>
        //public List<DelegationList> DelegationList(string Type, string UserId)
        //{
        //    return Db.DelegationLists.Where(P => P.DelegationType == Type && P.ToUser_USER_ID == UserId).DistinctBy(p => p.FromUser_USER_ID).ToList();
        //}

        ///// <summary>
        ///// To Retun the Delegation List Based on the Type
        ///// </summary>
        ///// <param name="Type">Delegation or Sharing</param>
        ///// <returns></returns>
        //public string WorklistAction(string UserId, string SN, string ActionName, string Comment, string forwardUserID)
        //{
        //    string result = "";
        //    try
        //    {
        //        if(!string.IsNullOrEmpty(forwardUserID))
        //        {
        //            int underScorePos = SN.IndexOf("_");
        //            string procInstID = SN.Substring(0, underScorePos);
        //            var forwardUserInfo = Db.VW_EMPLOYEE.Where(p => p.UserID == forwardUserID).FirstOrDefault();
        //            var actionTakerRecord = Db.RequestFormTaskActioners.Where(p => p.ProcInstID.ToString() == procInstID).FirstOrDefault();
        //            if(!string.IsNullOrEmpty(actionTakerRecord.ActionTakerUserID))
        //            {
        //                actionTakerRecord.ActionTakerUserID = forwardUserInfo.UserID;
        //                actionTakerRecord.ActionTakerEmployeeID = forwardUserInfo.EmployeeID;
        //                actionTakerRecord.ActionTakerFullName = forwardUserInfo.FullName;
        //                actionTakerRecord.ActionTakerDeptName = forwardUserInfo.DEPT;
        //                Db.SaveChanges();
        //            }
        //        }
        //        WorkflowFacade workfacade = new WorkflowFacade();
        //        workfacade.ExecuteAction(UserId, SN, ActionName, Comment);
        //        result = "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        result = "Failed";
        //        throw ex;
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// To Get the Applicant List for Listing page
        ///// </summary>
        ///// <param name="RuleID">IT0005,IT0008</param>
        ///// <param name="UserId">ApporverID</param>
        ///// <param name="WorkId">UserID</param>
        ///// Type - Draft / CheckStatus
        /////  SP  : pProcessWorkerGet_GetApplicant (Refer the pProcessWorkerGet SP From  Workflow DataBase )
        ///// <returns>Applicant list</returns>
        //public List<Applicant> GetApplicantList(string RuleID, string WorkId, string UserId, string Type, string EmployeeId)
        //{
        //    try
        //    {
        //        List<Applicant> employees = new List<Applicant>();
        //        SqlParameter[] sqlp = {
        //             new SqlParameter ("RuleCode",RuleID ),
        //             new SqlParameter("UserID",DBNull.Value),
        //            new SqlParameter("WorkerID", DBNull.Value),
        //            new SqlParameter("Type", DBNull.Value),
        //            new SqlParameter("EmployeeId", EmployeeId)};
        //        if (!string.IsNullOrEmpty(UserId))
        //        {
        //            sqlp[1].Value = UserId;
        //        }
        //        if (!string.IsNullOrEmpty(WorkId))
        //        {
        //            sqlp[2].Value = WorkId;
        //        }
        //        if (!string.IsNullOrEmpty(Type))
        //        {
        //            sqlp[3].Value = Type;
        //        }
        //        employees = Db.Database.SqlQuery<Applicant>("exec [pProcessWorkerGet_GetApplicant] @RuleCode,@UserID,@WorkerID,@Type,@EmployeeId", sqlp).ToList(); // Refer the pProcessWorkerGet_GetApplicant SP From  Workflow DataBase

        //        return employees.DistinctBy(p => p.UserId).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        
        //public void DeleteFile(string GUID)
        //{
        //    try
        //    {
        //        var fileRecord = Db.RequestFormAttachments.Where(p => p.AttachmentGUID == new Guid(GUID)).FirstOrDefault();
        //        if (fileRecord != null)
        //        {
        //            string Dircur = ConfigurationManager.AppSettings.Get("UNCPath") + fileRecord.FormID.ToString(); //UNC Path From Web Config
        //            foreach (string s in System.IO.Directory.GetFiles(Dircur, fileRecord.AttachmentGUID + ".*"))
        //            {
        //                System.IO.File.Delete(s);
        //            }
        //            Db.RequestFormAttachments.Remove(fileRecord);
        //            Db.SaveChanges();
        //        }
        //    } catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<Applicant> GetEmployeeDetailsWithRole(string RuleID, string WorkId)
        //{
        //    try
        //    {
        //        List<Applicant> employees = new List<Applicant>();
        //        SqlParameter[] sqlp = {
        //             new SqlParameter ("RuleCode",RuleID ),
        //             new SqlParameter("UserID",DBNull.Value),
        //            new SqlParameter("WorkerID", DBNull.Value)};
        //        if (!string.IsNullOrEmpty(WorkId))
        //        {
        //            sqlp[2].Value = WorkId;
        //        }
        //        return Db.Database.SqlQuery<Applicant>("exec [pProcessWorkerGet_FromUser] @RuleCode,@UserID,@WorkerID", sqlp).ToList();     
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void Redirect(string sn, string UserId)
        //{
        //    try
        //    {
        //        WorkflowFacade workfacade = new WorkflowFacade();
        //        workfacade.Redirect(sn, UserId);
        //    } catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        
        //public List<WorklistItem> GetWorkflowTasks()
        //{


        //    try
        //    {
        //        WorkflowFacade workfacade = new WorkflowFacade();
        //        return workfacade.GetWorklistItems();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        /*public List<ApproverHistory> GetApproverHistory(string userid)
        {
            try
            {
                SqlParameter[] sqlp = {
                    new SqlParameter("UserID", userid)};
                return Db.Database.SqlQuery<ApproverHistory>("exec [ApproverHistory] @UserID", sqlp).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/
        #endregion
    }

    /// <summary>
    /// Most Linque Mathod used for distincted by mention Column.
    /// </summary>
    public static class test
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }

        }
    }
}