using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("SharingList")]
    public class SharingList
    {
        public string DelegationType { get; set; }
        public string FromWorkerID { get; set; }
        public string FromWorkerEmployeeID { get; set; }
        public string FromWorkerFullName { get; set; }
        public string ToWorkerID { get; set; }
        public string ToWorkerEmployeeID { get; set; }
        public string ToWorkerFullName { get; set; }
        public string UserDeptCode { get; set; }
        public string Enabled { get; set; }
        public string Remark { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        [Key]
        public int DelegationID { get; set; }
        public Nullable<int> ActivityGroupID { get; set; }
        public string Permission { get; set; }
    }
}