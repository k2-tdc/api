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

        [Route("email-templates")]
        [HttpGet]
        public List<EmailTemplateDTO> GetEmailTemplateList(string process = null, int step = 0)
        {
            try
            {
                return this.emailService.GetEmailTemplateList(process, step);
            }
            catch (Exception ex)
            {
                var err = this.emailService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
        
        [Route("email-templates")]
        [HttpPost]
        public HttpResponseMessage SaveEmailTemplate()
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool, string> response = this.emailService.SaveEmailTemplate(stuff);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\""+response.Item2+"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.emailService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("email-templates/{TemplateId:int}")]
        [HttpPut]
        public HttpResponseMessage UpdateEmailTemplate(int TemplateId)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool,string> response = this.emailService.SaveEmailTemplate(stuff);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.emailService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("email-templates/{TemplateId:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteEmailTemplate(int TemplateId)
        {
            try
            {
                Tuple<bool,string> response = this.emailService.DeleteEmailTemplate(getCurrentUser(Request), TemplateId);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                    return new HttpResponseMessage { Content = new StringContent("{\"success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.emailService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("email-templates/{TemplateId:int}")]
        [HttpGet]
        public EmailTemplateDetailDTO GetEmailTemplateDetail(int TemplateId)
        {
            try
            {
                return this.emailService.GetEmailTemplateDetail(TemplateId);
            }
            catch (Exception ex)
            {
                var err = this.emailService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
