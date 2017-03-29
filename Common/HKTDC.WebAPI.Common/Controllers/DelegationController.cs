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

        [Route("workflow/users/{cuUserId}/delegation-list")]
        [HttpGet]
        public List<DelegationDTO> GetDelegationList(string cuUserId, string UserId = null)
        {
            try
            {
                //if (compareUser(Request, cuUserId))
                //{
                    return this.delegationSharingService.GetDelegationList(cuUserId, UserId);
                //}
                //else
                //{
                //    throw new UnauthorizedAccessException();
                //}
            }
            catch (Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, cuUserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/delegation-sharing-list/user")]
        [HttpGet]
        public List<UserDTO> GetDelegationUser(string Dept = null)
        {
            try
            {
                return this.delegationSharingService.GetDelegationUser(Dept);
            }
            catch (Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/delegation-sharing-list/action")]
        [HttpGet]
        public List<DelegationActionDTO> GetDelegationAction(string type)
        {

            try
            {
                return this.delegationSharingService.GetDelegationAction(type);
            }
            catch (Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/delegation-list")]
        [HttpPost]
        public HttpResponseMessage SaveDelegation(string UserId)
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

        [Route("workflow/users/{UserId}/delegation-list")]
        [HttpPut]
        public HttpResponseMessage UpdateDelegation(string UserId)
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

        [Route("workflow/users/{UserId}/delegation-list/{DelegationID:int}")]
        [HttpGet]
        public DelegationDetailDTO GetDelegationDetails(string UserId, int DelegationID)
        {
            try
            {
                if (compareUser(Request, UserId))
                {
                    return this.delegationSharingService.GetDelegationDetails(UserId, DelegationID);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.delegationSharingService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{UserId}/delegation-list/{DelegationID:int}")]
        [HttpDelete]
        public HttpResponseMessage DeleteDelegation(string UserId, int DelegationID)
        {
            try
            {
                if (compareUser(Request, UserId))
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
                var err = this.delegationSharingService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
