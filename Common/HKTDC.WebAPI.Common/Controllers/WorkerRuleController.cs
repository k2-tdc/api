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

        [Route("workflow/worker-rules/process")]
        [HttpGet]
        public List<ProcessListDTO> GetProcessList(string MenuId)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rules/process", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.workerRuleService.GetProcessList(getCurrentUser(Request), MenuId);
                } else
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

        [Route("workflow/worker-rules")]
        [HttpGet]
        public List<WorkerRuleDTO> GetWorkerRuleList(string process=null)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rules", "HttpGet", getCurrentUser(Request), process))
                {
                    return this.workerRuleService.GetWorkerRuleList(getCurrentUser(Request), process);
                } else
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

        [Route("workflow/worker-rules")]
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
                int? ProcessId;
                ProcessId = stuff.ProcessId;

                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rules", "HttpPost", getCurrentUser(Request), ProcessId.ToString()))
                {
                    Tuple<bool, string> response = this.workerRuleService.SaveWorkerRule(getCurrentUser(Request), stuff);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                } else
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

        [Route("workflow/worker-rules/{WorkerRuleId:int}")]
        [HttpPut]
        public HttpResponseMessage UpdateWorkerRule(int WorkerRuleId)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                int? ProcessId;
                ProcessId = stuff.ProcessId;

                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rules/{WorkerRuleId}", "HttpPut", getCurrentUser(Request), ProcessId.ToString()))
                {
                    Tuple<bool, string> response = this.workerRuleService.SaveWorkerRule(getCurrentUser(Request), stuff);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                } else
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

        [Route("workflow/worker-rules/{WorkerRuleId:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteWorkerRule(int WorkerRuleId)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rules/{WorkerRuleId}", "HttpDelete", getCurrentUser(Request), WorkerRuleId.ToString()))
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
                } else
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

        [Route("workflow/worker-rules/{WorkerRuleId:int}")]
        [HttpGet]
        public WorkerRuleDetailDTO GetWorkerRuleDetail(int WorkerRuleId)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rules/{WorkerRuleId}", "HttpGet", getCurrentUser(Request), WorkerRuleId.ToString()))
                {
                    return this.workerRuleService.GetWorkerRuleDetail(getCurrentUser(Request), WorkerRuleId);
                } else
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
        
        [Route("workflow/worker-rule-settings")]
        [HttpGet]
        public List<WorkerRuleRuleListDTO> GetWorkerRuleRuleList([FromUri(Name = "worker-rule-id")] int WorkerRuleId, [FromUri(Name = "user-id")] string UserId = null, [FromUri(Name = "worker-id")] string WorkerId = null)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings", "HttpGet", getCurrentUser(Request), WorkerRuleId.ToString()))
                {
                    return this.workerRuleService.GetWorkerRuleRuleList(getCurrentUser(Request), WorkerRuleId, UserId, WorkerId);
                } else
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

        [Route("workflow/worker-rule-settings/natures")]
        [HttpGet]
        public List<WorkerRuleNatureDTO> GetWorkerRuleNature()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/natures", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.workerRuleService.GetWorkerRuleNature();
                } else
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

        [Route("workflow/worker-rule-settings/grades")]
        [HttpGet]
        public List<WorkerRuleGradingDTO> GetWorkerRuleGrade()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/grades", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.workerRuleService.GetWorkerRuleGrade();
                } else
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

        [Route("workflow/worker-rule-settings/priorities")]
        [HttpGet]
        public List<WorkerRulePriorityDTO> GetWorkerRulePriority([FromUri(Name = "worker-rule-id")] int WorkerRuleId)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/priorities", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.workerRuleService.GetWorkerRulePriority(WorkerRuleId);
                } else
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

        //[Route("workflow/worker-rule/department")]
        //[HttpGet]
        //public List<WorkerRuleDepartmentDTO> GetWorkerRuleDepartment()
        //{
        //    try
        //    {
        //        return this.workerRuleService.GetWorkerRuleDepartment();
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.workerRuleService.ErrorLog(ex, getCurrentUser(Request));
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}

        [Route("workflow/worker-rule-settings/criteria")]
        [HttpGet]
        public List<WorkerRuleCriteriaDTO> GetWorkerRuleCriteria([FromUri(Name= "worker-rule-id")] int WorkerRuleId)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/criteria", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.workerRuleService.GetWorkerRuleCriteria(WorkerRuleId);
                } else
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

        [Route("workflow/worker-rule-settings/levels")]
        [HttpGet]
        public List<WorkerRuleOrgChartDTO> GetWorkerRuleOrgChart()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/levels", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.workerRuleService.GetWorkerRuleOrgChart();
                } else
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

        [Route("workflow/worker-rule-settings/user-groups")]
        [HttpGet]
        public List<WorkerRuleUserGroupDTO> GetWorkerRuleUserGroup()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/user-groups", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.workerRuleService.GetWorkerRuleUserGroup();
                } else
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

        [Route("workflow/worker-rule-settings/teams")]
        [HttpGet]
        public List<WorkerRuleTeamDTO> GetWorkerRuleTeam()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/teams", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.workerRuleService.GetWorkerRuleTeam();
                } else
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

        [Route("workflow/worker-rule-settings/team-filters")]
        [HttpGet]
        public List<WorkerRuleTeamFilterDTO> GetWorkerRuleTeamFilter()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/team-filters", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.workerRuleService.GetWorkerRuleTeamFilter();
                } else
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

        [Route("workflow/worker-rule-settings")]
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
                int WorkerRuleId = stuff.WorkerRuleId;
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings", "HttpPost", getCurrentUser(Request), WorkerRuleId.ToString()))
                {
                    Tuple<bool, string> response = this.workerRuleService.SaveWorkerRuleRule(getCurrentUser(Request), stuff);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                } else
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

        [Route("workflow/worker-rule-settings/{WorkerRuleSettingId:int}")]
        [HttpPut]
        public HttpResponseMessage UpdateWorkerRuleRule(int WorkerRuleSettingId)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                int WorkerRuleId = stuff.WorkerRuleId;
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/{WorkerRuleSettingId}", "HttpPut", getCurrentUser(Request), WorkerRuleId.ToString()))
                {
                    Tuple<bool, string> response = this.workerRuleService.SaveWorkerRuleRule(getCurrentUser(Request), stuff);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                } else
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

        [Route("workflow/worker-rule-settings/{WorkerRuleSettingId:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteWorkerRuleRule(int WorkerRuleSettingId)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/{WorkerRuleSettingId}", "HttpDelete", getCurrentUser(Request), WorkerRuleSettingId.ToString()))
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
                } else
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

        [Route("workflow/worker-rule-settings/{WorkerRuleSettingId:int}")]
        [HttpGet]
        public WorkerRuleRuleDetailDTO GetWorkerRuleRuleDetail(int WorkerRuleSettingId)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/{WorkerRuleSettingId}", "HttpGet", getCurrentUser(Request), WorkerRuleSettingId.ToString()))
                {
                    return this.workerRuleService.GetWorkerRuleRuleDetail(getCurrentUser(Request), WorkerRuleSettingId);
                } else
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

        [Route("workflow/worker-rule-settings/rules")]
        [HttpGet]
        public List<WorkerRuleRuleDTO> GetWorkerRuleRule([FromUri(Name = "worker-rule-id")] int WorkerRuleId)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rule-settings/rules", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.workerRuleService.GetWorkerRuleRule(WorkerRuleId);
                } else
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

        [Route("workflow/worker-rules/{WorkerRuleId:int}/preview")]
        [HttpPost]
        public HttpResponseMessage WorkerRulePreview(int WorkerRuleId)
        {
            try
            {
                //bool havePermission = this.workerRuleService.checkHavePermission(getCurrentUser(Request), "ADMIN", "Process Worker");
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/worker-rules/{WorkerRuleId}/preview", "HttpPost", getCurrentUser(Request), WorkerRuleId.ToString()))
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
