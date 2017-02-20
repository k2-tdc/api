using HKTDC.WebAPI.CHSW.Models;
using HKTDC.WebAPI.CHSW.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        [Route("users/{UserId}/draft-list/computer-app")]
        [HttpGet]
        public List<ChkFrmStatus> GetDraftList(string UserId, string EmployeeId, string refid = null, [FromUri(Name = "start-date")] string FDate = null, [FromUri(Name = "end-date")] string TDate = null, string applicant = null, int offset = 0, int limit = 999999, string sort = null)
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
                    return this.draftService.GetRequestList(refid, "Draft", FDate, TDate, applicant, UserId, "Draft", EmployeeId, offset, limit, sqlSortValue);
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                }
            }
            catch (Exception ex)
            {
                var err = this.draftService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        //Get Details Draft Edit
        [Route("users/{UserId}/draft-list/computer-app/{ReferID}")]
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
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                }
            }
            catch (Exception ex)
            {
                var err = this.draftService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }

        }

        //To Delete the Draft 
        [Route("users/{UserId}/draft-list/computer-app")]
        [HttpDelete]
        public HttpResponseMessage DeleteDraft(string UserId, string ReferID)
        {
            string result = "Failed";
            try
            {
                if (compareUser(Request, UserId))
                {
                    result = this.draftService.DeleteDraft(UserId, ReferID);

                    if (result == "Failed")
                        return Request.CreateResponse(HttpStatusCode.InternalServerError); //throws when error in SP
                    else if (result == "Unauthorized")
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
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
