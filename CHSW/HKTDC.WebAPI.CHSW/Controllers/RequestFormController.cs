using HKTDC.WebAPI.CHSW.Models;
using HKTDC.WebAPI.CHSW.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace HKTDC.WebAPI.CHSW.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RequestFormController : BaseController
    {
        private RequestFormService requestService;

        public RequestFormController()
        {
            this.requestService = new RequestFormService();
        }

        //Save the New Request details to the given Tables
        [Route("applications/computer-app")]
        [HttpPost]
        public HttpResponseMessage SubmitRequests([FromBody] dynamic request)
        {
            string result = "Failed";
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");

                string json;
                //To check whether the input is from request body or formdata
                if (s == null)
                    json = JsonConvert.SerializeObject(request);
                else
                    json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest); //throws when request without content
                result = this.requestService.SaveNewRequest(json);
                if (!Utility.Equals(result, "Failed"))
                {
                    // Start the workflow
                    this.requestService.startWorkFlow(result);
                }
            }
            catch (Exception ex)
            {
                result = "Failed"; //throws when parsing json
                var err = this.requestService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
            if (result == "Failed")
                return Request.CreateResponse(HttpStatusCode.InternalServerError); //throws when error in SP
            else
                //return Request.CreateResponse(HttpStatusCode.OK, result);
                return new HttpResponseMessage { Content = new StringContent("{\"FormID\":\"" + result + "\"}", System.Text.Encoding.UTF8, "application/json") };
        }
        
        #region old code
        
        ////Get the Menu Items Based on the Login User & Process 
        ////[Route("users/{UserId}/applications/computer-app/authorized-pages/{ProcessId?}")]
        //[HttpGet]
        //public Menus GetMenuItems(string UserId, string ProcessId)
        //{
        //    try
        //    {
        //        if (compareUser(Request, UserId))
        //        {
        //            return this.requestService.GetMenuItem(UserId, ProcessId);
        //        } else
        //        {
        //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
        //        }               
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, UserId);
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }

        //}
        
        //// Get Selected Applicant Details Dept,Title 
        ////[Route("users/{UserId}")]
        //[HttpGet]
        //public ApplicantDetails GetApplicant(string UserId, string Applicant)
        //{
        //    try
        //    {
        //        if (compareUser(Request, UserId))
        //        {
        //            return this.requestService.GetApplicantDetails(Applicant);
        //        } else
        //        {
        //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, UserId);
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}

        //// Get Applicant or Recommend By Details From SP          
        //[HttpGet]
        //public List<Applicant> GetEmployee(string RuleID, string WorkId, string UserId)
        //{
        //    try
        //    {
        //        if (compareUser(Request, UserId) || compareUser(Request, WorkId))
        //        {
        //            return this.requestService.GetAllEmployeeDetails(RuleID, WorkId, UserId, null);
        //        } else
        //        {
        //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, UserId);
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}
        //[HttpGet]
        //public List<Applicant> GetEmployee(string RuleID, string WorkId, string UserId, string EstCost)
        //{
        //    try
        //    {
        //        if (compareUser(Request, UserId) || compareUser(Request, WorkId))
        //        {
        //            return this.requestService.GetAllEmployeeDetails(RuleID, WorkId, UserId, EstCost);
        //        } else
        //        {
        //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, UserId);
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}
 
        ////[Route("users/{UserId}/workers")]
        //[HttpGet]
        //public List<Applicant> GetApprover(string RuleID, string WorkId, string UserId, string EstCost, string Applicant)
        //{
        //    try
        //    {
        //        if (compareUser(Request, UserId) || compareUser(Request, WorkId))
        //        {
        //            return this.requestService.GetAllEmployeeDetails(RuleID, WorkId, Applicant, EstCost);
        //        }
        //        else
        //        {
        //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, UserId);
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}
        
        ////Get Status for Check Status Dropdown
        ////[Route("applications/computer-app/status")]
        //[HttpGet]
        //public List<Reference> GetStatus(string task)
        ////public HttpResponseMessage GetStatus(string task)
        //{
        //    try
        //    {
        //        task = WebUtility.UrlDecode(task);
        //        //WorkflowFacade workfacade = new WorkflowFacade();
        //        //workfacade.StartProcessInstance("3");
        //        return this.requestService.GetStatusDetails(task);  
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, "");
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}
        
        //// Get ProcessList Details
        ////[Route("users/{UserId}/applications")]
        //[HttpGet]
        //public List<ProcessMenu> GetProcessList(String UserId)
        //{
        //    try
        //    {
        //        if (compareUser(Request, UserId))
        //        {
        //            return this.requestService.ProcessList(UserId);
        //        }
        //        else
        //        {
        //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, "");
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));

        //    }

        //}

        //// Get Process Step List Details
        ////[Route("applications/{ProId:int}/steps")]
        //[HttpGet]
        //public List<ProcessStepList> GetProcessStepList(int ProId)
        //{
        //    try
        //    {
        //        return this.requestService.ProcessStepList(ProId);
        //    }

        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, "");
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));

        //    }

        //}

        ////Submit New Delegation 
        //[HttpPost]
        //public HttpResponseMessage SubmitDelegation([FromBody] dynamic request)
        //{
        //    string result = "Failed";
        //    try
        //    {
        //        var s = HttpContext.Current.Request.Form.GetValues("model");

        //        string json;
        //        //To check whether the input is from request body or formdata
        //        if (s == null)
        //            json = JsonConvert.SerializeObject(request);
        //        else
        //            json = s[0];
        //        if (string.IsNullOrEmpty(json))
        //            throw new HttpResponseException(HttpStatusCode.BadRequest); //throws when request without content
        //        result = this.requestService.SaveNewDelegation(json);

        //    }
        //    catch (Exception ex)
        //    {
        //        result = "Failed"; //throws when parsing json
        //        var err = this.requestService.ErrorLog(ex, "");
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //    if (result == "Failed")
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, result); //throws when error in SP
        //    else
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        //// Get Delegation Details By Details From SP          
        ////[Route("users/{UserId}/delegation-list/{DeleId}")]
        //[HttpGet]
        //public List<DelegationDetails> GetDelegationDetails(string UserId, string DeleId, string ProId, string StepId, string Type)
        //{
        //    try
        //    {
        //        if (compareUser(Request, UserId))
        //        {
        //            return this.requestService.GetAllDelegationDetails(UserId, DeleId, ProId, StepId, Type);
        //        } else
        //        {
        //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, UserId);
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}

        //// Get Delegation Details By Details From SP          
        ////[Route("users/{UserId}/delegation-list/{DeleId}")]
        //[HttpPost]
        //public string DeleteDelegation(string UserId, string DeleId)
        //{
        //    try
        //    {
        //        return this.requestService.DeleteDelegationDetails(DeleId);
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, "");
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}

        //// Get Delegation List Details
        ////[Route("users/{UserId}/delegation-list")]
        //[HttpGet]
        //public List<DelegationList> GetDelegationList(string Type, string UserId)
        //{
        //    try
        //    {
        //        if (compareUser(Request, UserId))
        //        {
        //            return this.requestService.DelegationList(Type, UserId);
        //        }
        //        else
        //        {
        //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, "");
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));

        //    }

        //}

        

        ////[Route("users/{UserId}/work-list/computer-app/{ActionName}")]
        //[HttpPost]
        //public string WorklistAction([FromBody] dynamic request)
        //{
        //    string UserId, SN, ActionName, Remark, ForwardID;
        //    try
        //    {
        //        var s = HttpContext.Current.Request.Form.GetValues("model");

        //        string json;
        //        //To check whether the input is from request body or formdata
        //        if (s == null)
        //            json = JsonConvert.SerializeObject(request);
        //        else
        //            json = s[0];
        //        if (string.IsNullOrEmpty(json))
        //            throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
        //        dynamic stuff = JsonConvert.DeserializeObject(json);
        //        UserId = stuff.UserId; SN = stuff.SN; ActionName = stuff.ActionName; Remark = stuff.Remark;
        //        //if (ActionName == "Forward" && stuff.GetType().GetProperty("Forward_To_ID") != null)
        //        if (ActionName == "Forward")
        //        {
        //            ForwardID = stuff.Forward_To_ID;
        //            //this.requestService.Redirect(SN, ForwardID);
        //            //return "Success";
        //            return this.requestService.WorklistAction(UserId, SN, ActionName, Remark, ForwardID);
        //        }
        //        else
        //        {
        //            return this.requestService.WorklistAction(UserId, SN, ActionName, Remark, null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, "");

        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }

        //}
        
        ////Error Log
        //public HttpResponseMessage LogEror(string err, string UserId)
        //{
        //    Exception ex = new Exception(err);
        //    this.requestService.ErrorLog(ex, UserId);
        //    return Request.CreateResponse(HttpStatusCode.OK, "success");
        //}

        ////[Route("workers/{WorkId}/owners")]
        //[HttpGet]
        //public List<Applicant> GetApplicantList(string RuleID, string WorkId, string UserId, string Type, string EmployeeId)
        //{
        //    try
        //    {
        //        if (compareUser(Request, UserId) || compareUser(Request, WorkId))
        //        {
        //            Type = WebUtility.UrlDecode(Type);
        //            return this.requestService.GetApplicantList(RuleID, WorkId, UserId, Type, EmployeeId);
        //        } else
        //        {
        //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, UserId);
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}
        
        //[HttpGet]
        //public List<Applicant> GetFromUser(string RuleID, string WorkId)
        //{
        //    try
        //    {
        //        if (compareUser(Request, WorkId))
        //        {
        //            return this.requestService.GetEmployeeDetailsWithRole(RuleID, WorkId);
        //        }
        //        else
        //        {
        //            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.requestService.ErrorLog(ex, WorkId);
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}

        #endregion
    }
}
