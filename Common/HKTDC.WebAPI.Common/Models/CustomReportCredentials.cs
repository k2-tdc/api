using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    public class CustomReportCredentials : IReportServerCredentials
    {
        private string _UserName;
        private string _PassWord;
        private string _DomainName;

        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }

        public CustomReportCredentials(string UserName, string PassWord)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            //_DomainName = DomainName;
        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }

        public ICredentials NetworkCredentials
        {
            //get { return new NetworkCredential(_UserName, _PassWord, _DomainName); }
            get { return new NetworkCredential(_UserName, _PassWord); }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user,
        out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}