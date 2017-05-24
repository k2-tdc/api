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
    public class SharingController : BaseController
    {
        private DelegationSharingService delegationSharingService;

        public SharingController()
        {
            this.delegationSharingService = new DelegationSharingService();
        }

        [Route("workflow/users/{uid}/sharing-list")]
        [HttpGet]
        public List<DelegationDTO> GetSharingList(string uid)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/sharing-list", "HttpGet", getCurrentUser(Request), uid))
                {
                    return this.delegationSharingService.GetSharingList(getCurrentUser(Request), uid);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/sharing-list/actions")]
        [HttpGet]
        public List<DelegationActionDTO> GetSharingPermission()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/sharing-list/actions", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.delegationSharingService.GetDelegationAction("Sharing");
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/sharing-list")]
        [HttpPost]
        public HttpResponseMessage SaveSharing(string uid)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/sharing-list", "HttpPost", getCurrentUser(Request), uid))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.delegationSharingService.SaveDelegation(getCurrentUser(Request), stuff, true);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/sharing-list/{SharingID:int}")]
        [HttpPut]
        public HttpResponseMessage UpdateSharing(string uid)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/sharing-list/{SharingID}", "HttpPut", getCurrentUser(Request), uid))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.delegationSharingService.SaveDelegation(getCurrentUser(Request), stuff, true);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/sharing-list/{SharingID:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteSharing(string uid, int SharingID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/sharing-list/{SharingID}", "HttpDelete", getCurrentUser(Request), uid))
                {
                    Tuple<bool, string> response = this.delegationSharingService.DeleteSharing(getCurrentUser(Request), SharingID);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/sharing-list/{SharingID:int}")]
        [HttpGet]
        public DelegationDetailDTO GetSharingDetails(string uid, int SharingID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/sharing-list/{SharingID}", "HttpGet", getCurrentUser(Request), uid))
                {
                    return this.delegationSharingService.GetSharingDetails(uid, SharingID);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/share-work-list")]
        [HttpGet]
        public List<UserDTO> GetShareUserList(string uid, string process = null, string type = null)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/share-work-list", "HttpGet", getCurrentUser(Request), uid))
                {
                    return this.delegationSharingService.GetShareUserList(uid, process, type);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
