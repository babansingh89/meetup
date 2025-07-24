using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolERP_System.Areas.ERPAdmin.Models
{
    public class MstModels
    {

    }

    public class APPInfo
    {
        public string AppID { get; set; }
        public string SchName { get; set; }
        public string SchAddress { get; set; }
        public string Contact { get; set; }
        public string EmailID { get; set; }
        public string LicenceFrom { get; set; }
        public string LicenceTo { get; set; }
        public string IsPresentSMS { get; set; }
        public string IsAbsentSMS { get; set; }
        public string UserID { get; set; }
        public string UserPassword { get; set; }
        public string SchoolLogo { get; set; }
    }
    public class SupportAdmin
    {
        public string SupportID { get; set; }
        public string AppID { get; set; }
        public string TicketNo { get; set; }
        public string IssueTitle { get; set; }
        public string IssueDesc { get; set; }
        public string Status { get; set; }
        public string ResolveDesc { get; set; }
        public string ResolveDate { get; set; }
        public string UserID { get; set; }
        public string IssueBy { get; set; }
        public string CreatedDate { get; set; }
    }
}