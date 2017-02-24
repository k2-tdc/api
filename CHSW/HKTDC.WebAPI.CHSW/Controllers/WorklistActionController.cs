using HKTDC.WebAPI.CHSW.Models;
using HKTDC.WebAPI.CHSW.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HKTDC.WebAPI.CHSW.Controllers
{
    public class WorklistActionController : BaseController
    {
        private WorklistActionService worklistActionService;
        private ApprovalTaskService approvalTaskService;

        public WorklistActionController()
        {
            this.worklistActionService = new WorklistActionService();
            this.approvalTaskService = new ApprovalTaskService();
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/resend-email")]
        [HttpPost]
        public HttpResponseMessage resendEmail(string UserId)
        {
            try
            {
                if (compareUser(Request, UserId))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);

                    int status = this.worklistActionService.ResendEmail(UserId, Int32.Parse(stuff.FormID.ToString()));

                    if (status == 0)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                    } else if(status == 1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"Not the form applicant or not in apporval status.\"}", System.Text.Encoding.UTF8, "application/json") };
                    } else if(status == 2)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\0\", \"Msg\":\"Form not found.\"}", System.Text.Encoding.UTF8, "application/json") };
                    } else
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\0\", \"Msg\":\"Unhandled Exception.\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to perform action"));
                }
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/applications/computer-app/{ApplicationId}/recall")]
        [HttpPost]
        public string RecallAction(string ApplicationId, [FromBody] dynamic request)
        {
            string UserId, ProcInstID, ActionName, Remark;
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
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                UserId = stuff.UserId; ProcInstID = stuff.ProcInstID; ActionName = stuff.ActionName; Remark = stuff.Comment;
                return this.worklistActionService.RecallAction(UserId, ProcInstID, ActionName, Remark);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");

                if (ex.Message.Contains("Unable to recall the task"))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, ex.Message));
                else
                    throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }

        }

        [Route("workflow/users/{UserId}/work-list/computer-app/approve")]
        [HttpPost]
        public string ApproveAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/reject")]
        [HttpPost]
        public string RejectAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/complete")]
        [HttpPost]
        public string CompleteAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/return-to-applicant")]
        [HttpPost]
        public string ReturnToApplicantAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/recommend")]
        [HttpPost]
        public string RecommendAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/send-to-approver")]
        [HttpPost]
        public string SendToApproverAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/return")]
        [HttpPost]
        public string ReturnAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/delete")]
        [HttpPost]
        public string DeleteAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/forward")]
        [HttpPost]
        public string ForwardAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), stuff.Forward_To_ID.ToString());
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/cancel")]
        [HttpPost]
        public string CancelAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/send-to-its")]
        [HttpPost]
        public string SendToITSAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/send-to-applicant")]
        [HttpPost]
        public string SendToApplicantAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/return-to-prepare")]
        [HttpPost]
        public string ReturnToPrepareAction(string UserId, [FromBody] dynamic request)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                dynamic stuff = deserializaePar(s, request);
                return performWorklistAction(UserId, stuff.SN.ToString(), stuff.ActionName.ToString(), stuff.Remark.ToString(), null);
            }
            catch (Exception ex)
            {
                var err = this.worklistActionService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        protected dynamic deserializaePar(string[] s, dynamic request)
        {
            string json;
            //To check whether the input is from request body or formdata
            if (s == null)
                json = JsonConvert.SerializeObject(request);
            else
                json = s[0];
            if (string.IsNullOrEmpty(json))
                throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
            dynamic stuff = JsonConvert.DeserializeObject(json);
            return stuff;
        }

        protected string performWorklistAction(string UserId, string SN, string ActionName, string Remark, string ForwardID)
        {
            if (ActionName == "Forward")
            {
                return this.worklistActionService.WorklistAction(UserId, SN, ActionName, Remark, ForwardID);
            }
            else
            {
                return this.worklistActionService.WorklistAction(UserId, SN, ActionName, Remark, null);
            }
        }

        [Route("workflow/users/{UserId}/work-list/computer-app/{SN}")]
        [HttpGet]
        public List<Review> GetApproveDetails(string UserId, string SN, string ReferID)
        {
            try
            {
                if (compareUser(Request, UserId))
                {
                    string ProsIncId = SN.Substring(0, SN.IndexOf('_'));
                    return this.approvalTaskService.GetApproveDetails(UserId, ProsIncId, SN, ReferID);
                }
                else
                {
                    //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.approvalTaskService.ErrorLog(ex, UserId);
                if (ex.Message.Contains("The user has no enough permission to open this item"))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, ex.Message));
                else
                    throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));

            }
        }
    }
}
