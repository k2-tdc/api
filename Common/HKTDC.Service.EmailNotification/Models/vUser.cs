using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTDC.Service.EmailNotification.Models
{
    [Table("vUser")]
    public class vUser
    {
        [Key]
        public string UserID { get; set; }
        public string EmployeeID { get; set; }
        public string DEPT { get; set; }
        public string LocationCode { get; set; }
        public string DeptCode { get; set; }
        public string FullName { get; set; }
        public string OfficeTel { get; set; }
        public string Email { get; set; }
        public string grade { get; set; }
        public string GradeLevel { get; set; }
        public string EmpStatus { get; set; }
        public string DisplayOrder { get; set; }
        public string ROLE { get; set; }
        public string SH { get; set; }
        public string DH { get; set; }
        public string CM { get; set; }
        public string AD { get; set; }
        public string DED { get; set; }
        public string ED { get; set; }
        public Nullable<System.DateTime> TermDate { get; set; }
    }
}
