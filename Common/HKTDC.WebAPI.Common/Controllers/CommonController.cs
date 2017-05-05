using HKTDC.WebAPI.Common.Exceptions;
using HKTDC.WebAPI.Common.Models;
using HKTDC.WebAPI.Common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;

namespace HKTDC.WebAPI.Common.Controllers
{
    public class CommonController : BaseController
    {
        private CommonService commonService;

        public CommonController()
        {
            this.commonService = new CommonService();
        }

        [Route("workflow/applications")]
        [HttpPost]
        public Reference GetReferenceId()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/applications", "HttpPost", getCurrentUser(Request), null))
                {
                    return this.commonService.GetReferenceId(getCurrentUser(Request));
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/departments")]
        [HttpGet]
        public List<Dept> getDeptList(string type=null)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/departments", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.commonService.getDeptList(type);
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            } catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/activity-groups")]
        [HttpGet]
        public List<ProcessStepListDTO> GetEmailProcessStepList(string process = null, [FromUri(Name = "activity-group-type")] string activityGroupType = null)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/activity-groups", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.commonService.GetEmailProcessStepList(process, activityGroupType);
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
        
        [Route("workflow/attachments")]
        [HttpPost]
        public HttpResponseMessage SubmitFile(string refid, string process, string AttachmentType = null)
        {

            string result = "Failed";
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/attachments", "HttpPost", getCurrentUser(Request), refid))
                {
                    HttpRequestMessage request = this.Request;
                    if (!request.Content.IsMimeMultipartContent())
                    {
                        //throws when request without file attachment
                        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                    }

                    result = this.commonService.UploadFiles(HttpContext.Current.Request, getCurrentUser(Request), refid, process, AttachmentType);

                    if (result == "Failed")
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Unable to Upload file(s)"); //throws when error in SP
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/attachments")]
        [HttpGet]
        public HttpResponseMessage DownloadFile(string guid, string process)
        {
            try
            {
                //var headerUserID = getCurrentUser(Request);
                //if (!String.IsNullOrEmpty(headerUserID))
                //{
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/attachments", "HttpGet", getCurrentUser(Request), guid))
                {
                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                    var attachmentRecord = this.commonService.GetAttachment(process, new Guid(guid), getCurrentUser(Request));
                    if (attachmentRecord != null)
                    {
                        string Dircur = attachmentRecord.UNCPath + "/" + attachmentRecord.FormID.ToString() + (!string.IsNullOrEmpty(attachmentRecord.AttachmentType) ? ("/" + attachmentRecord.AttachmentType) : ""); //UNC Path From Web Config

                        if (Directory.Exists(Dircur))
                        {
                            var filePath = Dircur + "/" + attachmentRecord.AttachmentGUID.ToString() + "." + attachmentRecord.FileName.Substring(attachmentRecord.FileName.LastIndexOf('.') + 1);
                            var stream = new FileStream(filePath, FileMode.Open);
                            result.Content = new StreamContent(stream);
                            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                            result.Content.Headers.ContentDisposition.FileName = attachmentRecord.FileName;
                            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            result.Content.Headers.ContentLength = stream.Length;
                        }
                        return result;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to get the attachment");
                    }
                } else
                {
                    throw new UnauthorizedAccessException();
                }
                //}
                //else
                //{
                //    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized request.");
                //}
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/attachments")]
        [HttpDelete]
        public HttpResponseMessage DeleteFile(string guid, string process)
        {
            try
            {
                #region old_code
                //var s = HttpContext.Current.Request.Form.GetValues("model");

                //string json;
                ////To check whether the input is from request body or formdata
                //if (s == null)
                //    json = JsonConvert.SerializeObject(request);
                //else
                //    json = s[0];
                //if (string.IsNullOrEmpty(json))
                //    throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                //dynamic stuff = JsonConvert.DeserializeObject(json);
                //foreach (dynamic item in stuff.files)
                //{
                //    this.requestService.DeleteFile((item.GUID).ToString());
                //}
                #endregion
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/attachments", "HttpDelete", getCurrentUser(Request), guid))
                {
                    this.commonService.DeleteFile(guid, process);
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/attachments/config")]
        [HttpGet]
        public List<CommonSettingsDTO> GetFileType(string process)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/attachments/config", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.commonService.GetAttachmentSetting(process);
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users")]
        [HttpGet]
        public List<UserDTO> getAllUser()
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users", "HttpGet", getCurrentUser(Request), null))
                {
                    return this.commonService.getAllUser(getCurrentUser(Request));
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        // Get Selected Applicant Details Dept,Title 
        [Route("workflow/users/{uid}")]
        [HttpGet]
        public ApplicantDetails GetApplicant(string uid)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}", "HttpGet", getCurrentUser(Request), uid))
                {
                    return this.commonService.GetApplicantDetails(uid);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/workers")]
        [HttpGet]
        public List<Applicant> GetApprover(string uid, string rule)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/workers", "HttpGet", getCurrentUser(Request), uid))
                {
                    return this.commonService.GetAllEmployeeDetails(rule, null, uid, null);
                }
                else
                {
                    //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }


        [Route("workflow/users/{uid}/applications")]
        [HttpGet]
        public List<ProcessListDTO> GetProcessList(string uid)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/applications", "HttpGet", getCurrentUser(Request), uid))
                {
                    return this.commonService.GetProcessList();
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/work-list-count")]
        [HttpGet]
        public HttpResponseMessage GetWorklistCount(string uid, string process = null)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/work-list-count", "HttpGet", getCurrentUser(Request), uid))
                {
                    Tuple<bool, string> response = this.commonService.GetWorklistCount(uid, process);
                    if (response.Item1)
                    {
                        return new HttpResponseMessage { Content = new StringContent(response.Item2, System.Text.Encoding.UTF8, "application/json") };
                    }
                    else
                    {
                        //return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                        return new HttpResponseMessage { Content = new StringContent("", System.Text.Encoding.UTF8, "application/json") };
                    }
                } else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
        private string GenerateJsonData(string UserID, string ProcessName, string Unread, string Push)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[{");
            sb.AppendFormat("\"user\":\"{0}\"", UserID);
            sb.Append(",");
            sb.AppendFormat("\"type\":\"{0}\"", ProcessName);
            sb.Append(",");
            sb.AppendFormat("\"unread\":{0}", Unread);
            sb.Append(",");
            sb.AppendFormat("\"push\":{0}", Push);
            sb.Append("}]");

            return sb.ToString();
        }
        [Route("workflow/push-notification-work-list-count")]
        [HttpGet]
        public HttpResponseMessage GetWorklistCountForPushNotification(string UserId, string ProcessName, string Push)
        {
            try
            {
                Tuple<bool, int> response = this.commonService.GetWorklistCountByProcess(UserId, ProcessName);
                if (response.Item1)
                {
                    int pendingworkitemCount = response.Item2;
                    string endpointUrl = System.Configuration.ConfigurationManager.AppSettings.Get("PushNotificationEndpoint").ToString();
                    string postData = GenerateJsonData(UserId, ProcessName, pendingworkitemCount.ToString(), Push);
                    var client = new RESTClientService(endpointUrl, HttpVerb.POST, postData);
                    string strResult = client.MakeRequest();
                    return new HttpResponseMessage { Content = new StringContent(strResult, System.Text.Encoding.UTF8, "application/json") };
                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.InternalServerError, "Fail");
                    return new HttpResponseMessage { Content = new StringContent("", System.Text.Encoding.UTF8, "application/json") };
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/applications/authorized-pages")]
        [HttpGet]
        public Menus GetMenuItem(string uid, string process, string page = null)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/applications/authorized-pages", "HttpGet", getCurrentUser(Request), uid))
                {
                    //var watch = System.Diagnostics.Stopwatch.StartNew();
                    ProcessList pList = this.commonService.GetProcess(process.ToUpper());
                    if (pList != null)
                    {
                        Menus menu = this.commonService.GetMenuItem(uid, process, page);
                        //watch.Stop();
                        //var elapsedMs = watch.ElapsedMilliseconds;
                        //this.commonService.LogTime("Menu - " + page, elapsedMs);
                        return menu;
                    } else
                    {
                        throw new ProcessNotFoundException();
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        //[Route("workflow/users/{UserId}/applications/{ProcessName}/process-list")]
        //[HttpGet]
        //public List<ProcessListDTO> GetProcessListForWorkerRule(string UserId, string ProcessName)
        //{
        //    try
        //    {
        //        //if (compareUser(Request, UserId))
        //        //{
        //        return this.commonService.GetProcessListForWorkerRule(UserId, ProcessName);
        //        //return null;
        //        //}
        //        //else
        //        //{
        //        //throw new UnauthorizedAccessException();
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        var err = this.commonService.ErrorLog(ex, UserId);
        //        throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
        //    }
        //}
        
        [Route("workflow/workers/{WorkId}/owners")]
        [HttpGet]
        public List<Applicant> GetEmployee(string WorkId, string rule, string UserId = null)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/workers/{WorkId}/owners", "HttpGet", getCurrentUser(Request), WorkId))
                {
                    return this.commonService.GetAllEmployeeDetails(rule, WorkId, UserId, null);
                }
                else
                {
                    //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [Route("workflow/users/{uid}/work-list")]
        [HttpGet]
        public List<ChkFrmStatus> GetWorklist(string uid, int offset = 0, int limit = 99999, string sort = null, string refid = null, string status = null, [FromUri(Name = "start-date")] string FDate = null, [FromUri(Name = "end-date")] string TDate = null, string ProsIncId = null, string process = null)
        {
            try
            {
                if (HKTDC.Utils.AuthorizationUtil.CheckApiAuthorized("workflow/users/{uid}/work-list", "HttpGet", getCurrentUser(Request), uid))
                {
                    string sqlSortValue = "";
                    if (!String.IsNullOrEmpty(sort))
                    {
                        string[] tmp = sort.Split(',');
                        List<string> tmpArr = new List<string>();
                        foreach (var i in tmp)
                        {
                            tmpArr.Add(Utility.changeSqlCodeDraftList(i));
                        }
                        sqlSortValue = String.Join(",", tmpArr.ToArray());
                    }
                    return this.commonService.GetWorklist(refid, status, FDate, TDate, uid, ProsIncId, offset, limit, sqlSortValue, process);
                }
                else
                {
                    //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.commonService.ErrorLog(ex, getCurrentUser(Request));
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
