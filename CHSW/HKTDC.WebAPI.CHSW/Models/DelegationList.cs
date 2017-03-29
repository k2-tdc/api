﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HKTDC.WebAPI.CHSW.Models
{
    [Table("DelegationList")]
    public class DelegationList
    {
        public string DelegationType { get; set; }
        public string FromUser_UserID { get; set; }
        public string FromUser_EmployeeID { get; set; }
        public string FromUser_FullName { get; set; }
        public string ToUser_UserID { get; set; }
        public string ToUser_EmployeeID { get; set; }
        public string ToUser_FullName { get; set; }
        public string ToUser_Dept { get; set; }
        public string Enabled { get; set; }
        public string Remark { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        [Key]
        public int DelegationID { get; set; }
        public int ProcessID { get; set; }
        public Nullable<int> ActivityGroupID { get; set; }
        public string Permission { get; set; }
    }
}