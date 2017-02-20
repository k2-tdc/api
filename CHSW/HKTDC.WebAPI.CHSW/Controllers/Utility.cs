using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Controllers
{
    public class Utility
    {
        public static string Generate(string name, int FormId, string RefId)
        {
            return string.Format("{0}-{1}-{2}", name, RefId, FormId);
        }

        public static string changeSqlCodeDraftList(string pro)
        {
            string returnValue = "";
            string order = "";
            if (pro[0] == '+')
            {
                order = " desc";
            }
            pro = pro.Substring(1);
            switch (pro)
            {
                case "lastactiontime": returnValue = "a.[ModifiedOn]"; break;
                case "applicant": returnValue = "a.[ApplicantFullName]"; break;
                default: break;
            }
            return returnValue + order;
        }
    }
}