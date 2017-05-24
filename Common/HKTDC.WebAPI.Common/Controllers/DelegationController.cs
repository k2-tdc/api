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
    public class DelegationController : BaseController
    {
        private DelegationSharingService delegationSharingService;

        public DelegationController()
        {
            this.delegationSharingService = new DelegationSharingService();
        }

        [Route("workflow/users/{uid}/delegation-list")]
        [HttpGet]
        public List<DelegationDTO> GetDelegationList(string uid)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/delegation-list", "HttpGet", getCurrentUser(Request), uid))
                {
                    return this.delegationSharingService.GetDelegationList(getCurrentUser(Request), uid);
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

        //[Route("workflow/delegation-sharing-list/user")]
        //[HttpGet]
        //public List<UserDTO> GetDelegationUser(string Dept = null)
        //{
        //    try
        //    {
        //        return this.delegationSharingService.GetDelegationUser(Dept);
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.delegationSharingService.ErrorLog(ex, getCurrentUser(Request));
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}

        [Route("workflow/delegation-list/actions")]
        [HttpGet]
        public List<DelegationActionDTO> GetDelegationAction()
        {

            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/delegation-list/actions", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.delegationSharingService.GetDelegationAction("Delegation");
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

        [Route("workflow/users/{uid}/delegation-list")]
        [HttpPost]
        public HttpResponseMessage SaveDelegation(string uid)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/delegation-list", "HttpPost", getCurrentUser(Request), uid))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.delegationSharingService.SaveDelegation(getCurrentUser(Request), stuff);
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
            catch(Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/delegation-list/{DelegationID:int}")]
        [HttpPut]
        public HttpResponseMessage UpdateDelegation(string uid, int DelegationID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/delegation-list/{DelegationID}", "HttpPut", getCurrentUser(Request), uid))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.delegationSharingService.SaveDelegation(getCurrentUser(Request), stuff);
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

        [Route("workflow/users/{uid}/delegation-list/{DelegationID:int}")]
        [HttpGet]
        public DelegationDetailDTO GetDelegationDetails(string uid, int DelegationID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/delegation-list/{DelegationID}", "HttpGet", getCurrentUser(Request), uid))
                {
                    return this.delegationSharingService.GetDelegationDetails(uid, DelegationID);
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

        [Route("workflow/users/{uid}/delegation-list/{DelegationID:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteDelegation(string uid, int DelegationID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/delegation-list/{DelegationID}", "HttpDelete", getCurrentUser(Request), uid))
                {
                    Tuple<bool, string> response = this.delegationSharingService.DeleteDelegation(getCurrentUser(Request), DelegationID);
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
    }
}
