using HKTDC.WebAPI.Common.Models;
using HKTDC.WebAPI.Common.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public List<ProcessListDTO> GetProcessList()
        {
            try
            {
               return this.workerRuleService.GetProcessList(getCurrentUser(Request));
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
    }
}
