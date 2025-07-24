using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolERP_System.Models
{
    public class loggedInAdmin
    {
        public string LoginID { get; set; }
        public string AppID { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public string SchAddress { get; set; }
        public string SchoolLogo { get; set; }
        public string userType { get; set; }
        public string OutPut { get; set; }

        public string SR_Pic { get; set; }
        public string SR_Email { get; set; }
        public string SR_PhNo { get; set; }
        public string SR_ID { get; set; }
        public string SR_Gender { get; set; }
    }
    public class loggedInParents
    {
        public string StudentID { get; set; }
        public string ClassID { get; set; }
        public string SectionID { get; set; }
        public string SessionID { get; set; }
        public string UserID { get; set; }
    }
}