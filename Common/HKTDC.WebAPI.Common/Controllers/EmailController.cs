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
    public class EmailController : BaseController
    {
        private EmailService emailService;

        public EmailController()
        {
            this.emailService = new EmailService();
        }

        [Route("workflow/email-templates")]
        [HttpGet]
        public List<EmailTemplateDTO> GetEmailTemplateList(string process = null, [FromUri(Name = "activity-group")] int step = 0)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/email-templates", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.emailService.GetEmailTemplateList(getCurrentUser(Request), process, step);
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.emailService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
        
        [Route("workflow/email-templates")]
        [HttpPost]
        public HttpResponseMessage SaveEmailTemplate()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/email-templates", "HttpPost", getCurrentUser(Request), null))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.emailService.SaveEmailTemplate(getCurrentUser(Request), stuff);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        //return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.emailService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/email-templates/{templateID:int}")]
        [HttpPut]
        public HttpResponseMessage UpdateEmailTemplate(int templateID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/email-templates/{templateID}", "HttpPost", getCurrentUser(Request), null))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.emailService.SaveEmailTemplate(getCurrentUser(Request), stuff);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        //return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.emailService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/email-templates/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteEmailTemplate()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/email-templates/delete", "HttpPost", getCurrentUser(Request), null))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");

                    string json = s[0];

                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    foreach (dynamic item in stuff.data)
                    {
                        int TemplateId = item.TemplateId;
                        Tuple<bool, string> response = this.emailService.DeleteEmailTemplate(getCurrentUser(Request), TemplateId);
                        if (!response.Item1)
                        {
                            return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                        }
                    }

                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.emailService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/email-templates/{templateID:int}")]
        [HttpGet]
        public EmailTemplateDetailDTO GetEmailTemplateDetail(int templateID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/email-templates/{templateID}", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.emailService.GetEmailTemplateDetail(getCurrentUser(Request), templateID);
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.emailService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
