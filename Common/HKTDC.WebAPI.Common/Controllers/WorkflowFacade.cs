using aZaaS.Compact.Framework;
using aZaaS.Compact.Framework.Workflow;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HKTDC.WebAPI.Common.Controllers
{
    public class WorkflowFacade
    {
        private ServiceContext context = new ServiceContext();
        private WFClientService wfclient;

        private string ProcessFullName;
        private string ExecuteActionColumn = ConfigurationManager.AppSettings["ExecuteActionColumn"].ToString();
        private string domain = ConfigurationManager.AppSettings["WindowDomain"].ToString();
        public WorkflowFacade()
        {
            ServiceContext.Initialize();
            wfclient = new WFClientService(AuthenticationType.Windows);
        }

        public void setWFProcess(string ProcessName)
        {
            this.ProcessFullName = ProcessName;
        }

        public void MyTestInitialize()
        {

            this.context = new ServiceContext();

        }

        /// <summary>
        ///A test for StartProcessInstance
        ///</summary>

        public int StartProcessInstance(int FormId, string UserId, string RefId, string Remark)
        {
            string folio = Utility.Generate("Folio", FormId, RefId);
            Dictionary<string, object> datafields = new Dictionary<string, object>();
            datafields["FormID"] = FormId;
            datafields["Remark"] = Remark;
            int procInstId = 0;
            //-C01:Start a normal process. 
            bool success = false;
            try
            {

                this.context.UserName = domain + "\\" + UserId;
                procInstId = this.wfclient.StartProcessInstance(this.context, ProcessFullName.Split(';').FirstOrDefault(), folio, datafields);
                //use this id to update RequestFormMaster set ProcInsID=procInstId where FormID=56
                success = true;

            }
            catch (Exception ex)
            {
                success = false;
                throw ex;
            }
            return procInstId;
            //Assert.IsTrue(success);

        }


        public string GetFormIDDataFieldValue(int p_ProcInstId)
        {
            try
            {
                string dfValue = this.wfclient.GetProcInstDataFieldValue(this.context, p_ProcInstId, "FormID").ToString();
                return dfValue;
            }
            catch (Exception ex)
            {
                throw ex;

            }



        }

        public string GetDataFieldValue(int p_ProcInstId, string p_DataFieldName)
        {

            try
            {
                string dfValue = this.wfclient.GetProcInstDataFieldValue(this.context, p_ProcInstId, p_DataFieldName).ToString();
                return dfValue;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }



        public void ExecuteAction(string UserId, string SN, string ActionName, string comment)
        {
            try
            {
                this.context.UserName = domain + "\\" + UserId;
                string sn = SN;
                string actionName = ActionName;
                List<DataField> activityDFs = new List<DataField>();
                DataField df1 = new DataField();
                df1.Name = ExecuteActionColumn.Split(';').FirstOrDefault();
                df1.Value = comment;
                activityDFs.Add(df1);
                DataField df2 = new DataField();
                df2.Name = ExecuteActionColumn.Split(';').LastOrDefault();
                df2.Value = DateTime.Now;
                activityDFs.Add(df2);

                this.wfclient.ExecuteAction(this.context, sn, "", new List<DataField>(), activityDFs, actionName, string.Empty, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExecuteActionDelegation()
        {
            try
            {
                this.context.UserName = @"HK\cccheung";
                string delegationUser = @"HK\brlin";
                string sn = "76_34";
                string actionName = "Approve";
                List<DataField> activityDFs = new List<DataField>();
                DataField df1 = new DataField();
                df1.Name = "Task_Comment";
                df1.Value = "test";
                activityDFs.Add(df1);
                DataField df2 = new DataField();
                df2.Name = "Task_CommentedOn";
                df2.Value = DateTime.Now;
                activityDFs.Add(df2);

                this.wfclient.ExecuteAction(this.context, sn, delegationUser, new List<DataField>(), activityDFs, actionName, string.Empty, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<WorklistItem> GetWorklistItems()
        {
            try
            {
                List<WorklistItem> worklist = new List<WorklistItem>();
                string[] processNames = ProcessFullName.Split(';').ToArray();
                foreach (string processName in processNames)
                {
                    List<WorklistItem> items = this.wfclient.GetWorklistItemsByProcess(this.context, PlatformType.ASP, processName);
                    if (items != null && items.Count > 0)
                        worklist.AddRange(items.ToList());

                }
                return worklist;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<WorklistItem> GetWorklistItemsByProcess(string UserId)
        {
            try
            {
                this.context.UserName = domain + "\\" + UserId;//@"HK\cccheung"mayik;
                List<WorklistItem> worklist = new List<WorklistItem>();
                string[] processNames = ProcessFullName.Split(';').ToArray();
                foreach (string processName in processNames)
                {
                    List<WorklistItem> items = this.wfclient.GetWorklistItemsByProcess(this.context, PlatformType.ASP, processName);
                    if (items != null && items.Count > 0)
                        worklist.AddRange(items.Where<WorklistItem>(item => item.ActivityName.Contains("Recall").Equals(false)).ToList());
                    //foreach (WorklistItem item in items)
                    //{
                    //    int procInstId = item.ProcInstID;//use this id to get request form detail.
                    // Actions actions = item.Actions;
                    //    string strItemUrl = item.Data;//URL to open work list item. like https://workflowuat.tdc.org.hk/home/request.html?formtype=review&SN=73_34
                    //    string sn = item.SN;//format: ProcInstID_ActivityDestInstID

                    //}
                }
                return worklist;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<WorklistItem> GeWorklistItemsByProcessAction(string UserId, string ActionStatus)
        {
            try
            {
                this.context.UserName = domain + "\\" + UserId;//@"HK\cccheung"mayik;
                List<WorklistItem> worklist = new List<WorklistItem>();
                string[] processNames = ProcessFullName.Split(';').ToArray();
                foreach (string processName in processNames)
                {
                    List<WorklistItem> items = this.wfclient.GetWorklistItemsByProcess(this.context, PlatformType.ASP, processName, ActionStatus);
                    if (items != null && items.Count > 0)
                        worklist.AddRange(items.Where<WorklistItem>(item => item.ActivityName.Contains("Recall").Equals(false)).ToList());
                    //foreach (WorklistItem item in items)
                    //{
                    //    int procInstId = item.ProcInstID;//use this id to get request form detail.
                    // Actions actions = item.Actions;
                    //    string strItemUrl = item.Data;//URL to open work list item. like https://workflowuat.tdc.org.hk/home/request.html?formtype=review&SN=73_34
                    //    string sn = item.SN;//format: ProcInstID_ActivityDestInstID

                    //}
                }
                return worklist;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public WorklistItem GetWorklistItemsWithRecallByProcess(string UserId, string ProcInstID, string ActionStatus)
        {
            try
            {

                this.context.UserName = domain + "\\" + UserId;//@"HK\cccheung"mayik;
                WorklistItem worklist = new WorklistItem();
                string[] processNames = ProcessFullName.Split(';').ToArray();
                int proinsid = Convert.ToInt32(ProcInstID);
                foreach (string processName in processNames)
                {
                    List<WorklistItem> items = this.wfclient.GetWorklistItemsByProcess(this.context, PlatformType.ASP, processName, ActionStatus);
                    if (items != null && items.Count > 0)
                        worklist = items.Where(P => P.ProcInstID == proinsid).FirstOrDefault();
                }
                return worklist;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void OpenWorklistItemOnErrorStatus()
        {
            string SN = "5094_50";
            this.context.UserName = @"HK\CCCHEUNG";
            WorklistItem wlItem = this.wfclient.OpenWorklistItem(this.context, SN);

            // Assert.IsNotNull(wlItem);
        }

        //[TestMethod]
        //public void GetProcessInstanceOnErrorStatusTest()
        //{
        //    int procInstID = 5094;
        //    Workflow.ProcessInstance inst = wfclient.GetProcessInstance(this.context, procInstID);

        //    Assert.IsNotNull(inst);
        //}

        public void Redirect(string sn, string UserID)
        {
            try
            {
                this.wfclient.Redirect(sn, UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
