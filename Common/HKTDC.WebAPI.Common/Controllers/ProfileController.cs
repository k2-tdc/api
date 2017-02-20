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
    public class ProfileController : BaseController
    {
        private ProfileService profileService;

        public ProfileController()
        {
            this.profileService = new ProfileService();
        }

        [Route("users/{UserId}/email-profiles")]
        [HttpGet]
        public List<NotificationProfileDTO> GetProfileList(string UserId, string profile = null)
        {
            try
            {
                if (compareUser(Request, UserId))
                {
                    return this.profileService.GetProfileList(UserId, profile);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.profileService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("users/{UserId}/email-profiles")]
        [HttpPost]
        public HttpResponseMessage SaveProfile(string UserId)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool, string> response = this.profileService.SaveProfile(stuff);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"success\":\"1\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                }
            }
            catch (Exception ex)
            {
                var err = this.profileService.ErrorLog(ex, "");

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("users/{UserId}/email-profiles/{ProfileID:int}")]
        [HttpPut]
        public HttpResponseMessage UpdateProfile(string UserId, int ProfileID)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool,string> response = this.profileService.SaveProfile(stuff);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"success\":\"1\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                }
            }
            catch (Exception ex)
            {
                var err = this.profileService.ErrorLog(ex, "");

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("users/{UserId}/email-profiles/{ProfileId:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteProfile(string UserId, int ProfileId)
        {
            try
            {
                if (compareUser(Request, UserId))
                {
                    bool response = this.profileService.DeleteProfile(ProfileId);
                    if (response)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"success\":\"1\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.profileService.ErrorLog(ex, "");

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("users/{UserId}/email-profiles/{ProfileId:int}")]
        [HttpGet]
        public NotificationProfileDetailDTO GetProfileDetail(string UserId, int ProfileId)
        {
            try
            {
                if (compareUser(Request, UserId))
                {
                    return this.profileService.GetProfileDetail(ProfileId, UserId);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.profileService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("profile-list")]
        [HttpGet]
        public List<UserDTO> GetProfileFieldList()
        {
            try
            {
                return this.profileService.GetProfileFieldList(this.getCurrentUser(Request));
            }
            catch (Exception ex)
            {
                var err = this.profileService.ErrorLog(ex, this.getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
