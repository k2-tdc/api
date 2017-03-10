using aZaaS.Compact.Framework.Workflow;
using HKTDC.WebAPI.Common.Controllers;
using HKTDC.WebAPI.Common.DBContext;
using HKTDC.WebAPI.Common.Exceptions;
using HKTDC.WebAPI.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace HKTDC.WebAPI.Common.Services
{
    public class CommonService : BaseService
    {
        private EntityContext Db = new EntityContext();

        public Menus GetMenuItem(string UserId, string ProcessName, string page = null)
        {
            List<MenuList> MList = new List<MenuList>();
            List<Menus> FullMenu = new List<Menus>();

            Menus returnMenu = new Menus();

            try
            {
                SqlParameter[] sqlp = {
                     new SqlParameter ("ProcessName",ProcessName.ToUpper()),
                     new SqlParameter("EmployeeId",UserId),
                     new SqlParameter("Page", DBNull.Value)};

                if(!String.IsNullOrEmpty(page))
                {
                    sqlp[2].Value = page;
                }

                MList = Db.Database.SqlQuery<MenuList>("exec [K2_GetMenu] @ProcessName,@EmployeeId,@Page", sqlp).ToList();

                foreach (var FirstlevelMenu in MList.DistinctBy(P => P.MMenu))
                {
                    if (!string.IsNullOrEmpty(FirstlevelMenu.MMenu))
                    {
                        Menus menus = new Menus();
                        menus.UserName = FirstlevelMenu.UserName;
                        menus.UserID = FirstlevelMenu.User_ID;
                        menus.Name = FirstlevelMenu.MMenu;
                        menus.EmployeeNo = FirstlevelMenu.EMPLOYEENO;
                        if(isAdminRole(FirstlevelMenu.RoleName))
                        {
                            menus.IsAdmin = true;
                        } else
                        {
                            menus.IsAdmin = false;
                        }
                        menus.RoleType = FirstlevelMenu.RoleName;

                        List<Submenu> submenulist = new List<Submenu>();
                        string Link2 = "";
                        foreach (string SecondLevelMenu in MList.Where(P => P.MMenu == FirstlevelMenu.MMenu).Select(P => P.SubMenu).Distinct())
                        {
                            if (!string.IsNullOrEmpty(SecondLevelMenu))
                            {
                                Submenu submenu = new Submenu();
                                submenu.Name = SecondLevelMenu;
                                if ((MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu).Select(P => P.DOrder).Distinct().FirstOrDefault().Length) == 2)
                                {
                                    Link2 = MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu).Select(P => P.Menulink).Distinct().FirstOrDefault();
                                    submenu.Scount = MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu).Select(P => P.Scount).Distinct().FirstOrDefault();
                                    if (!string.IsNullOrEmpty(Link2))
                                    {
                                        submenu.Mlink = Link2;
                                    }
                                    else
                                    {
                                        submenu.Mlink = "#";
                                    }
                                    submenu.MenuId = MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu).Select(P => P.MenuId).Distinct().FirstOrDefault();
                                }
                                List<SSubMenu> ssubmenulist = new List<SSubMenu>();
                                string Link3 = "";
                                foreach (string ThirdLevelMenu in MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu).Select(P => P.SSubMenu).Distinct())
                                {
                                    if (!string.IsNullOrEmpty(ThirdLevelMenu))
                                    {
                                        SSubMenu ssubmenu = new SSubMenu();
                                        ssubmenu.Name = ThirdLevelMenu;
                                        ssubmenulist.Add(ssubmenu);
                                        if ((MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu && P.SSubMenu == ThirdLevelMenu).Select(P => P.DOrder).Distinct().FirstOrDefault().Length) == 3)
                                        {
                                            Link3 = MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu && P.SSubMenu == ThirdLevelMenu).Select(P => P.Menulink).Distinct().FirstOrDefault();
                                            ssubmenu.Scount = MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu && P.SSubMenu == ThirdLevelMenu).Select(P => P.Scount).Distinct().FirstOrDefault();
                                            if (!string.IsNullOrEmpty(Link3))
                                            {
                                                ssubmenu.Mlink = Link3;  //If the 3rd Level link value is Not Null
                                            }
                                            else
                                            {
                                                ssubmenu.Mlink = "#";
                                            }
                                            ssubmenu.MenuId = MList.Where(P => P.MMenu == FirstlevelMenu.MMenu && P.SubMenu == SecondLevelMenu && P.SSubMenu == ThirdLevelMenu).Select(P => P.MenuId).Distinct().FirstOrDefault();
                                        }
                                    }
                                }
                                if (ssubmenulist.Count > 0)
                                    submenu.sumenu = ssubmenulist;
                                else
                                    submenu.sumenu = null;
                                if (string.IsNullOrEmpty(submenu.Mlink))
                                {
                                    submenu.Mlink = "#"; //If the 2nd Level link value is Null
                                }
                                submenulist.Add(submenu);
                            }
                        }
                        if (submenulist.Count > 0)
                            menus.Menu = submenulist;
                        else
                            menus.Menu = null;
                        returnMenu = menus;
                    }
                }

            }
            catch (Exception ex)
            {
                returnMenu = null;
                throw ex;
            }
            return returnMenu;
        }

        public ProcessList GetProcess(string ProcessName)
        {
            return Db.ProcessList.Where(p => p.ProcessName == ProcessName).FirstOrDefault();
        }

        public List<ProcessListDTO> GetProcessListForWorkerRule(string UserId, string ProcessName)
        {
            List<ProcessListDTO> listDTO = new List<ProcessListDTO>();
            try
            {
                SqlParameter[] sqlp = {
                     new SqlParameter ("UserId",UserId),
                     new SqlParameter("ProcessName",ProcessName.ToUpper())};

                listDTO = Db.Database.SqlQuery<ProcessListDTO>("exec [K2_GetProcessList] @UserId,@ProcessName", sqlp).ToList();
            }
            catch (Exception ex)
            {
                listDTO = null;
                throw ex;
            }
            return listDTO;
        }

        public List<ProcessListDTO> GetProcessList()
        {
            List<ProcessListDTO> listDTO = new List<ProcessListDTO>();
            try
            {
                listDTO = (from a in Db.ProcessList
                         select new ProcessListDTO
                         {
                             ProcessID = a.ProcessID,
                             ProcessName = a.ProcessName,
                             ProcessDisplayName = a.ProcessDisplayName,
                             Flag = a.Flag
                         }).ToList();
            }
            catch (Exception ex)
            {
                listDTO = null;
                throw ex;
            }
            return listDTO;
        }

        public List<ProcessStepListDTO> GetEmailProcessStepList(string process, string activityGroupType)
        {
            List <ProcessStepListDTO> listDTO = new List<ProcessStepListDTO>();
            try
            {
                SqlParameter[] sqlp = {
                     new SqlParameter ("process",DBNull.Value),
                     new SqlParameter("groupType",DBNull.Value)};

                if(!string.IsNullOrEmpty(process))
                {
                    sqlp[0].Value = process;
                }
                if(!string.IsNullOrEmpty(activityGroupType))
                {
                    sqlp[1].Value = activityGroupType;
                }

                listDTO = Db.Database.SqlQuery<ProcessStepListDTO>("exec [K2_GetStepGroup] @process,@groupType", sqlp).ToList();
            }
            catch (Exception ex)
            {
                listDTO = null;
                throw ex;
            }
            return listDTO;
        }

        public List<UserDTO> getAllUser(string UserId)
        {
            return Db.vUser.Where(p => p.UserID != UserId).Select(p => new UserDTO
            {
                UserID = p.UserID,
                EmployeeID = p.EmployeeID,
                FullName = p.FullName
            }).OrderBy(p => p.FullName).ToList();
        }

        /// <summary>
        /// To Get the Applicant Or Recommed by Details 
        /// </summary>
        /// <param name="RuleID">IT0005,IT0008</param>
        /// <param name="UserId">ApporverID</param>
        /// <param name="WorkId">UserID</param>
        ///  SP  : pProcessWorkerGet (Refer the pProcessWorkerGet SP From  Workflow DataBase )
        /// <returns>Applicant list</returns>
        public List<Applicant> GetAllEmployeeDetails(string RuleID, string WorkId, string UserId, string EstCost)
        {
            try
            {
                List<Applicant> employees = new List<Applicant>();
                SqlParameter[] sqlp = {
                     new SqlParameter ("RuleCode",RuleID.Replace(",", ";") ),
                     new SqlParameter("UserID",DBNull.Value),
                    new SqlParameter("WorkerID", DBNull.Value),
                    new SqlParameter("EstCost", DBNull.Value)};
                if (!string.IsNullOrEmpty(UserId))
                {
                    sqlp[1].Value = UserId;
                }
                if (!string.IsNullOrEmpty(WorkId))
                {
                    sqlp[2].Value = WorkId;
                }
                if (!string.IsNullOrEmpty(EstCost))
                {
                    sqlp[3].Value = EstCost;
                }
                employees = Db.Database.SqlQuery<Applicant>("exec [K2_pProcessWorkerGet] @RuleCode,@UserID,@WorkerID,@EstCost", sqlp).ToList(); // Refer the pProcessWorkerGet SP From  Workflow DataBase
                if (!string.IsNullOrEmpty(EstCost))
                {
                    return employees.DistinctBy(p => p.WorkerId).ToList();
                }
                else
                {
                    return employees;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Used to return  Selected Applican Details
        /// VIEW : VW_EMPLOYEE
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>Retun format ApplicantDetails</returns>
        public ApplicantDetails GetApplicantDetails(string UserId)
        {
            ApplicantDetails applicant = new ApplicantDetails();
            try
            {
                applicant = Db.vUser.ToList().Where(X => X.UserID == UserId).Select(x => new ApplicantDetails
                {
                    Depart = x.DEPT,
                    Office = x.LocationCode,
                    Title = x.ROLE
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                applicant = null;
                throw ex;

            }
            return applicant;
        }

        /// <summary>
        /// SP : K2_GetNextRefId
        /// </summary>
        /// <returns></returns>
        public Reference GetReferenceId(string UserId)
        {
            SqlParameter[] sqlp = { new SqlParameter("UserId", UserId) };
            return Db.Database.SqlQuery<Reference>("exec [K2_GetNextRefId] @UserId", sqlp).FirstOrDefault();
        }

        public List<CommonSettingsDTO> GetAttachmentSetting(string process)
        {
            List<CommonSettingsDTO> listDTO = new List<CommonSettingsDTO>();
            try
            {
                SqlParameter[] sqlp = {
                     new SqlParameter ("Process",process)};

                listDTO = Db.Database.SqlQuery<CommonSettingsDTO>("exec [K2_GetAttachmentConfig] @Process", sqlp).ToList();
            }
            catch (Exception ex)
            {
                listDTO = null;
                throw ex;
            }
            return listDTO;
        }

        /// <summary>
        /// To Upload the Attached Files from New Request and Edit the Draft form to given UNC Path
        /// </summary>
        /// <param name="refid">Reference ID</param>
        /// <param name="httpRequest">File Stream</param>
        /// <param name="allfilenames">FileNames</param>
        /// SP : K2_EditAttachment
        /// <returns>Return True : Got Success else : Failed form SP</returns>
        public string UploadFiles(HttpRequest httpRequest, string userid, string refid = null, string process = null, string AttachmentType = null)
        {
            //string rid = httpRequest.Form.Get("refid").ToString();
            //string process = httpRequest.Form.Get("process").ToString();
            string rid = refid;
            var Files = httpRequest.Files;

            string result = "";
            List<ProcessRequestFormAttachment> AttachList = new List<ProcessRequestFormAttachment>();
            //#region handleexistionfile
            try
            {
                #region old code
                /*if (string.IsNullOrEmpty(allfilenames))
                {
                    int formid = Db.RequestFormMasters.Where(P => P.ReferenceID == rid).Select(P => P.FormID).FirstOrDefault();
                    if (formid > 0)
                    {
                        Db.RequestFormAttachments.RemoveRange(Db.RequestFormAttachments.Where(P => P.FormID == formid));
                        Db.SaveChanges();
                        string Dircur = ConfigurationManager.AppSettings.Get("UNCPath") + formid.ToString(); //UNC Path From Web Config
                        if (Directory.Exists(Dircur))
                            System.IO.Directory.Delete(Dircur, true);
                        result = "SUCCESS";
                    }
                    else
                    {
                        result = "Failed";
                    }
                }
                else
                {
                    SqlParameter[] sqlp = {
                     new SqlParameter ("ReferId",refid ),
                     new SqlParameter("FileName",allfilenames.Replace(',',':')) };
                    AttachList = Db.Database.SqlQuery<RequestFormAttachment>("exec K2_EditAttachment @ReferId,@FileName", sqlp).ToList();
                    // ErrorLog(null, "sp list count"+ AttachList.Count);
                    if (AttachList.Count > 0)
                    {

                        foreach (var file in AttachList.Distinct())
                        {
                            string Dircur = ConfigurationManager.AppSettings.Get("UNCPath") + file.FormID.ToString(); //UNC Path From Web Config
                            var postedFile = file.FileName;
                            if (Directory.Exists(Dircur))
                            {
                                var filePath = Dircur + "/" + file.AttachmentGUID.ToString() + "." + postedFile.Substring(postedFile.LastIndexOf('.') + 1);
                                File.Delete(filePath);
                            }

                            // NOTE: To delete in memory use postedFile.InputStream
                        }
                    }
                    #endregion
                    if (httpRequest.Files.Count > 0)
                    {
                        //ErrorLog(null, "File list count " + httpRequest.Files.Count);

                        AttachList = Db.RequestFormAttachments.Where(p => p.FormID == Db.RequestFormMasters.Where(P => P.ReferenceID == rid).Select(P => P.FormID).FirstOrDefault()).ToList();
                        //ErrorLog(null, "DbFileList Count " + AttachList.Count);
                        if (AttachList.Count > 0)
                        {

                            foreach (string file in httpRequest.Files)
                            {

                                string Dircur = ConfigurationManager.AppSettings.Get("UNCPath") + AttachList.Select(P => P.FormID).FirstOrDefault().ToString(); //UNC Path From Web Config
                                var postedFile = httpRequest.Files[file];
                                //ErrorLog(null, "Saving File " + postedFile.FileName);
                                if (!Directory.Exists(Dircur))
                                    System.IO.Directory.CreateDirectory(Dircur);
                                var filePath = Dircur + "/" + AttachList.Where(P => P.FileName == new FileInfo(postedFile.FileName).Name).Select(P => P.AttachmentGUID).FirstOrDefault().ToString() + new FileInfo(postedFile.FileName).Extension;
                                postedFile.SaveAs(filePath);
                                // ErrorLog(null, "File Saved success  " + postedFile.FileName);
                                // NOTE: To store in memory use postedFile.InputStream
                            }
                            result = "SUCCESS";
                        }
                        else
                        {
                            result = "Failed";
                        }
                    }

                }*/
                #endregion
                if (httpRequest.Files.Count > 0)
                {
                    SqlParameter[] sqlp = {
                     new SqlParameter ("process",process),
                     new SqlParameter("refid",rid)};

                    int processID = this.getProcessId(process);
                    int formID = Db.Database.SqlQuery<int>("exec [K2_GetFormID] @process,@refid", sqlp).FirstOrDefault();
                    foreach (string file in httpRequest.Files)
                    {
                        string Dircur = ConfigurationManager.AppSettings.Get("DefaultUNCPath");
                        if (!Directory.Exists(Dircur))
                            System.IO.Directory.CreateDirectory(Dircur);

                        string UNCPath = Dircur + process.ToUpper();
                        Dircur = UNCPath + '/' + formID.ToString();
                        var postedFile = httpRequest.Files[file];
                        if (!Directory.Exists(Dircur))
                            System.IO.Directory.CreateDirectory(Dircur);

                        if (!string.IsNullOrEmpty(AttachmentType))
                        {
                            Dircur = Dircur + '/' + AttachmentType;
                            if (!Directory.Exists(Dircur))
                                System.IO.Directory.CreateDirectory(Dircur);
                        }

                        Guid AttachmentGUID = Guid.NewGuid();

                        var userInfo = Db.vUser.Where(p => p.UserID == userid).FirstOrDefault();

                        ProcessRequestFormAttachment newAttachment = new ProcessRequestFormAttachment();
                        FileInfo fileInfo = new FileInfo(postedFile.FileName);
                        newAttachment.ProcessID = processID;
                        newAttachment.UNCPath = UNCPath;
                        newAttachment.FileName = fileInfo.Name;
                        newAttachment.FormID = formID;
                        newAttachment.AttachmentGUID = AttachmentGUID;
                        newAttachment.UploadedDate = DateTime.Now;
                        newAttachment.UploadedByUserID = userInfo.UserID;
                        newAttachment.UploadedByEmployeeID = userInfo.EmployeeID;
                        newAttachment.UploadedByFullName = userInfo.FullName;
                        newAttachment.UploadedByDeptName = userInfo.DEPT;
                        newAttachment.FileType = fileInfo.Extension;
                        newAttachment.AttachmentType = AttachmentType;
                        //newAttachment.FileSize = Convert.ToDecimal(fileInfo.Length);
                        Db.ProcessRequestFormAttachment.Add(newAttachment);
                        Db.SaveChanges();

                        var filePath = Dircur + "/" + AttachmentGUID.ToString() + fileInfo.Extension;
                        postedFile.SaveAs(filePath);

                        var attachmentRecord = Db.ProcessRequestFormAttachment.Where(p => p.AttachmentGUID == AttachmentGUID && p.FormID == formID).FirstOrDefault();
                        if(attachmentRecord != null)
                        {
                            FileInfo newInfo = new FileInfo(@filePath);
                            attachmentRecord.FileSize = newInfo.Length;
                            Db.SaveChanges();
                        }
                    }
                    result = "SUCCESS";
                }
            }
            catch (Exception ex)
            {
                result = "Failed";
                throw ex;
            }

            return result;
        }

        public ProcessRequestFormAttachment GetAttachment(string process, Guid AttachmentGuid, string UserId)
        {
            int processId = this.getProcessId(process);
            var attachmentRecord = Db.ProcessRequestFormAttachment.Where(p => p.AttachmentGUID == AttachmentGuid && p.ProcessID == processId).FirstOrDefault();
            string employeeId = Db.vUser.Where(p => p.UserID == UserId).Select(p => p.EmployeeID).FirstOrDefault();
            SqlParameter[] sqlp = {
                     new SqlParameter ("UserId", UserId),
                     new SqlParameter("EmployeeId", DBNull.Value),
                     new SqlParameter("FormId", DBNull.Value),
                     new SqlParameter("Type", "1"),
                     new SqlParameter("Process", process)
                };
            if (attachmentRecord != null)
            {
                sqlp[2].Value = attachmentRecord.FormID;
            }
            if (!String.IsNullOrEmpty(employeeId))
            {
                sqlp[1].Value = employeeId;
            }
            int permit = Db.Database.SqlQuery<int>("exec [K2_CheckUserPermission] @UserId,@EmployeeId,@FormId,@Type,@Process", sqlp).FirstOrDefault();

            if (permit > 0)
            {
                return attachmentRecord;
            }
            else
            {
                return null;
            }
        }

        public void DeleteFile(string GUID, string process)
        {
            try
            {
                int processID = this.getProcessId(process);
                var fileRecord = Db.ProcessRequestFormAttachment.Where(p => p.AttachmentGUID == new Guid(GUID) && p.ProcessID == processID).FirstOrDefault();
                if (fileRecord != null)
                {
                    string Dircur = fileRecord.UNCPath + "/" + fileRecord.FormID.ToString() + (!string.IsNullOrEmpty(fileRecord.AttachmentType) ? ("/" + fileRecord.AttachmentType) : ""); //UNC Path From Web Config
                    foreach (string s in System.IO.Directory.GetFiles(Dircur, fileRecord.AttachmentGUID + ".*"))
                    {
                        System.IO.File.Delete(s);
                    }
                    Db.ProcessRequestFormAttachment.Remove(fileRecord);
                    Db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected int getProcessId(string process)
        {
            return Db.ProcessList.Where(p => p.ProcessName == process).Select(p => p.ProcessID).FirstOrDefault();
        }

        public List<ChkFrmStatus> GetWorklist(string ReferID, string CStat, string FDate, string TDate, string UserId, string ProsIncId, int offset = 0, int limit = 999999, string sort = null, string process = null)
        {
            string[] availableProcess = ConfigurationManager.AppSettings.Get("AvailableProcess").ToString().Split(';');

            List<ChkFrmStatus> FormRequests = new List<ChkFrmStatus>();
            WorkflowFacade workfacade = new WorkflowFacade();
            List<WorklistItem> worklist = new List<WorklistItem>();
            List<WorklistItem> sharedworklist = new List<WorklistItem>();
            try { 
                if (!string.IsNullOrEmpty(process))
                {
                    if (availableProcess.Contains(process))
                    {
                        string processName = ConfigurationManager.AppSettings.Get(process + "ProcessName").ToString();
                        workfacade.setWFProcess(processName);
                        worklist = workfacade.GetWorklistItemsByProcess(UserId);
                        int processId = this.getProcessId(process);
                        var requestList = this.getProcessWorklist(worklist, processId, process, ReferID, CStat, FDate, TDate, UserId, ProsIncId, offset, limit, sort);
                        if(requestList!=null)
                        {
                            FormRequests.AddRange(requestList);
                        }
                    } else
                    {
                        FormRequests = null;
                    }
                } else
                {
                    foreach(var proc in availableProcess)
                    {
                        string processName = ConfigurationManager.AppSettings.Get(proc + "ProcessName").ToString();
                        workfacade.setWFProcess(processName);
                        worklist = workfacade.GetWorklistItemsByProcess(UserId);
                        int processId = this.getProcessId(proc);
                        var requestList = this.getProcessWorklist(worklist, processId, proc, ReferID, CStat, FDate, TDate, UserId, ProsIncId, offset, limit, sort);
                        if (requestList != null)
                        {
                            FormRequests.AddRange(requestList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FormRequests = null;
                throw ex;
            }
            
            return FormRequests;
        }

        protected List<ChkFrmStatus> getProcessWorklist(List<WorklistItem> worklist, int processId, string process, string ReferID, string CStat, string FDate, string TDate, string UserId, string ProsIncId, int offset, int limit, string sort)
        {
            string proinsid = "";
            List<CheckStatus> StatusList = new List<CheckStatus>();
            List<ChkFrmStatus> FormRequests = new List<ChkFrmStatus>();
            if (worklist.Count() > 0)
            {
                proinsid = String.Join(",", worklist.Select(P => P.ProcInstID)).TrimEnd(',');
            }

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
                        new SqlParameter("Process", process)};

                    if (!string.IsNullOrEmpty(ReferID))
                        sqlp[0].Value = ReferID;
                    if (!string.IsNullOrEmpty(CStat))
                        sqlp[1].Value = CStat;
                    if (!string.IsNullOrEmpty(FDate))
                        sqlp[2].Value = FDate;
                    if (!string.IsNullOrEmpty(TDate))
                        sqlp[3].Value = TDate;
                    if (!string.IsNullOrEmpty(UserId))
                        sqlp[5].Value = UserId;

                    if (string.IsNullOrEmpty(ProsIncId))
                    {
                        if (worklist.Count > 0)
                            sqlp[7].Value = String.Join(",", worklist.Select(P => P.ProcInstID)).TrimEnd(',');
                    }
                    else
                    {
                        sqlp[7].Value = ProsIncId;
                    }

                    StatusList = Db.Database.SqlQuery<CheckStatus>("exec [K2_GetProcessWorkList] @ReferID,@CStatus,@FDate,@TDate,@SUser,@UserId,@TOwner,@ProcIncId,@offset,@limit,@sort,@Process", sqlp).ToList();

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
                                status.ProcessId = processId;
                                status.ProcInstID = request.ProcInstID;
                                status.ReferenceID = request.ReferenceID;
                                status.FormStatus = request.FormStatus;
                                status.SubmittedOn = request.SubmittedOn;
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

                                //if (!proinsid.Split(',').Contains(request.ProcInstID))
                                //{
                                //    status.ProcessUrl = worklist.Where(p => p.ProcInstID == procid).Select(P => P.Data).FirstOrDefault() + "&SharedBy=" + SUser;
                                //    status.Type = "Sharing";
                                //}
                                //else
                                //status.ProcessUrl = worklist.Where(p => p.ProcInstID == procid).Select(P => P.Data).FirstOrDefault();
                                //}
                                status.ProcessUrl = worklist.Where(p => p.ProcInstID == procid).Select(P => P.Data).FirstOrDefault();
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

        public Tuple<bool, string> GetWorklistCount(string UserId, string process = null)
        {
            bool haveResult = false;
            string returnStr = "";
            string returnStrHead = "{\"Data\":[{";
            string returnStrBody = "";
            string returnStrEnd = "}]}";
            string[] availableProcess = ConfigurationManager.AppSettings.Get("AvailableProcess").ToString().Split(';');

            List<ChkFrmStatus> FormRequests = new List<ChkFrmStatus>();
            WorkflowFacade workfacade = new WorkflowFacade();
            List<WorklistItem> worklist = new List<WorklistItem>();
            List<WorklistItem> sharedworklist = new List<WorklistItem>();
            try
            {
                if (!string.IsNullOrEmpty(process))
                {
                    process = process.ToUpper();
                    if (availableProcess.Contains(process))
                    {
                        string processName = ConfigurationManager.AppSettings.Get(process + "ProcessName").ToString();
                        workfacade.setWFProcess(processName);
                        worklist = workfacade.GetWorklistItemsByProcess(UserId);
                        int count = worklist.Count();

                        returnStrBody = returnStrBody + "\"" + process + "_count\":" + count.ToString();
                        haveResult = true;
                    }
                }
                else
                {
                    foreach (var proc in availableProcess)
                    {
                        string processName = ConfigurationManager.AppSettings.Get(proc + "ProcessName").ToString();
                        workfacade.setWFProcess(processName);
                        worklist = workfacade.GetWorklistItemsByProcess(UserId);
                        int count = worklist.Count();
                        
                        if (!string.IsNullOrEmpty(returnStrBody))
                        {
                            returnStrBody = returnStrBody + ",";
                        }

                        returnStrBody = returnStrBody + "\"" + proc + "_count\":" + count.ToString();
                        haveResult = true;
                    }
                }

                returnStr = returnStrHead + returnStrBody + returnStrEnd;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Tuple.Create(haveResult, returnStr);
        }

        public List<Dept> getDeptList()
        {
            List<Dept> deptList = new List<Dept>();
            deptList = Db.vDepartment.Select(p => new Dept
            {
                DeptCode = p.CODE,
                DeptName = p.DESCRIPTION
            }).OrderBy(p => p.DeptName).ToList();
            return deptList;
        }
    }
}