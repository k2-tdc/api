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
    public class ApprovalTaskController : BaseController
    {
        private ApprovalTaskService approvalTaskService;

        public ApprovalTaskController()
        {
            this.approvalTaskService = new ApprovalTaskService();
        }

        [Route("workflow/users/{UserId}/approval-work-list/computer-app")]
        [HttpGet]
        public List<ChkFrmStatus> GetApproveList(string UserId, string refid = null, string status = null, [FromUri(Name = "start-date")] string FDate = null, [FromUri(Name = "end-date")] string TDate = null, string SUser = null, string ProsIncId = null, int offset = 0, int limit = 999999, string sort = null, [FromUri(Name = "applicant-employee-id")] string applicantEmpNo = null, string applicant = null)
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
                    return this.approvalTaskService.GetApproveList(refid, status, FDate, TDate, UserId, SUser, ProsIncId, offset, limit, sqlSortValue, applicant, applicantEmpNo);
                }
                else
                {
                    //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                    throw new UnauthorizedAccessException();
                }
            }
            catch (Exception ex)
            {
                var err = this.approvalTaskService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }

        }
    }
}
