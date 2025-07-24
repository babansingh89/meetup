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
using System.Globalization;

namespace SchoolERP_System.Controllers
{
    [SessionAuthorizedAttribute]
    public class DailyAttendanceController : Controller
    {
        #region Take
        public ActionResult TakeAttendance()
        {
            if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType.ToString() == "SuperAdmin")
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
                ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            }
            else
            {
                SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectAssignClass"),
                  new SqlParameter("UserID",  ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                  new SqlParameter("userType",  ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
            };
                ViewBag.ClassList = Utility.GetDropDownList("SP_EmpClassMapping", "ClassID", "ClassName", prm2, "", "", "Select");
            }
            return View();
        }
        public ActionResult viewStudentForAttendance(string ClassID, string SectionID)
        {
            try
            {
                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectStudent"),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SectionID", SectionID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_AttendanceSave", prm1, CommandType.StoredProcedure);
                List<SelectStudent> STList = Utility.ConvertDataTableToClassObjectList<SelectStudent>(dt);
                string msg = "Yes";
                if (dt.Columns.Count == 1)
                    msg = dt.Rows[0][0].ToString();
                if (STList.Count == 0)
                    msg = "No Record Found!";
                mixArray[0] = msg;
                mixArray[1] = STList;

                return Json(mixArray, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public DataTable GetAttdanceTable(List<AttendanceSave> stddt)
        {
            GenLib objLib = new GenLib();
            DataTable dt = new DataTable();
            dt = objLib.ToDataTable(stddt);
            return dt;
        }
        public ActionResult saveStudentAttendance(string ClassID, string SectionID, List<AttendanceSave> strStudent)
        {
            DataTable dtst = null;
            DataTable dttAdr = GetAttdanceTable(strStudent);
            SqlParameter[] prmm = new SqlParameter[]
                     {
                              new SqlParameter("type", "SelectInManual"),
                               new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID)
                     };
            DataTable dt = new SQLHelperMaster().ExecuteDataTable("SP_AppInfo", prmm, CommandType.StoredProcedure);
            if (dt.Rows.Count > 0 && (dt.Rows[0]["Is_Manual_A"].ToString().ToLower() == "true" || dt.Rows[0]["Is_Manual_P"].ToString().ToLower() == "true"))
            {
                SqlParameter[] prm = new SqlParameter[] {
                  new SqlParameter("@type", "GetStudentForSMS"),
                  new SqlParameter("@ClassID", ClassID),
                  new SqlParameter("@SectionID", SectionID),
                  new SqlParameter("@dtAttendance", dttAdr)
                  };
                dtst = new SQLHelper().ExecuteDataTable("SP_AttendanceSave", prm, CommandType.StoredProcedure);
            }
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@type", "Insert"),
                  new SqlParameter("@ClassID", ClassID),
                  new SqlParameter("@SectionID", SectionID),
                  new SqlParameter("@dtAttendance", dttAdr),
                  new SqlParameter("CreatedBy", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID)

            };
            var Output = new SQLHelper().ExecuteScalar("SP_AttendanceSave", prm1, CommandType.StoredProcedure);
            if (Output.ToString() == "Yes" && dt.Rows.Count > 0 && dtst.Rows.Count > 0)
            {
                InSMS(dt, dtst);
            }

            return Json(Output, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region  InSMS Methods
        public void InSMS(DataTable dt, DataTable dtst)
        {
            HelperClass objHelp = new HelperClass();
            try
            {
                if (dtst.Rows.Count > 0)
                {
                    for (int ij = 0; ij < dtst.Rows.Count; ij++)
                    {
                        string MSG = null;
                        MSG = dt.Rows[0]["Present_Text"].ToString().Replace("@@STUDENT##", dtst.Rows[ij]["SR_StudentName"].ToString()).Replace("@@SCHOOL##", "- " + dt.Rows[0]["SchName"].ToString()).Replace("@@AT##", DateTime.Today.ToString("dd-MM-yyyy"));
                        if (dt.Rows[0]["Is_Manual_P"].ToString().ToLower() == "true")
                            objHelp.sendSms(dtst.Rows[ij]["SR_PhNo"].ToString(), MSG, dt.Rows[0]["username"].ToString(), dt.Rows[0]["sendername"].ToString(), dt.Rows[0]["apikey"].ToString());

                    }
                    #region update
                    dtst.Columns.Remove("SR_StudentName");
                    dtst.Columns.Remove("SR_PhNo");
                    SqlParameter[] prmin = new SqlParameter[] {
                                           new SqlParameter("type", "InsertSMSManual"),
                                           new SqlParameter("dtAttendance", dtst)
                                                };
                    string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_AttendanceSave", prmin, CommandType.StoredProcedure));
                    #endregion
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region View
        public ActionResult ViewAttendance()
        {

            if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType.ToString() == "SuperAdmin")
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
                ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            }
            else
            {
                SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectAssignClass"),
                  new SqlParameter("UserID",  ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                  new SqlParameter("userType",  ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
            };
                ViewBag.ClassList = Utility.GetDropDownList("SP_EmpClassMapping", "ClassID", "ClassName", prm2, "", "", "Select");
            }
            SqlParameter[] prm3 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectMonth")
            };
            ViewBag.MonthList = Utility.GetDropDownList("SP_AttendanceSave", "ID", "MonthNameDesc", prm3, "", "", "Select");

            return View();
        }
        public ActionResult viewMonthAttendance(string MonthID, string ClassID, string SectionID)
        {
            try
            {
                object[] mixArray = new object[3];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("MonthText", MonthID),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SectionID", SectionID),
                };
                DataTable dt_User = new SQLHelper().ExecuteDataTable("pr_RptAttendence", prm1, CommandType.StoredProcedure);
                int count = 0;
                if (dt_User.Rows.Count > 0)
                {
                    string val_28 = dt_User.Rows[0][dt_User.Columns.Count - 4].ToString();
                    string val_29 = dt_User.Rows[0][dt_User.Columns.Count - 3].ToString();
                    string val_30 = dt_User.Rows[0][dt_User.Columns.Count - 2].ToString();
                    string val_31 = dt_User.Rows[0][dt_User.Columns.Count - 1].ToString();
                   
                    foreach (DataColumn column in dt_User.Columns)
                    {
                        if (column.ColumnName == "28" || column.ColumnName == "29" || column.ColumnName == "30" || column.ColumnName == "31")
                        {
                            if ((column.ColumnName == "28" && val_28 =="0") || (column.ColumnName == "29" && val_29 == "0") ||
                                (column.ColumnName == "30" && val_30 == "0") || (column.ColumnName == "31" && val_31 == "0"))
                            {
                                count = count + 1;
                            }
                            else
                            {
                                dt_User.Columns[column.ColumnName].ColumnName = "_" + column.ColumnName;
                            }
                        }
                        else
                        {
                            dt_User.Columns[column.ColumnName].ColumnName = "_" + column.ColumnName;
                        }
                    }
                }
                //dt_User.Columns[column.ColumnName].ColumnName = "_" + column.ColumnName;
                List<StudentReportModel> StudentList = Utility.ConvertDataTableToClassObjectList<StudentReportModel>(dt_User);



                mixArray[0] = dt_User.Columns.Count - (13+ count);
                mixArray[1] = 0;
                mixArray[2] = StudentList;
                return Json(mixArray, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewStudent()
        {
            SqlParameter[] prm3 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectMonth")
            };
            ViewBag.MonthList = Utility.GetDropDownList("SP_AttendanceSave", "ID", "MonthNameDesc", prm3, "", "", "Select");

            return View();
        }
        public ActionResult viewAttendanceStudent(string MonthID)
        {
            try
            {
                string UserID = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID;
                if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Parents")
                {
                    UserID = ((loggedInParents)System.Web.HttpContext.Current.Session["loggedInParents"]).UserID;
                }
                object[] mixArray = new object[3];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("MonthText", MonthID),
                    new SqlParameter("UserID",UserID),
                };
                DataTable dt_User = new SQLHelper().ExecuteDataTable("pr_RptAttendence_Parents", prm1, CommandType.StoredProcedure);
                mixArray[1] = dt_User.Rows[0]["PDays"].ToString();
                mixArray[2] = dt_User.Rows[0]["ADays"].ToString();
                #region List
                string days = dt_User.Rows[0]["DaysInMonth"].ToString();
                dt_User.Columns.Remove("DaysInMonth");
                dt_User.Columns.Remove("SunDayCount");
                dt_User.Columns.Remove("HolidayCount");
                dt_User.Columns.Remove("TotalWorkingDays");
                dt_User.Columns.Remove("PDays");
                dt_User.Columns.Remove("StudentName");
                dt_User.Columns.Remove("StudentCode");
                dt_User.Columns.Remove("ID");
                dt_User.Columns.Remove("StudentID");

                //foreach (DataColumn column in dt_User.Columns)
                //{
                //    dt_User.Columns[column.ColumnName].ColumnName = "_" + column.ColumnName;
                //}
                DataTable dtShow = new DataTable();
                dtShow.Columns.Add("start");
                dtShow.Columns.Add("end");
                dtShow.Columns.Add("summary");
                dtShow.Columns.Add("mask");
                DataRow dr;
                char[] whitespace = new char[] { ' ', '\t' };
                string[] mnyr = MonthID.Split(whitespace);
                //1
                if (dt_User.Rows[0]["1"].ToString() == "P" || dt_User.Rows[0]["1"].ToString() == "A" || dt_User.Rows[0]["1"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-01";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-01";
                    dr[2] = dt_User.Rows[0]["1"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //2
                if (dt_User.Rows[0]["2"].ToString() == "P" || dt_User.Rows[0]["2"].ToString() == "A" || dt_User.Rows[0]["2"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-02";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-02";
                    dr[2] = dt_User.Rows[0]["2"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //3
                if (dt_User.Rows[0]["3"].ToString() == "P" || dt_User.Rows[0]["3"].ToString() == "A" || dt_User.Rows[0]["3"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-03";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-03";
                    dr[2] = dt_User.Rows[0]["3"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //4
                if (dt_User.Rows[0]["4"].ToString() == "P" || dt_User.Rows[0]["4"].ToString() == "A" || dt_User.Rows[0]["4"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-04";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-04";
                    dr[2] = dt_User.Rows[0]["4"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //5
                if (dt_User.Rows[0]["5"].ToString() == "P" || dt_User.Rows[0]["5"].ToString() == "A" || dt_User.Rows[0]["5"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-05";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-05";
                    dr[2] = dt_User.Rows[0]["5"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //6
                if (dt_User.Rows[0]["6"].ToString() == "P" || dt_User.Rows[0]["6"].ToString() == "A" || dt_User.Rows[0]["6"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-06";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-06";
                    dr[2] = dt_User.Rows[0]["6"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //7
                if (dt_User.Rows[0]["7"].ToString() == "P" || dt_User.Rows[0]["7"].ToString() == "A" || dt_User.Rows[0]["7"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-07";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-07";
                    dr[2] = dt_User.Rows[0]["7"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //8
                if (dt_User.Rows[0]["8"].ToString() == "P" || dt_User.Rows[0]["8"].ToString() == "A" || dt_User.Rows[0]["8"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-08";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-08";
                    dr[2] = dt_User.Rows[0]["8"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //9
                if (dt_User.Rows[0]["9"].ToString() == "P" || dt_User.Rows[0]["9"].ToString() == "A" || dt_User.Rows[0]["9"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-09";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-09";
                    dr[2] = dt_User.Rows[0]["9"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //10
                if (dt_User.Rows[0]["10"].ToString() == "P" || dt_User.Rows[0]["10"].ToString() == "A" || dt_User.Rows[0]["10"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-10";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-10";
                    dr[2] = dt_User.Rows[0]["10"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //11
                if (dt_User.Rows[0]["11"].ToString() == "P" || dt_User.Rows[0]["11"].ToString() == "A" || dt_User.Rows[0]["11"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-11";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-11";
                    dr[2] = dt_User.Rows[0]["11"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //12
                if (dt_User.Rows[0]["12"].ToString() == "P" || dt_User.Rows[0]["12"].ToString() == "A" || dt_User.Rows[0]["12"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-12";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-12";
                    dr[2] = dt_User.Rows[0]["12"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //13
                if (dt_User.Rows[0]["13"].ToString() == "P" || dt_User.Rows[0]["13"].ToString() == "A" || dt_User.Rows[0]["13"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-13";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-13";
                    dr[2] = dt_User.Rows[0]["13"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //14
                if (dt_User.Rows[0]["14"].ToString() == "P" || dt_User.Rows[0]["14"].ToString() == "A" || dt_User.Rows[0]["14"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-14";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-14";
                    dr[2] = dt_User.Rows[0]["14"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //15
                if (dt_User.Rows[0]["15"].ToString() == "P" || dt_User.Rows[0]["15"].ToString() == "A" || dt_User.Rows[0]["15"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-15";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-15";
                    dr[2] = dt_User.Rows[0]["15"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //16
                if (dt_User.Rows[0]["16"].ToString() == "P" || dt_User.Rows[0]["16"].ToString() == "A" || dt_User.Rows[0]["16"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-16";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-16";
                    dr[2] = dt_User.Rows[0]["16"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //17
                if (dt_User.Rows[0]["17"].ToString() == "P" || dt_User.Rows[0]["17"].ToString() == "A" || dt_User.Rows[0]["17"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-17";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-17";
                    dr[2] = dt_User.Rows[0]["17"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //18
                if (dt_User.Rows[0]["18"].ToString() == "P" || dt_User.Rows[0]["18"].ToString() == "A" || dt_User.Rows[0]["18"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-18";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-18";
                    dr[2] = dt_User.Rows[0]["18"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //19
                if (dt_User.Rows[0]["19"].ToString() == "P" || dt_User.Rows[0]["19"].ToString() == "A" || dt_User.Rows[0]["19"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-19";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-19";
                    dr[2] = dt_User.Rows[0]["19"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //20
                if (dt_User.Rows[0]["20"].ToString() == "P" || dt_User.Rows[0]["20"].ToString() == "A" || dt_User.Rows[0]["20"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-20";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-20";
                    dr[2] = dt_User.Rows[0]["20"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //21
                if (dt_User.Rows[0]["21"].ToString() == "P" || dt_User.Rows[0]["21"].ToString() == "A" || dt_User.Rows[0]["21"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-21";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-21";
                    dr[2] = dt_User.Rows[0]["21"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //22
                if (dt_User.Rows[0]["22"].ToString() == "P" || dt_User.Rows[0]["22"].ToString() == "A" || dt_User.Rows[0]["22"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-22";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-22";
                    dr[2] = dt_User.Rows[0]["22"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //23
                if (dt_User.Rows[0]["23"].ToString() == "P" || dt_User.Rows[0]["23"].ToString() == "A" || dt_User.Rows[0]["23"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-23";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-23";
                    dr[2] = dt_User.Rows[0]["23"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //24
                if (dt_User.Rows[0]["24"].ToString() == "P" || dt_User.Rows[0]["24"].ToString() == "A" || dt_User.Rows[0]["24"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-24";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-24";
                    dr[2] = dt_User.Rows[0]["24"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //25
                if (dt_User.Rows[0]["25"].ToString() == "P" || dt_User.Rows[0]["25"].ToString() == "A" || dt_User.Rows[0]["25"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-25";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-25";
                    dr[2] = dt_User.Rows[0]["25"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //26
                if (dt_User.Rows[0]["26"].ToString() == "P" || dt_User.Rows[0]["26"].ToString() == "A" || dt_User.Rows[0]["26"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-26";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-26";
                    dr[2] = dt_User.Rows[0]["26"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //27
                if (dt_User.Rows[0]["27"].ToString() == "P" || dt_User.Rows[0]["27"].ToString() == "A" || dt_User.Rows[0]["27"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-27";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-27";
                    dr[2] = dt_User.Rows[0]["27"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                //28
                if (dt_User.Rows[0]["28"].ToString() == "P" || dt_User.Rows[0]["28"].ToString() == "A" || dt_User.Rows[0]["28"].ToString() == "H")
                {
                    dr = dtShow.NewRow();
                    dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-28";
                    dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-28";
                    dr[2] = dt_User.Rows[0]["28"].ToString();
                    dr[3] = false;
                    dtShow.Rows.Add(dr);
                }
                if (days == "29")
                {
                    if (dt_User.Rows[0]["29"].ToString() == "P" || dt_User.Rows[0]["29"].ToString() == "A" || dt_User.Rows[0]["29"].ToString() == "H")
                    {
                        dr = dtShow.NewRow();
                        dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-29";
                        dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-29";
                        dr[2] = dt_User.Rows[0]["29"].ToString();
                        dr[3] = false;
                        dtShow.Rows.Add(dr);
                    }
                }
                if (days == "30")
                {
                    if (dt_User.Rows[0]["30"].ToString() == "P" || dt_User.Rows[0]["30"].ToString() == "A" || dt_User.Rows[0]["30"].ToString() == "H")
                    {
                        dr = dtShow.NewRow();
                        dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-30";
                        dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-30";
                        dr[2] = dt_User.Rows[0]["30"].ToString();
                        dr[3] = false;
                        dtShow.Rows.Add(dr);
                    }
                }
                if (days == "31")
                {
                    if (dt_User.Rows[0]["31"].ToString() == "P" || dt_User.Rows[0]["31"].ToString() == "A" || dt_User.Rows[0]["31"].ToString() == "H")
                    {
                        dr = dtShow.NewRow();
                        dr[0] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-31";
                        dr[1] = mnyr[1].ToString() + "-" + GetID(mnyr[0].ToString()) + "-31";
                        dr[2] = dt_User.Rows[0]["31"].ToString();
                        dr[3] = false;
                        dtShow.Rows.Add(dr);
                    }
                }

                #endregion  List

                List<StudentCalenderModel> StudentList = Utility.ConvertDataTableToClassObjectList<StudentCalenderModel>(dtShow);
                mixArray[0] = StudentList;
                return Json(mixArray, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public string GetID(string txt)
        {
            int month = DateTime.ParseExact(txt, "MMMM", CultureInfo.InvariantCulture).Month;
            string strmonth = Convert.ToString(month);
            if (strmonth.Length == 1)
            {
                strmonth = "0" + strmonth;
            }
            return strmonth;
        }
        #endregion  

    }
}