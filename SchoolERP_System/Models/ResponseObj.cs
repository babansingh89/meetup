using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace SchoolERP_System.Models
{
    public class ResponseObj
    {
        public int success { get; set; }
        public string message { get; set; }
        public DataTable data { get; set; }
        public DataSet dataSet { get; set; }
    }
}