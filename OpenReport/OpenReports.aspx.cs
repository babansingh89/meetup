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
            // Add this line to fix the HTTPS/TLS handshake issue
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls12 |
                System.Net.SecurityProtocolType.Tls11 |
                System.Net.SecurityProtocolType.Tls;

            var keys = Request.QueryString["ReportName"];
            if (keys != null)
            {
                JObject obj = JObject.Parse(keys);
                var master = (JArray)obj.SelectToken("Master");
                var detal = (JArray)obj.SelectToken("Detail");

                GenerateReport(master, detal);
            }
        }
        private void GenerateReportold2(JArray master, JArray detal)
        {
            string server = "", database = "", userid = "", password = "";
            try
            {
                // 1. Basic Report Setup
                string reportName = master[0]["ReportName"].ToString();
                string FileName = master[0]["FileName"].ToString();
                string AppID = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID;

                crystalReport.Load(Server.MapPath("~/Reports/" + reportName));

                // 2. Database Connection Logic
                string connStr = "Password=school@123;Persist Security Info=True;User ID=schoollogin;Initial Catalog=SchoolERP_" + AppID + "_DB;Data Source=103.118.16.231,1232";
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connStr);

                crystalReport.DataSourceConnections[0].SetConnection(builder.DataSource, builder.InitialCatalog, builder.UserID, builder.Password);
                crystalReport.SetDatabaseLogon(builder.UserID, builder.Password, builder.DataSource, builder.InitialCatalog);

                // 3. IMAGE PROCESSING & RESIZING LOGIC
                // We look for the image name in your JSON parameters
                foreach (var item in detal)
                {
                    string a = item.ToString();
                    Params = JsonConvert.DeserializeObject<Dictionary<string, string>>(a.Replace("\n", ""));
                }

                if (Params != null && Params.ContainsKey("ImageName"))
                {
                    string imgName = "3941909e_16112024214657.jpg"; // Example: "logo.jpg"
                    string physicalPath = Server.MapPath("~/SchImage/" + imgName);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // Resize the image to 200x200 pixels
                        byte[] resizedImage = ResizeImageToByteArray(physicalPath, 200, 200);

                        // Pass the actual image data to a Parameter named 'FixedImage'
                        // Important: Create 'FixedImage' parameter in your RPT file first!
                        crystalReport.SetParameterValue("FixedImage", resizedImage);
                    }
                }

                // 4. Handle other Text Parameters
                if (Params != null)
                {
                    foreach (KeyValuePair<string, string> entry in Params)
                    {
                        if (crystalReport.ParameterFields["@" + entry.Key] != null)
                        {
                            crystalReport.SetParameterValue("@" + entry.Key, entry.Value);
                        }
                    }
                }

                // 5. Export to PDF
                formatType = ExportFormatType.PortableDocFormat;
                crystalReport.ExportToHttpResponse(formatType, Response, false, FileName);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "");
                ClientScript.RegisterStartupScript(this.GetType(), "Error", "<script>alert('" + errorMsg + "')</script>");
            }
            finally
            {
                crystalReport.Close();
                crystalReport.Dispose();
                GC.Collect();
            }
        }

        // Helper Function to handle the actual resizing
        // Helper Function to handle the actual resizing
        private byte[] ResizeImageToByteArray(string imagePath, int width, int height)
        {
            using (System.Drawing.Image sourceImage = System.Drawing.Image.FromFile(imagePath))
            {
                // Create a new bitmap with the exact fixed dimensions
                using (System.Drawing.Bitmap objBitmap = new System.Drawing.Bitmap(width, height))
                {
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(objBitmap))
                    {
                        // High-quality scaling settings
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        // Draw the source image into the fixed-size destination
                        g.DrawImage(sourceImage, 0, 0, width, height);
                    }
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        objBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }
            }
        }
        private void GenerateReport(JArray master, JArray detal)
        {
            string server = "", database = "", userid = "", password = "";
            try
            {
                // 1. Basic Report Setup
                string reportName =  master[0]["ReportName"].ToString();
                string FileName = master[0]["FileName"].ToString();
                string AppID = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID;

                if (detal != null && detal.Count > 0)
                {
                    detal[0]["AppID"] = AppID;
                }

                // 2. Database Connection Logic
                string connStr = "Password=school@123;Persist Security Info=True;User ID=schoollogin;Initial Catalog=SchoolERP_" + AppID + "_DB;Data Source=103.118.16.231,1232";
                crystalReport.Load(Server.MapPath("~/Reports/" + reportName));

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connStr);
                server = builder.DataSource;
                database = builder.InitialCatalog;
                userid = builder.UserID;
                password = builder.Password;

                crystalReport.DataSourceConnections[0].SetConnection(server, database, userid, password);
                crystalReport.DataSourceConnections[0].IntegratedSecurity = false;
                crystalReport.SetDatabaseLogon(userid, password, server, database);

                // Apply connection to Subreports
                for (int i = 0; i < crystalReport.Subreports.Count; i++)
                {
                    crystalReport.Subreports[i].DataSourceConnections[0].SetConnection(server, database, userid, password);
                    crystalReport.Subreports[i].SetDatabaseLogon(userid, password, server, database);
                }

                string folderPath = Server.MapPath("~/SchImage/");
                if (!folderPath.EndsWith("\\")) folderPath += "\\";
                crystalReport.SetParameterValue("FolderPath", folderPath);

                string imagePath = Server.MapPath("~/UploadedImage/");
                if (!imagePath.EndsWith("\\")) imagePath += "\\";
                crystalReport.SetParameterValue("StudentImagePath", imagePath);

                // 4. Handle other Parameters from JSON
                foreach (var item in detal)
                {
                    string a = item.ToString();
                    Params = JsonConvert.DeserializeObject<Dictionary<string, string>>(a.Replace("\n", ""));
                }

                if (Params != null)
                {
                    foreach (KeyValuePair<string, string> entry in Params)
                    {
                        // Check if parameter exists in report before setting to avoid errors
                        if (crystalReport.ParameterFields["@" + entry.Key] != null)
                        {
                            crystalReport.SetParameterValue("@" + entry.Key, entry.Value);
                        }
                    }
                }

                // 5. Export and Clean up
                formatType = ExportFormatType.PortableDocFormat;
                crystalReport.ExportToHttpResponse(formatType, Response, false, FileName);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", "");
                ClientScript.RegisterStartupScript(this.GetType(), "Error", "<script>alert('" + errorMsg + "')</script>");
            }
            finally
            {
                crystalReport.Close();
                crystalReport.Dispose();
                GC.Collect();
            }
        }
        private void GenerateReportold(JArray master, JArray detal)
        {
            string server = "", database = "", userid = "", password = ""; 
            try
            {
                string reportName = "Report4.rpt"; // master[0]["ReportName"].ToString();
                string FileName = master[0]["FileName"].ToString();
                string AppID = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID; 

                if (detal != null && detal.Count > 0)
                {
                    detal[0]["AppID"] = AppID;
                }

                SqlConnection con = new SqlConnection("Password=school@123;Persist Security Info=True;User ID=schoollogin;Initial Catalog=SchoolERP_" + AppID + "_DB;Data Source=103.118.16.231,1232");
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

               // crystalReport.VerifyDatabase();

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
    
    