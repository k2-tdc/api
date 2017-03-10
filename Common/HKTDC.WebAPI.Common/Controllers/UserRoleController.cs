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
    public class UserRoleController : BaseController
    {
        private UserRoleService userRoleService;

        public UserRoleController()
        {
            this.userRoleService = new UserRoleService();
        }

        [Route("workflow/user-role")]
        [HttpGet]
        public List<UserRoleDTO> GetUserRoleList()
        {
            try
            {
                return this.userRoleService.GetUserRoleList();
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-role")]
        [HttpPost]
        public HttpResponseMessage SaveUserRole()
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool, string> response = this.userRoleService.SaveUserRole(stuff);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\""+response.Item1+"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item1 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-role/{RoleId}")]
        [HttpPut]
        public HttpResponseMessage UpdateUserRole(string RoleId)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool, string> response = this.userRoleService.SaveUserRole(stuff);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"" + response.Item1 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item1 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-role/{RoleId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUserRole(string RoleId)
        {
            try
            {
                Tuple<bool, string> response = this.userRoleService.DeleteUserRole(getCurrentUser(Request), RoleId);
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
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-role/{RoleId}")]
        [HttpGet]
        public UserRoleDetailDTO GetUserRoleDetail(string RoleId)
        {
            try
            {
                return this.userRoleService.GetUserRoleDetail(RoleId);
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-role-member")]
        [HttpPost]
        public HttpResponseMessage SaveUserRoleMember()
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool, string> response = this.userRoleService.SaveUserRoleMember(stuff, getCurrentUser(Request));
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item1 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-role-member/{RoleMemberId}")]
        [HttpPut]
        public HttpResponseMessage UpdateUserRoleMember(string RoleMemberId)
        {
            try
            {
                var s = HttpContext.Current.Request.Form.GetValues("model");
                string json = s[0];
                if (string.IsNullOrEmpty(json))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                dynamic stuff = JsonConvert.DeserializeObject(json);
                Tuple<bool, string> response = this.userRoleService.UpdateUserRoleMember(stuff, getCurrentUser(Request));
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item1 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-role-member/{RoleMemberId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUserRoleMember(string RoleMemberId)
        {
            try
            {
                Tuple<bool, string> response = this.userRoleService.DeleteUserRoleMember(getCurrentUser(Request), RoleMemberId);
                if (response.Item1)
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"\"}", System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                }
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("workflow/user-role-member/{RoleMemberId}")]
        [HttpGet]
        public UserRoleMemberDetailDTO GetUserRoleMemberDetail(string RoleMemberId)
        {
            try
            {
                return this.userRoleService.GetUserRoleMemberDetail(RoleMemberId);
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
