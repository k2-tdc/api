using HKTDC.WebAPI.CHSW.DBContext;
using HKTDC.WebAPI.CHSW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Services
{
    public class ReportService : BaseService
    {
        private EntityContext Db = new EntityContext();

        public List<Dept> getDeptList()
        {
            List<Dept> deptList = new List<Dept>();
            deptList = Db.vDepartment.Select(p => new Dept
            {
                DeptCode = p.CODE,
                DeptName = p.DESCRIPTION
            }).OrderBy(p => p.DeptName).ToList();
            return deptList;
        }

        public List<VWEmployeeDTO> getApplicantList()
        {
            List<VWEmployeeDTO> applicantList = new List<VWEmployeeDTO>();
            applicantList = Db.VW_EMPLOYEE.Select(p => new VWEmployeeDTO
            {
                UserId = p.UserID,
                UserFullName = p.FullName
            }).OrderBy(p => p.UserFullName).ToList();
            return applicantList;
        }
    }
}