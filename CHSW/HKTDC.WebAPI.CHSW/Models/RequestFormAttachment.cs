using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("RequestFormAttachment")]
    public class RequestFormAttachment
    {
        public string FileName { get; set; }
        public Nullable<int> FormID { get; set; }
        [Key]
        public System.Guid AttachmentGUID { get; set; }
        public Nullable<System.DateTime> UploadedDate { get; set; }
        public string UploadedByUserID { get; set; }
        public string UploadedByEmployeeID { get; set; }
        public string UploadedByFullName { get; set; }
        public string UploadedByDeptName { get; set; }
    }
}