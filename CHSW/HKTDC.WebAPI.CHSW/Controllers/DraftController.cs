using HKTDC.WebAPI.CHSW.Models;
using HKTDC.WebAPI.CHSW.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HKTDC.WebAPI.CHSW.Controllers
{
    public class DraftController : BaseController
    {
        private DraftService draftService;

        public DraftController()
        {
            this.draftService = new DraftService();
        }

        //Get Details for Draft List Based on the Search Options
        [Route("workflow/users/{UserId}/draft-list/computer-app")]
        [HttpGet]
        public List<ChkFrmStatus> GetDraftList(string UserId, string refid = null, [FromUri(Name = "start-date")] string FDate = null, [FromUri(Name = "end-date")] string TDate = null, string applicant = null, int offset = 0, int limit = 999999, string sort = null, [FromUri(Name = "applicant-employee-id")] string applicantEmpNo = null, string EmployeeId = null)
        {
            try
            {
                if (compareUser(Request, UserId))
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
                    return this.draftService.GetRequestList(refid, "Draft", FDate, TDate, applicant, UserId, "Draft", applicantEmpNo, offset, limit, sqlSortValue);
                }
                else
                {
                    //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.draftService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        //Get Details Draft Edit
        [Route("workflow/users/{UserId}/draft-list/computer-app/{ReferID}")]
        [HttpGet]
        public List<Review> GetDraftDetails(string ReferID, string UserId)
        {
            try
            {
                if (compareUser(Request, UserId))
                {
                    return this.draftService.GetRequestDetails(ReferID, UserId, null, "Draft");
                }
                else
                {
                    //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.draftService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }

        }

        //To Delete the Draft 
        [Route("workflow/users/{UserId}/draft-list/computer-app/delete")]
        [HttpPost]
        public HttpResponseMessage DeleteDraft(string UserId, [FromBody] dynamic request)
        {
            string result = "Failed";
            try
            {
                if (compareUser(Request, UserId))
                {
                    var s = HttpContext.Current.Request.Form.GetValues("model");
                
                    string json;
                    //To check whether the input is from request body or formdata
                    if (s == null)
                        json = JsonConvert.SerializeObject(request);
                    else
                        json = s[0];
                    
                    if (string.IsNullOrEmpty(json))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);//throws when request without content
                    dynamic stuff = JsonConvert.DeserializeObject(json);
                    foreach (dynamic item in stuff.data)
                    {
                        result = this.draftService.DeleteDraft(UserId, (item.ReferenceID).ToString());
                        if (result == "Failed")
                            return Request.CreateResponse(HttpStatusCode.InternalServerError); //throws when error in SP
                        else if (result == "Unauthorized")
                            return Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                    

                    if (result == "Failed")
                        return Request.CreateResponse(HttpStatusCode.InternalServerError); //throws when error in SP
                    else if (result == "Unauthorized")
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.draftService.ErrorLog(ex, "");
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
