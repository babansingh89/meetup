using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SchoolERP_System.Models;
using CrystalDecisions.Shared.Json;

namespace SchoolERP_System.OpenReport
{
    public partial class OpenReports : System.Web.UI.Page
    {
        ExportFormatType formatType = ExportFormatType.NoFormat;
        ReportDocument crystalReport = new ReportDocument();
        //string conString = ConfigurationManager.AppSettings["ConStrERPAdmin"];

        Dictionary<string, string> Params;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            var keys = Request.QueryString["ReportName"];
            if (keys != null)
            {
                JObject obj = JObject.Parse(keys);
                var master = (JArray)obj.SelectToken("Master");
                var detal = (JArray)obj.SelectToken("Detail");

                GenerateReport(master, detal);
            }
        }
        private void GenerateReport(JArray master, JArray detal)
        {
            string server = "", database = "", userid = "", password = ""; 
            try
            {
                string reportName = master[0]["ReportName"].ToString();
                string FileName = master[0]["FileName"].ToString();
                string AppID = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID; 

                if (detal != null && detal.Count > 0)
                {
                    detal[0]["AppID"] = AppID;
                }

                SqlConnection con = new SqlConnection("Password=school@123;Persist Security Info=True;User ID=schoollogin;Initial Catalog=SchoolERP_" + AppID + "_DB;Data Source=198.38.81.242,1232");
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                crystalReport.Load(Server.MapPath("~/Reports/" + reportName));
                crystalReport.Refresh();

                builder.ConnectionString = con.ConnectionString;

                server = builder.DataSource;
                database = builder.InitialCatalog;   //"SchoolERP_18_DB";//
                userid = builder.UserID;
                password = builder.Password;

                crystalReport.DataSourceConnections[0].SetConnection(server, database, userid, password);
                crystalReport.DataSourceConnections[0].IntegratedSecurity = false;
                crystalReport.SetDatabaseLogon(server, database, userid, password);

                for (int i = 0; i < crystalReport.Subreports.Count; i++)
                {
                    crystalReport.Subreports[i].DataSourceConnections[0].SetConnection(server, database, userid, password);
                    crystalReport.Subreports[i].DataSourceConnections[0].IntegratedSecurity = false;
                    crystalReport.Subreports[i].SetDatabaseLogon(server, database, userid, password);
                }

                crystalReport.VerifyDatabase();

                foreach (var item in detal)
                {
                    string a = item.ToString();
                    string noNewLines = a.Replace("\n", "");
                    Params = JsonConvert.DeserializeObject<Dictionary<string, string>>(noNewLines);
                }

                if (Params != null)
                {
                    if (Params.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> entry in Params)
                        {
                            crystalReport.SetParameterValue("@" + entry.Key, entry.Value);
                        }
                    }
                }


                formatType = ExportFormatType.PortableDocFormat;
                crystalReport.ExportToHttpResponse(formatType, Response, false, FileName);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                ClientScript.RegisterStartupScript(this.GetType(), "Error", "<script>alert('" + ex.Message.Replace("'", "") + "')</script>");
            }
            finally
            {
                crystalReport.Close();
                crystalReport.Dispose();
                GC.Collect();
            }
        }

    }
}
    
    