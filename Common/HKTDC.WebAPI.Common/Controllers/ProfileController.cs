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

        [Route("workflow/users/{uid}/email-profiles")]
        [HttpGet]
        public List<NotificationProfileDTO> GetProfileList(string uid)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/email-profiles", "HttpGet", getCurrentUser(Request), uid))
                {
                    return this.profileService.GetProfileList(getCurrentUser(Request), uid);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.profileService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/email-profiles")]
        [HttpPost]
        public HttpResponseMessage SaveProfile(string uid)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/email-profiles", "HttpPost", getCurrentUser(Request), uid))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.profileService.SaveProfile(stuff);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                    }
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.profileService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/email-profiles/{profileID:int}")]
        [HttpPut]
        public HttpResponseMessage UpdateProfile(string uid, int profileID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/email-profiles/{profileID}", "HttpPut", getCurrentUser(Request), uid))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.profileService.SaveProfile(stuff);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                    }
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.profileService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/email-profiles/{profileID:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteProfile(string uid, int profileID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/email-profiles/{profileID}", "HttpDelete", getCurrentUser(Request), uid))
                {
                    bool response = this.profileService.DeleteProfile(profileID);
                    if (response)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\"}", System.Text.Encoding.UTF8, "application/json") };
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
                var err = this.profileService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/email-profiles/{profileID:int}")]
        [HttpGet]
        public NotificationProfileDetailDTO GetProfileDetail(string uid, int profileID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/email-profiles/{profileID}", "HttpGet", getCurrentUser(Request), uid))
                {
                    return this.profileService.GetProfileDetail(profileID, uid);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.profileService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        //[Route("workflow/profile-list")]
        //[HttpGet]
        //public List<UserDTO> GetProfileFieldList()
        //{
        //    try
        //    {
        //        return this.profileService.GetProfileFieldList(this.getCurrentUser(Request));
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.profileService.ErrorLog(ex, this.getCurrentUser(Request));
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}
    }
}
