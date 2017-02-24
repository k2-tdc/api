using HKTDC.WebAPI.CHSW.Models;
using HKTDC.WebAPI.CHSW.Services;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HKTDC.WebAPI.CHSW.Controllers
{
    public class CheckStatusController : BaseController
    {
        private CheckStatusService checkStatusService;
        private HistoryService historyService;
        private ReportService reportService;

        public CheckStatusController()
        {
            this.checkStatusService = new CheckStatusService();
            this.historyService = new HistoryService();
            this.reportService = new ReportService();
        }

        // Get Three Level Services
        [Route("workflow/applications/computer-app/service-types")]
        [HttpGet]
        public List<ServiceLevel1> GetServiceType()
        {
            try
            {
                return this.checkStatusService.GetAllServiceTypes();
            }

            catch (Exception ex)
            {
                var err = this.checkStatusService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));

            }
        }

        [Route("workflow/applications/computer-app/service-providers")]
        [HttpGet]
        public List<VWEmployeeDTO> GetForwardEmployee(string applicant)
        {
            try
            {
                return this.checkStatusService.GetForwardEmployee(applicant);
            }
            catch (Exception ex)
            {
                var err = this.checkStatusService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/applications/computer-app/approval-history")]
        [HttpGet]
        public List<ChkFrmStatus> getHistoryList(string userid, string employeeid, string applicant = null, [FromUri(Name = "approval-start-date")] string approvalStartDate = null, [FromUri(Name = "approval-end-date")] string approvalEndDate = null, string status = null, string refid = null, [FromUri(Name = "create-start-date")] string createStartDate = null, [FromUri(Name = "create-end-date")] string createEndDate = null, string keyword = null)
        {
            return this.historyService.getApprovalList(userid, employeeid, applicant, approvalStartDate, approvalEndDate, status, refid, createStartDate, createEndDate, keyword);
        }

        [Route("workflow/applications/computer-app/usage-report")]
        [HttpGet]
        public HttpResponseMessage exportExcel(string refid = null, string department = null, string applicant = null, [FromUri(Name = "create-date-start")] string createdatestart = null, [FromUri(Name = "create-date-end")] string createdateend = null, [FromUri(Name = "completion-date-start")] string completiondatestart = null, [FromUri(Name = "completion-date-end")] string completiondateend = null, string keyword = null, string sort = null)
        {
            try
            {
                Tuple<byte[], string> res = this.generateExcel(refid, department, applicant, createdatestart, createdateend, completiondatestart, completiondateend, keyword, sort);

                var result = new HttpResponseMessage(HttpStatusCode.OK);
                Stream stream = new MemoryStream(res.Item1);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = res.Item2
                };

                return result;
            }
            catch (Exception ex)
            {
                var err = this.reportService.ErrorLog(ex, null);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        //Get Details for Check Status Based on the Search Options
        [Route("workflow/users/{UserId}/applications/computer-app")]
        [HttpGet]
        public List<ChkFrmStatus> GetRequestList(string UserId, string EmployeeId, int offset = 0, int limit = 999999, string sort = null, string refid = null, string status = null, [FromUri(Name = "start-date")] string FDate = null, [FromUri(Name = "end-date")] string TDate = null, string applicant = null)
        {
            try
            {
                if (compareUser(Request, UserId))
                {
                    string sqlSortValue = "";
                    if (!String.IsNullOrEmpty(sort))
                    {
                        string[] tmp = sort.Split(',');
                        List<string> tmpArr = new List<string>();
                        foreach (var i in tmp)
                        {
                            tmpArr.Add(Utility.changeSqlCodeDraftList(i));
                        }
                        sqlSortValue = String.Join(",", tmpArr.ToArray());
                    }
                    return this.checkStatusService.GetRequestList(refid, status, FDate, TDate, applicant, UserId, "CheckStatus", EmployeeId, offset, limit, sqlSortValue);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.checkStatusService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        //Get Details for Review
        [Route("workflow/applications/computer-app/{ReferID}")]
        [HttpGet]
        public List<Review> GetRequestDetails(string ReferID, string UserId, string ProInstID)
        {
            try
            {
                if (compareUser(Request, UserId))
                {
                    return this.checkStatusService.GetRequestDetails(ReferID, UserId, ProInstID, "Review");
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.checkStatusService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        protected Tuple<byte[], string> generateExcel(string RefId, string Dept, string Applicant, string CreateDateStart, string CreateDateEnd, string CompletionDateStart, string CompletionDateEnd, string Keyword, string sort)
        {
            try
            {
                using (var reportViewer = new ReportViewer())
                {
                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    reportViewer.ProcessingMode = ProcessingMode.Remote;
                    IReportServerCredentials irsc = new CustomReportCredentials(ConfigurationManager.AppSettings.Get("SSRSLoginUser").ToString(), ConfigurationManager.AppSettings.Get("SSRSLoginPassword").ToString());
                    reportViewer.ServerReport.ReportServerCredentials = irsc;
                    reportViewer.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings.Get("SSRSPath").ToString());
                    reportViewer.ServerReport.ReportPath = ConfigurationManager.AppSettings.Get("SSTSFolderPath").ToString();

                    reportViewer.ServerReport.SetParameters(new ReportParameter("RefId", RefId));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("Dept", Dept));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("Applicant", Applicant));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("CreateDateStart", CreateDateStart));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("CreateDateEnd", CreateDateEnd));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("CompletionDateStart", CompletionDateStart));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("CompletionDateEnd", CompletionDateEnd));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("Keyword", Keyword));

                    if (!String.IsNullOrEmpty(sort))
                    {
                        string[] tmp = sort.Split(',');
                        List<string> tmpArr = new List<string>();
                        foreach (var i in tmp)
                        {
                            tmpArr.Add(changeSqlCode(i));
                        }
                        var sqlValue = String.Join(",", tmpArr.ToArray());
                        if (sqlValue != "")
                        {
                            reportViewer.ServerReport.SetParameters(new ReportParameter("Sort", sqlValue));
                        }
                    }

                    byte[] resultByte = reportViewer.ServerReport.Render("Excel", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                    //string saveExcel = ConfigurationManager.AppSettings.Get("SSRSSaveExcel").ToString();
                    //if (saveExcel == "1")
                    //{
                    //    using (FileStream fs = new FileStream(@ConfigurationManager.AppSettings.Get("SSRSSavePath").ToString() + fileName, FileMode.Create))
                    //    {
                    //        fs.Write(resultByte, 0, resultByte.Length);
                    //    }
                    //}

                    return Tuple.Create(resultByte, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected string changeSqlCode(string pro)
        {
            string returnValue = "";
            string order = "";
            if (pro[0] == '+')
            {
                order = " desc";
            }
            pro = pro.Substring(1);
            switch (pro)
            {
                case "dept": returnValue = "a.ApplicantDeptName"; break;
                case "applicant": returnValue = "a.ApplicantFullName"; break;
                default: break;
            }
            return returnValue + order;
        }
    }
}
