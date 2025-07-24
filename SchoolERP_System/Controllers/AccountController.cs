using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolERP_System.Models;
using SchoolERP_System.Helper;

namespace SchoolERP_System.Controllers
{
    public class AccountController : Controller
    {
        //public static string strConnectionString = "Data Source=103.48.50.181,1232;Initial Catalog=SchoolERP_Master_DB;User ID=sa;Password=P^YygR!12Xit&aP4LJ;Persist Security Info=True;";
        public static string strConnectionString = ConfigurationManager.AppSettings["ConStrERPAdmin"];

        [HttpGet]
        public ActionResult AdminAuthentication()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckAdminAuthentication(string userType, string Email, string Password, string Domain)
        {
            try
            {
                string Output = string.Empty;
                SqlConnection con = new SqlConnection(strConnectionString);
                using (SqlCommand cmd = new SqlCommand("SP_MasterLogin"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserType", userType);
                    cmd.Parameters.AddWithValue("@UserID", Email);
                    cmd.Parameters.AddWithValue("@UserPassword", Password);
                    cmd.Parameters.AddWithValue("@Domain", Domain);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        Output = Convert.ToString(dt.Rows[0]["output"]);
                        if (Output == "success")
                        {
                            loggedInAdmin smc = new loggedInAdmin();
                            smc.LoginID = Convert.ToString(dt.Rows[0]["LoginID"]);
                            smc.AppID = Convert.ToString(dt.Rows[0]["AppID"]);
                            smc.UserID = Convert.ToString(dt.Rows[0]["UserID"]);
                            smc.Name = Convert.ToString(dt.Rows[0]["SchName"]);
                            smc.SchoolLogo = Convert.ToString(dt.Rows[0]["SchoolLogo"]);
                            smc.SchAddress = Convert.ToString(dt.Rows[0]["SchAddress"]);
                            smc.userType = Convert.ToString(userType);
                            System.Web.HttpContext.Current.Session["SchNameDashboard"] = dt.Rows[0]["SchName"];
                            System.Web.HttpContext.Current.Session["SchoolLogo"] = dt.Rows[0]["SchoolLogo"];
                            System.Web.HttpContext.Current.Session["userType"] = userType;
                            System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;
                            System.Web.HttpContext.Current.Session["AppID"] = Convert.ToString(dt.Rows[0]["AppID"]);
                            if (Password == "123456")
                            {
                                Output = "PC";
                            }
                            else
                            {
                                Output = userType;
                            }
                            if (userType == "Parents")
                            {
                                try
                                {
                                    SqlParameter[] prm1 = new SqlParameter[] {
                                        new SqlParameter("Type", "ParentsLogin"),
                                        new SqlParameter("UserID",Email),
                                    };
                                    DataTable dtParents = new SQLHelper().ExecuteDataTable("SP_StudentMarksStudent", prm1, CommandType.StoredProcedure);
                                    loggedInParents smp = new loggedInParents();
                                    smp.SessionID = Convert.ToString(dtParents.Rows[0]["SR_SessionID"]);
                                    smp.StudentID = Convert.ToString(dtParents.Rows[0]["SR_ID"]);
                                    smp.ClassID = Convert.ToString(dtParents.Rows[0]["SR_ClassID"]);
                                    smp.UserID = Convert.ToString(dtParents.Rows[0]["UserID"]);
                                    System.Web.HttpContext.Current.Session["loggedInParents"] = smp;
                                }
                                catch (Exception ex)
                                {
                                    return Json("Error", JsonRequestBehavior.AllowGet);
                                }

                            }
                        }
                        return Json(Output, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Output = "Error";
                    }
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string Error = Convert.ToString(ex.Message);
                return Json(Error, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ResetPassword(string passsword)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                    new SqlParameter("UserType",((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
                    new SqlParameter("UserPassword", passsword),
                    new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID)
                };
                string Output = Convert.ToString(new SQLHelperMaster().ExecuteScalar("SP_ResetLogin", prm1, CommandType.StoredProcedure));
                if (Output == "resetSuccessful")
                {
                    Output = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType;
                    if (passsword == "123456")
                    {
                        Output = "PC";
                    }
                }
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Logout()
        {
            System.Web.HttpContext.Current.Session["loggedInAdmin"] = null;
            System.Web.HttpContext.Current.Session.Abandon();
            return RedirectToAction("AdminAuthentication", "Account");
        }
        [HttpPost]
        public ActionResult CheckAdminAuthentication_App(string userType, string Email, string Password, string Domain)
        {
            try
            {
                string Output = string.Empty;
                loggedInAdmin smc = new loggedInAdmin();
                SqlConnection con = new SqlConnection(strConnectionString);
                using (SqlCommand cmd = new SqlCommand("SP_MasterLogin"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserType", userType);
                    cmd.Parameters.AddWithValue("@UserID", Email);
                    cmd.Parameters.AddWithValue("@UserPassword", Password);
                    cmd.Parameters.AddWithValue("@Domain", Domain);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        Output = Convert.ToString(dt.Rows[0]["output"]);
                        if (Output == "success")
                        {

                            smc.LoginID = Convert.ToString(dt.Rows[0]["LoginID"]);
                            smc.AppID = Convert.ToString(dt.Rows[0]["AppID"]);
                            smc.UserID = Convert.ToString(dt.Rows[0]["UserID"]);
                            smc.Name = Convert.ToString(dt.Rows[0]["SchName"]);
                            smc.SchoolLogo = Convert.ToString(dt.Rows[0]["SchoolLogo"]);
                            smc.SchAddress = Convert.ToString(dt.Rows[0]["SchAddress"]);
                            smc.userType = Convert.ToString(userType);
                            System.Web.HttpContext.Current.Session["SchNameDashboard"] = dt.Rows[0]["SchName"];
                            System.Web.HttpContext.Current.Session["SchoolLogo"] = dt.Rows[0]["SchoolLogo"];
                            System.Web.HttpContext.Current.Session["userType"] = userType;
                            System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;
                            System.Web.HttpContext.Current.Session["AppID"] = Convert.ToString(dt.Rows[0]["AppID"]);

                            try
                            {
                                SqlParameter[] prm1 = new SqlParameter[] {
                                    new SqlParameter("UserType", userType),
                                    new SqlParameter("UserID",Email),
                                    new SqlParameter("UserPassword",Password),
                                    new SqlParameter("Domain",Domain)
                                };
                                DataTable dtdtl = new SQLHelper().ExecuteDataTable("SP_Login_Detail", prm1, CommandType.StoredProcedure);
                                smc.SR_Pic = Convert.ToString(dtdtl.Rows[0]["SR_Pic"]);
                                smc.SR_Email = Convert.ToString(dtdtl.Rows[0]["SR_Email"]);
                                smc.SR_PhNo = Convert.ToString(dtdtl.Rows[0]["SR_PhNo"]);
                                smc.SR_ID = Convert.ToString(dtdtl.Rows[0]["SR_ID"]);
                                smc.SR_Gender = Convert.ToString(dtdtl.Rows[0]["SR_Gender"]);
                            }
                            catch (Exception ex)
                            {
                                return Json(new { Output = "fail", Data="", Message = "Some error found" });
                            }

                        }
                        return Json(new { Output = "success", Data = smc, Message = "Data found" });
                    }
                    else
                    {
                        return Json(new { Output = "fail", Data = "", Message = "Login failed" });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = Convert.ToString(ex.Message);
                return Json(new { Output = "fail", Data = "", Message = "Some error found" });
            }
        }
    }
}