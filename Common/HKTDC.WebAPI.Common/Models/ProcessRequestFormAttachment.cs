using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.Common.Models
{
    [Table("ProcessRequestFormAttachment")]
    public class ProcessRequestFormAttachment
    {
        [Key]
        public int ProcessRequestFormAttachmentID { get; set; }
        public int ProcessID { get; set; }
        public string UNCPath { get; set; }
        public Nullable<int> FormID { get; set; }
        public System.Guid AttachmentGUID { get; set; }
        public string FileName { get; set; }
        public Nullable<System.DateTime> UploadedDate { get; set; }
        public string UploadedByUserID { get; set; }
        public string UploadedByEmployeeID { get; set; }
        public string UploadedByFullName { get; set; }
        public string UploadedByDeptName { get; set; }
        public string FileType { get; set; }
        public decimal FileSize { get; set; }
        public string Remark { get; set; }
        public string AttachmentType { get; set; }
    }
}