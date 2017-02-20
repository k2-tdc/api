using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HKTDC.WebAPI.CHSW.Controllers
{
    public class BaseController : ApiController
    {
        public string getCurrentUser(HttpRequestMessage request)
        {
            IEnumerable<string> headerValues;
            var headerUserID = string.Empty;
            var keyFound = request.Headers.TryGetValues("X-User-ID", out headerValues);
            if (keyFound)
            {
                headerUserID = headerValues.FirstOrDefault();
            }

            return headerUserID;
        }

        public bool compareUser(HttpRequestMessage request, String userID)
        {
            bool ret = false;
            IEnumerable<string> headerValues;
            var headerUserID = string.Empty;
            var keyFound = request.Headers.TryGetValues("X-User-ID", out headerValues);
            if (keyFound)
            {
                headerUserID = headerValues.FirstOrDefault();
            }

            if (!String.IsNullOrEmpty(headerUserID) && Utility.Equals(headerUserID, userID))
            {
                ret = true;
            }

            return ret;
        }
    }
}
