using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("RequestFormCC")]
    public class RequestFormCC
    {
        public string UserID { get; set; }
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public string Remark { get; set; }
        [Key]
        public int CCID { get; set; }
        public Nullable<int> FormID { get; set; }
        public string DeptName { get; set; }
    }
}