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

        [Route("workflow/role-permission")]
        [HttpGet]
        public List<UserPermissionDTO> GetUserPermissionList()
        {
            try
            {
                return this.userPermissionService.GetUserPermissionList(getCurrentUser(Request));
            }
            catch (Exception ex)
            {
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/role-permission")]
        [HttpDelete]
        public HttpResponseMessage DeleteUserPermission(string RolePermissionGUID)
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
                if (!string.IsNullOrEmpty(RolePermissionGUID))
                {
                    string[] perGUID = RolePermissionGUID.Split(',');
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
            }
            catch (Exception ex)
            {
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/role-permission/menuitem/{ProcessId:int}")]
        [HttpGet]
        public List<UserPermissionMenuItemDTO> GetMenuItem(int ProcessId)
        {
            try
            {
                return this.userPermissionService.GetMenuItem(ProcessId, getCurrentUser(Request));
            }
            catch (Exception ex)
            {
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/role-permission/user-role/{ProcessId:int}")]
        [HttpGet]
        public List<UserRoleDTO> GetUserRole(int ProcessId)
        {
            try
            {
                return this.userPermissionService.GetUserRole(ProcessId, getCurrentUser(Request));
            }
            catch (Exception ex)
            {
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/role-permission")]
        [HttpPost]
        public HttpResponseMessage SaveUserPermission()
        {
            try
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
            }
            catch (Exception ex)
            {
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/role-permission")]
        [HttpPut]
        public HttpResponseMessage UpdateUserPermission()
        {
            try
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
            }
            catch (Exception ex)
            {
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/role-permission-detail")]
        [HttpGet]
        public UserPermissionDetailDTO GetRolePermissionDetail(string RolePermissionGUID)
        {
            try
            {
                return this.userPermissionService.GetRolePermissionDetail(RolePermissionGUID, getCurrentUser(Request));
            }
            catch (Exception ex)
            {
                var err = this.userPermissionService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
