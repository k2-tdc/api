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
    public class WorklistActionService : BaseService
    {
        private EntityContext Db = new EntityContext();

        public int ResendEmail(string UserId, int FormID)
        {
            // return 0 - success, 1 - not applicant of form / not approval status, 2 - no form, 3 - exception
            int returnStatus = 0;

            try
            {
                // Check applicant or Approval status
                var form = Db.RequestFormMasters.Where(p => p.ApplicantUserID == UserId && p.FormStatus == "Approval" && p.FormID == FormID).FirstOrDefault();
                if (form != null)
                {
                    SqlParameter[] sqlp = { new SqlParameter("FormID", FormID) };
                    ResendDetail detailResult;
                    detailResult = Db.Database.SqlQuery<ResendDetail>("exec [K2_GetResendDetail] @FormID", sqlp).FirstOrDefault();
                    if (detailResult != null)
                    {
                        SqlParameter[] sqlp2 = {
                            new SqlParameter("isSubTask", detailResult.isSubTask),
                            new SqlParameter("SubTaskID", detailResult.SubTaskID),
                            new SqlParameter("ProcInstID", detailResult.ProcInstID),
                            new SqlParameter("ProcessName", detailResult.ProcessName),
                            new SqlParameter("ActInstID", detailResult.ActInstID),
                            new SqlParameter("ActivityName", detailResult.ActivityName),
                            new SqlParameter("ActionOwnerUserID", detailResult.ActionOwnerUserID),
                            new SqlParameter("ActionOwnerEmployeeID", detailResult.ActionOwnerEmployeeID),
                            new SqlParameter("FormID", detailResult.FormID)
                        };
                        Db.Database.ExecuteSqlCommand("exec [K2_CreateEmailNotificationTask] @isSubTask,@SubTaskID,@ProcInstID,@ProcessName,@ActInstID,@ActivityName,@ActionOwnerUserID,@ActionOwnerEmployeeID,@FormID", sqlp2);
                    }
                    else
                    {
                        returnStatus = 2;
                    }
                }
                else
                {
                    returnStatus = 1;
                }
            }
            catch (Exception ex)
            {
                returnStatus = 3;
                throw ex;
            }

            return returnStatus;
        }

        public string RecallAction(string UserId, string ProcInstID, string ActionName, string Comment)
        {
            string result = "";
            try
            {
                WorkflowFacade workfacade = new WorkflowFacade();
                WorklistItem worklist = workfacade.GetWorklistItemsWithRecallByProcess(UserId, ProcInstID, "Applicant Recall");
                if (worklist != null)
                {
                    workfacade.ExecuteAction(UserId, worklist.SN, ActionName, Comment);
                    result = "Success";
                }
                else
                {
                    result = "Failed";
                    throw new System.ArgumentException("Unable to recall the task");
                }

            }
            catch (Exception ex)
            {
                result = "Failed";
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// To Retun the Delegation List Based on the Type
        /// </summary>
        /// <param name="Type">Delegation or Sharing</param>
        /// <returns></returns>
        public string WorklistAction(string UserId, string SN, string ActionName, string Comment, string forwardUserID)
        {
            string result = "";
            try
            {
                if (!string.IsNullOrEmpty(forwardUserID))
                {
                    int underScorePos = SN.IndexOf("_");
                    string procInstID = SN.Substring(0, underScorePos);
                    var forwardUserInfo = Db.VW_EMPLOYEE.Where(p => p.UserID == forwardUserID).FirstOrDefault();
                    var actionTakerRecord = Db.RequestFormTaskActioners.Where(p => p.ProcInstID.ToString() == procInstID).FirstOrDefault();
                    if (!string.IsNullOrEmpty(actionTakerRecord.ActionTakerUserID))
                    {
                        actionTakerRecord.ActionTakerUserID = forwardUserInfo.UserID;
                        actionTakerRecord.ActionTakerEmployeeID = forwardUserInfo.EmployeeID;
                        actionTakerRecord.ActionTakerFullName = forwardUserInfo.FullName;
                        actionTakerRecord.ActionTakerDeptName = forwardUserInfo.DEPT;
                        Db.SaveChanges();
                    }
                }
                WorkflowFacade workfacade = new WorkflowFacade();
                workfacade.ExecuteAction(UserId, SN, ActionName, Comment);
                result = "Success";
            }
            catch (Exception ex)
            {
                result = "Failed";
                throw ex;
            }

            return result;
        }
    }
}