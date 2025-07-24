using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using SchoolERP_System.Models;
using SchoolERP_System.Helper;
using System.IO;
using System.Configuration;
using SelectPdf;
using System.Drawing;

namespace SchoolERP_System.Controllers
{
    [SessionAuthorizedAttribute]
    public class MessagingController : Controller
    {
        #region ST
        public ActionResult ST()
        {
            return View();
        }
        public ActionResult saveSTMaster(string STID, string STName, string STDesc)
        {
            try
            {
                string Type = "";
                if (STID == "" || STID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("STID", STID),
                    new SqlParameter("STName", STName),
                    new SqlParameter("STDesc", STDesc)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ST", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewSTMaster(string STID, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("STID", STID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ST", prm1, CommandType.StoredProcedure);
                List<ST> STList = Utility.ConvertDataTableToClassObjectList<ST>(dt);
                return Json(STList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteSTMaster(string STID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("STID", STID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ST", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region StudentSMS
        public ActionResult StudentSMS()
        {
            List<SelectListItem> DropDownlistA = new List<SelectListItem>();
            List<SelectListItem> DropDownlistB = new List<SelectListItem>();

            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            DropDownlistB = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "");
            DropDownlistA.AddRange(DropDownlistB);
            ViewBag.ClassList = DropDownlistA;
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.MSGList = Utility.GetDropDownList("SP_ST", "STID", "STName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult saveStudentSMSMaster(List<SMSStudent> StudentID, string SMSText, string SMSType)
        {
            try
            {

                HelperClass help = new HelperClass();
                DataTable dtsms = help.ExecuteSMSSettings();
                if (dtsms.Rows.Count > 0)
                {

                    var strID = string.Join(",", StudentID.Select(i => i.StudentID));
                    var strNo = string.Join(",", StudentID.Select(i => i.Mobile));
                   
                    if (strID.Length > 0 && strNo.Length > 0)
                    {
                        string msg = help.sendSms(strNo, SMSText, dtsms.Rows[0]["username"].ToString(), dtsms.Rows[0]["sendername"].ToString(), dtsms.Rows[0]["apikey"].ToString());
                        SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SMSInsert"),
                    new SqlParameter("StudentID", strID),
                    new SqlParameter("SMSText", SMSText),
                    new SqlParameter("SMSType", SMSType)
                };
                        string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_SMSInsert", prm1, CommandType.StoredProcedure));

                        return Json(Output, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult saveStudentSMSMasterAP(List<SMSStudent> StudentID, string SMSText, string SMSType)
        {
            try
            {

                HelperClass help = new HelperClass();
                DataTable dtsms = help.ExecuteSMSSettings();
                if (dtsms.Rows.Count > 0)
                {

                    var strID = string.Join(",", StudentID.Select(i => i.StudentID));
                    var strNo = string.Join(",", StudentID.Select(i => i.Mobile));

                    if (strID.Length > 0 && strNo.Length > 0)
                    {
                        string msg = help.sendSms(strNo, SMSText, dtsms.Rows[0]["username"].ToString(), dtsms.Rows[0]["sendername"].ToString(), dtsms.Rows[0]["apikey"].ToString());
                        SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SMSInsert"),
                    new SqlParameter("StudentID", strID),
                    new SqlParameter("SMSText", SMSText),
                    new SqlParameter("SMSType", "AP")
                };
                        string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_SMSInsert", prm1, CommandType.StoredProcedure));
                       
                        return Json(Output, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewStudentSMSMaster(string STID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectStudent"),
                    new SqlParameter("STID", STID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ST", prm1, CommandType.StoredProcedure);
                List<SelectStudent> STList = Utility.ConvertDataTableToClassObjectList<SelectStudent>(dt);
                return Json(STList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteStudentSMSMaster(string STID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("STID", STID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ST", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region StudentSMSIndividual
        public ActionResult StudentSMSIndividual()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            return View();
        }
        public ActionResult saveStudentSMSMasterIndividual(List<SMSStudentIndividual> StudentID, string SMSType)
        {
            try
            {

                HelperClass help = new HelperClass();
                DataTable dtsms = help.ExecuteSMSSettings();
                if (dtsms.Rows.Count > 0)
                {
                    if (StudentID.Count > 0)
                    {
                        foreach (var strStudent in StudentID)
                        {
                            string msg = help.sendSms(strStudent.Mobile, strStudent.strText, dtsms.Rows[0]["username"].ToString(), dtsms.Rows[0]["sendername"].ToString(), dtsms.Rows[0]["apikey"].ToString());

                            SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SMSInsert"),
                    new SqlParameter("StudentID", strStudent.StudentID),
                    new SqlParameter("SMSText", strStudent.strText),
                    new SqlParameter("SMSType", SMSType)
                      };
                            string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_SMSInsert", prm1, CommandType.StoredProcedure));

                        }
                    }
                    return Json("InsertSuccessful", JsonRequestBehavior.AllowGet);
                }
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
      
        public ActionResult viewStudentSMSMasterIndividual(string STID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectStudent"),
                    new SqlParameter("STID", STID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ST", prm1, CommandType.StoredProcedure);
                List<SelectStudent> STList = Utility.ConvertDataTableToClassObjectList<SelectStudent>(dt);
                return Json(STList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

      

        #endregion

        #region APSMS
        public ActionResult APSMS()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.MSGList = Utility.GetDropDownList("SP_ST", "STID", "STName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult saveAPSMSMaster(string STID, string STName, string STDesc)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Insert"),
                    new SqlParameter("STID", STID),
                    new SqlParameter("STName", STName),
                    new SqlParameter("STDesc", STDesc)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ST", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewAPSMSMaster(string STID, string section)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectStudentBYClassSection"),
                    new SqlParameter("STID", STID),
                    new SqlParameter("STName", section),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ST", prm1, CommandType.StoredProcedure);
                List<SelectStudent> STList = Utility.ConvertDataTableToClassObjectList<SelectStudent>(dt);
                return Json(STList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteAPSMSMaster(string STID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("STID", STID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ST", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region TeacherSMS
        public ActionResult TeacherSMS()
        {
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.MSGList = Utility.GetDropDownList("SP_ST", "STID", "STName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult saveTeacherSMSMaster(List<SMSStudent> StudentID, string SMSText, string SMSType)
        {
            try
            {
                HelperClass help = new HelperClass();
                DataTable dtsms = help.ExecuteSMSSettings();
                if (dtsms.Rows.Count > 0)
                {

                    var strID = string.Join(",", StudentID.Select(i => i.EMPID));
                    var strNo = string.Join(",", StudentID.Select(i => i.Mobile));
                    if (strID.Length > 0 && strNo.Length > 0)
                    {
                        string msg = help.sendSms(strNo, SMSText, dtsms.Rows[0]["username"].ToString(), dtsms.Rows[0]["sendername"].ToString(), dtsms.Rows[0]["apikey"].ToString());


                        SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SMSInsert"),
                    new SqlParameter("StudentID", strID),
                    new SqlParameter("SMSText", SMSText),
                    new SqlParameter("SMSType", SMSType)
                };
                        string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_SMSInsert", prm1, CommandType.StoredProcedure));
                        return Json(Output, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewTeacherSMSMaster(string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type)
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Employee", prm1, CommandType.StoredProcedure);
                List<Employee> STList = Utility.ConvertDataTableToClassObjectList<Employee>(dt);
                return Json(STList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }



        #endregion

        #region MANUAL SMS
        public ActionResult ManualSMS()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.MSGList = Utility.GetDropDownList("SP_ST", "STID", "STName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult ViewStudent(string STID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectStudentBYClass"),
                    new SqlParameter("STID", STID),
                    //new SqlParameter("STName", section),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ST", prm1, CommandType.StoredProcedure);
                List<SelectStudent> STList = Utility.ConvertDataTableToClassObjectList<SelectStudent>(dt);
                return Json(STList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SendManualSMS(string StudentID , string SMSText, string ClassID, string MobNo)
        {
            try
            {
                HelperClass help = new HelperClass();
                DataTable dtsms = help.ExecuteSMSSettings();
                if (dtsms.Rows.Count > 0)
                {
                    //if (StudentID.Length > 0 && strNo.Length > 0)
                    //{
                    string formatedmsg = "Dear Parent/Guardian, your child has left the school premises " +
                        SMSText + " " +
                        Convert.ToString(System.Web.HttpContext.Current.Session["SchNameDashboard"]);

                        string msg = help.sendSms(MobNo, formatedmsg, dtsms.Rows[0]["username"].ToString(), dtsms.Rows[0]["sendername"].ToString(), dtsms.Rows[0]["apikey"].ToString());
                        SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SMSInsert"),
                    new SqlParameter("StudentID", StudentID),
                    new SqlParameter("SMSText", SMSText),
                    new SqlParameter("SMSType", "AP")
                };
                        string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_SMSInsert", prm1, CommandType.StoredProcedure));

                        return Json(Output, JsonRequestBehavior.AllowGet);
                    //}
                }
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}