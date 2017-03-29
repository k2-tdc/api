using HKTDC.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HKTDC.WebAPI.Common.Controllers
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
            var headerUserID = this.getCurrentUser(request);
            if (!String.IsNullOrEmpty(headerUserID) && headerUserID.Equals(userID))
            {
                ret = true;
            }

            return ret;
        }

        public int? TryParseNullable(string val)
        {
            int outValue;
            return int.TryParse(val, out outValue) ? (int?)outValue : null;
        }
    }
}
