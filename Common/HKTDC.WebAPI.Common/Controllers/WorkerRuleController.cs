using HKTDC.WebAPI.Common.Models;
using HKTDC.WebAPI.Common.Services;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HKTDC.WebAPI.Common.Controllers
{
    public class WorkerRuleController : BaseController
    {
        private WorkerRuleService workerRuleService;

        public WorkerRuleController()
        {
            this.workerRuleService = new WorkerRuleService();
        }

        [Route("workflow/worker-rule/process")]
        [HttpGet]
        public List<ProcessListDTO> GetProcessList(string MenuId)
        {
            try
            {
               return this.workerRuleService.GetProcessList(getCurrentUser(Request), MenuId);
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule")]
        [HttpGet]
        public List<WorkerRuleDTO> GetWorkerRuleList(string process=null)
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleList(getCurrentUser(Request), process);
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule")]
        [HttpPost]
        public HttpResponseMessage SaveWorkerRule()
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool, string> response = this.workerRuleService.SaveWorkerRule(getCurrentUser(Request), stuff);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule")]
        [HttpPut]
        public HttpResponseMessage UpdateWorkerRule()
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool, string> response = this.workerRuleService.SaveWorkerRule(getCurrentUser(Request), stuff);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/{WorkerRuleId:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteWorkerRule(int WorkerRuleId)
        {
            try
            {
                Tuple<bool, string> response = this.workerRuleService.DeleteWorkerRule(getCurrentUser(Request), WorkerRuleId);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/{WorkerRuleId:int}")]
        [HttpGet]
        public WorkerRuleDetailDTO GetWorkerRuleDetail(int WorkerRuleId)
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleDetail(getCurrentUser(Request), WorkerRuleId);
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
        
        [Route("workflow/worker-rule/{WorkerRuleId:int}/rule")]
        [HttpGet]
        public List<WorkerRuleRuleListDTO> GetWorkerRuleRuleList(int WorkerRuleId, string UserId = null, string WorkerId = null)
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleRuleList(getCurrentUser(Request), WorkerRuleId, UserId, WorkerId);
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/nature")]
        [HttpGet]
        public List<WorkerRuleNatureDTO> GetWorkerRuleNature()
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleNature();
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/grade")]
        [HttpGet]
        public List<WorkerRuleGradingDTO> GetWorkerRuleGrade()
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleGrade();
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/priority/{Code}")]
        [HttpGet]
        public List<WorkerRulePriorityDTO> GetWorkerRulePriority(string Code)
        {
            try
            {
                return this.workerRuleService.GetWorkerRulePriority(Code);
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/department")]
        [HttpGet]
        public List<WorkerRuleDepartmentDTO> GetWorkerRuleDepartment()
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleDepartment();
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/criteria/{Code}")]
        [HttpGet]
        public List<WorkerRuleCriteriaDTO> GetWorkerRuleCriteria(string Code)
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleCriteria(Code);
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/level")]
        [HttpGet]
        public List<WorkerRuleOrgChartDTO> GetWorkerRuleOrgChart()
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleOrgChart();
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/user-group")]
        [HttpGet]
        public List<WorkerRuleUserGroupDTO> GetWorkerRuleUserGroup()
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleUserGroup();
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/team")]
        [HttpGet]
        public List<WorkerRuleTeamDTO> GetWorkerRuleTeam()
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleTeam();
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/team-filter")]
        [HttpGet]
        public List<WorkerRuleTeamFilterDTO> GetWorkerRuleTeamFilter()
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleTeamFilter();
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/rule")]
        [HttpPost]
        public HttpResponseMessage SaveWorkerRuleRule()
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool, string> response = this.workerRuleService.SaveWorkerRuleRule(getCurrentUser(Request), stuff);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/rule")]
        [HttpPut]
        public HttpResponseMessage UpdateWorkerRuleRule()
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool, string> response = this.workerRuleService.SaveWorkerRuleRule(getCurrentUser(Request), stuff);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/rule/{WorkerRuleSettingId:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteWorkerRuleRule(int WorkerRuleSettingId)
        {
            try
            {
                Tuple<bool, string> response = this.workerRuleService.DeleteWorkerRuleRule(getCurrentUser(Request), WorkerRuleSettingId);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/rule/{WorkerRuleSettingId:int}")]
        [HttpGet]
        public WorkerRuleRuleDetailDTO GetWorkerRuleRuleDetail(int WorkerRuleSettingId)
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleRuleDetail(getCurrentUser(Request), WorkerRuleSettingId);
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/rule/{Code}")]
        [HttpGet]
        public List<WorkerRuleRuleDTO> GetWorkerRuleRule(string Code)
        {
            try
            {
                return this.workerRuleService.GetWorkerRuleRule(Code);
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/worker-rule/preview")]
        [HttpPost]
        public HttpResponseMessage WorkerRulePreview()
        {
            try
            {
                bool havePermission = this.workerRuleService.checkHavePermission(getCurrentUser(Request), "ADMIN", "Worker Rule");
                if (havePermission)
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");

                    string json;
                    json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest); //throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);

                    Tuple<byte[], string> res = this.generateExcel(stuff);

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
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        protected Tuple<byte[], string> generateExcel(dynamic stuff)
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
                    string RuleCode, UserId1, UserId2, Team, Department, DateFrom, DateTo, Criteria;
                    int Rule, Score, Priority;
                    int? LevelNo, GroupID, GroupID1, Grade1, Grade2, TeamFilter, Grade3, Grade4;

                    RuleCode = stuff.RuleCode;
                    Rule = stuff.Rule;
                    Score = stuff.Score;
                    UserId1 = stuff.UserId1;
                    UserId2 = stuff.UserId2;
                    Team = stuff.Team;
                    Department = stuff.Department;
                    DateFrom = stuff.DateFrom;
                    DateTo = stuff.DateTo;
                    Priority = stuff.Priority;
                    Grade1 = this.TryParseNullable(stuff.Grade1.ToString());
                    Grade2 = this.TryParseNullable(stuff.Grade2.ToString());
                    Grade3 = this.TryParseNullable(stuff.Grade3.ToString());
                    Grade4 = this.TryParseNullable(stuff.Grade4.ToString());
                    LevelNo = this.TryParseNullable(stuff.LevelNo.ToString());
                    GroupID = this.TryParseNullable(stuff.GroupID.ToString());
                    GroupID1 = this.TryParseNullable(stuff.GroupID1.ToString());
                    TeamFilter = this.TryParseNullable(stuff.TeamFilter.ToString());
                    Criteria = "";

                    if (!string.IsNullOrEmpty(stuff.Criteria.ToString()))
                    {
                        foreach (var i in stuff.Criteria)
                        {
                            if (!string.IsNullOrEmpty(Criteria)) Criteria += ';';
                            Criteria += i;
                        }
                        if(!string.IsNullOrEmpty(Criteria))
                        {
                            Criteria += ";";
                        }
                    }

                    reportViewer.ProcessingMode = ProcessingMode.Remote;
                    IReportServerCredentials irsc = new CustomReportCredentials(ConfigurationManager.AppSettings.Get("SSRSLoginUser").ToString(), ConfigurationManager.AppSettings.Get("SSRSLoginPassword").ToString());
                    reportViewer.ServerReport.ReportServerCredentials = irsc;
                    reportViewer.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings.Get("SSRSPath").ToString());
                    reportViewer.ServerReport.ReportPath = ConfigurationManager.AppSettings.Get("SSTSFolderPath").ToString();

                    reportViewer.ServerReport.SetParameters(new ReportParameter("RuleCode", RuleCode));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("Template", Rule.ToString()));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("Score", Score.ToString()));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("UserEmployeeID", UserId2));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("UserMinGradeLevel", Grade3.ToString()));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("UserMaxGradeLevel", Grade4.ToString()));
                    if (GroupID1.GetValueOrDefault(0) != 0)
                    {
                        reportViewer.ServerReport.SetParameters(new ReportParameter("UserGroupID", GroupID1.ToString()));
                    }
                    reportViewer.ServerReport.SetParameters(new ReportParameter("UserDepartment", Department));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("UserTeam", Team));
                    if (TeamFilter.GetValueOrDefault(0) != 0)
                    {
                        reportViewer.ServerReport.SetParameters(new ReportParameter("UserTeamFilter", TeamFilter.ToString()));
                    }
                    reportViewer.ServerReport.SetParameters(new ReportParameter("isDefault", Priority.ToString()));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("WorkerEmployeeID", UserId1));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("WorkerMinGradeLevel", Grade1.ToString()));
                    reportViewer.ServerReport.SetParameters(new ReportParameter("WorkerMaxGradeLevel", Grade2.ToString()));
                    if (GroupID.GetValueOrDefault(0) != 0)
                    {
                        reportViewer.ServerReport.SetParameters(new ReportParameter("WorkerGroupID", GroupID.ToString()));
                    }
                    if (LevelNo.GetValueOrDefault(0) != 0)
                    {
                        reportViewer.ServerReport.SetParameters(new ReportParameter("WorkerOrgChartLevel", LevelNo.ToString()));
                    }
                    reportViewer.ServerReport.SetParameters(new ReportParameter("OtherCriteria", Criteria));
                    if (!string.IsNullOrEmpty(DateFrom))
                    {
                        reportViewer.ServerReport.SetParameters(new ReportParameter("StartDate", DateFrom + " 00:00:00"));
                    }
                    if (!string.IsNullOrEmpty(DateTo))
                    {
                        reportViewer.ServerReport.SetParameters(new ReportParameter("EndDate", DateTo + " 00:00:00"));
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
    }
}
