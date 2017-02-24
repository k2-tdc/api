using HKTDC.WebAPI.CHSW.Models;
using HKTDC.WebAPI.CHSW.Services;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HKTDC.WebAPI.CHSW.Controllers
{
    public class ReportController : BaseController
    {
        private ReportService reportService;

        public ReportController()
        {
            this.reportService = new ReportService();
        }

        [Route("workflow/departments")]
        [HttpGet]
        public List<Dept> getDeptList()
        {
            return reportService.getDeptList();
        }

        [Route("workflow/users")]
        [HttpGet]
        public List<VWEmployeeDTO> getApplicantList()
        {
            return reportService.getApplicantList();
        }
    }
}
