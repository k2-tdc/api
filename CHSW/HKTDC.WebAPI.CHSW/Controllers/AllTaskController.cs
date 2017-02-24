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
    public class AllTaskController : BaseController
    {
        AllTaskService taskService;
        
        public AllTaskController()
        {
            this.taskService = new AllTaskService();
        }

        //Get Details for WorkList Based on the Search Options
        [Route("workflow/users/{UserId}/work-list/computer-app")]
        [HttpGet]
        public List<ChkFrmStatus> GetWorklist(string UserId, string refid = null, string status = null, [FromUri(Name = "start-date")] string FDate = null, [FromUri(Name = "end-date")] string TDate = null, string SUser = null, string ProsIncId = null, int offset = 0, int limit = 999999, string sort = null)
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
                    return this.taskService.GetWorklist(refid, status, FDate, TDate, UserId, SUser, ProsIncId, offset, limit, sqlSortValue);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.taskService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }

        [HttpGet]
        public List<Review> GetWorklistDetails(string UserId, string ProsIncId, string SN, string ReferID)
        {
            try
            {
                if (compareUser(Request, UserId))
                {
                    return this.taskService.GetWorklistDetails(UserId, ProsIncId, SN, ReferID);
                }
                else
                {
                    //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.taskService.ErrorLog(ex, UserId);
                if (ex.Message.Contains("The user has no enough permission to open this item"))
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, ex.Message));
                else
                    throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));

            }
        }
    }
}
