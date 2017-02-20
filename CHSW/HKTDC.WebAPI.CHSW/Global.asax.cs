using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace HKTDC.WebAPI.CHSW
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
                        GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}