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
    public class HistoryController : BaseController
    {
        private HistoryService historyService;

        public HistoryController()
        {
            historyService = new HistoryService();
        }
        
        [Route("users/{UserId}/approval-history/computer-app/{ReferID}")]
        [HttpGet]
        public List<Review> GetHistoryDetails(string ReferID, string UserId, string ProInstID)
        {
            try
            {
                if (compareUser(Request, UserId))
                {
                    return this.historyService.GetHistoryDetails(ReferID, UserId, ProInstID);
                }
                else
                {
                    throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unable to get data"));
                }
            }
            catch (Exception ex)
            {
                var err = this.historyService.ErrorLog(ex, UserId);
                throw new HttpResponseException(Request.CreateErrorResponse(err.Code, err.Message));
            }
        }
    }
}
