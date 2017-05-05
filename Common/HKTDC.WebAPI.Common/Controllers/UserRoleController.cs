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

        [Route("workflow/user-roles")]
        [HttpGet]
        public List<UserRoleDTO> GetUserRoleList(string process = null)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/user-roles", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.userRoleService.GetUserRoleList(getCurrentUser(Request), process);
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-roles")]
        [HttpPost]
        public HttpResponseMessage SaveUserRole()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/user-roles", "HttpPost", getCurrentUser(Request), null))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.userRoleService.SaveUserRole(stuff);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
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
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-roles/{RoleID}")]
        [HttpPut]
        public HttpResponseMessage UpdateUserRole(string RoleID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/user-roles/{RoleID}", "HttpPut", getCurrentUser(Request), null))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.userRoleService.SaveUserRole(stuff);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"1\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
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
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-roles/{RoleID}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUserRole(string RoleID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/user-roles/{RoleID}", "HttpDelete", getCurrentUser(Request), null))
                {
                    Tuple<bool, string> response = this.userRoleService.DeleteUserRole(getCurrentUser(Request), RoleID);
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
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-roles/{RoleID}")]
        [HttpGet]
        public UserRoleDetailDTO GetUserRoleDetail(string RoleID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/user-roles/{RoleID}", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.userRoleService.GetUserRoleDetail(getCurrentUser(Request), RoleID);
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-role-members")]
        [HttpPost]
        public HttpResponseMessage SaveUserRoleMember()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/user-role-members", "HttpPost", getCurrentUser(Request), null))
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
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-role-members/{RoleMemberID}")]
        [HttpPut]
        public HttpResponseMessage UpdateUserRoleMember(string RoleMemberID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/user-role-members/{RoleMemberID}", "HttpPut", getCurrentUser(Request), null))
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
                        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                    }
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));

                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/user-role-members/{RoleMemberID}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUserRoleMember(string RoleMemberID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/user-role-members/{RoleMemberID}", "HttpDelete", getCurrentUser(Request), null))
                {
                    Tuple<bool, string> response = this.userRoleService.DeleteUserRoleMember(getCurrentUser(Request), RoleMemberID);
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
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("workflow/user-role-members/{RoleMemberID}")]
        [HttpGet]
        public UserRoleMemberDetailDTO GetUserRoleMemberDetail(string RoleMemberID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/user-role-members/{RoleMemberID}", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.userRoleService.GetUserRoleMemberDetail(RoleMemberID);
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.userRoleService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
