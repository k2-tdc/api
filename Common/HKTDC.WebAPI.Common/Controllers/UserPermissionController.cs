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
    public class UserPermissionController : BaseController
    {
        private UserPermissionService userPermissionService;

        public UserPermissionController()
        {
            this.userPermissionService = new UserPermissionService();
        }

        [Route("workflow/role-permissions")]
        [HttpGet]
        public List<UserPermissionDTO> GetUserPermissionList(string process = null)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/role-permissions", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.userPermissionService.GetUserPermissionList(getCurrentUser(Request), process);
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/role-permissions/{PermissionID}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUserPermission(string PermissionID)
        {
            try
            {
                //var s = HttpContext.Current.Request.Form.GetValues("model");
                //string json = s[0];
                //if (string.IsNullOrEmpty(json))
                //    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                //dynamic stuff = JsonConvert.DeserializeObject(json);
                //foreach (dynamic usrper in stuff.RolePermissionGUID)
                //{
                //    Tuple<bool, string> response = this.userPermissionService.DeleteUserPermission(getCurrentUser(Request), usrper.ToString());
                //    if (!response.Item1)
                //    {
                //        return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                //    }
                //}
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/role-permissions/{PermissionID}", "HttpDelete", getCurrentUser(Request), null))
                {
                    if (!string.IsNullOrEmpty(PermissionID))
                    {
                        string[] perGUID = PermissionID.Split(',');
                        foreach (var usrper in perGUID)
                        {
                            Tuple<bool, string> response = this.userPermissionService.DeleteUserPermission(getCurrentUser(Request), usrper.ToString());
                            if (!response.Item1)
                            {
                                return new HttpResponseMessage { Content = new StringContent("{\"Success\":\"0\", \"Msg\":\"" + response.Item2 + "\"}", System.Text.Encoding.UTF8, "application/json") };
                            }
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
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/permissions")]
        [HttpGet]
        public List<UserPermissionMenuItemDTO> GetMenuItem(string process)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/permissions", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.userPermissionService.GetMenuItem(process, getCurrentUser(Request));
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        //[Route("workflow/role-permission/user-role/{ProcessId:int}")]
        //[HttpGet]
        //public List<UserRoleDTO> GetUserRole(int ProcessId)
        //{
        //    try
        //    {
        //        return this.userPermissionService.GetUserRole(ProcessId, getCurrentUser(Request));
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}

        [Route("workflow/role-permissions")]
        [HttpPost]
        public HttpResponseMessage SaveUserPermission()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/role-permissions", "HttpPost", getCurrentUser(Request), null))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.userPermissionService.SaveUserPermission(stuff, getCurrentUser(Request));
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
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/role-permissions")]
        [HttpPut]
        public HttpResponseMessage UpdateUserPermission()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/role-permissions", "HttpPut", getCurrentUser(Request), null))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                    string json = s[0];
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    Tuple<bool, string> response = this.userPermissionService.SaveUserPermission(stuff, getCurrentUser(Request));
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
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/role-permissions/{PermissionID}")]
        [HttpGet]
        public UserPermissionDetailDTO GetRolePermissionDetail(string PermissionID)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/role-permissions/{PermissionID}", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.userPermissionService.GetRolePermissionDetail(PermissionID, getCurrentUser(Request));
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
