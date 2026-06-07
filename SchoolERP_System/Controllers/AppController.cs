using Newtonsoft.Json;
using Razorpay.Api;
using SchoolERP_System.Helper;
using SchoolERP_System.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Razor.Configuration;


namespace SchoolERP_System.Controllers
{
    public class AppController : Controller
    {
        loggedInAdmin smc = new loggedInAdmin();

        #region LOGIN
        [HttpPost]
        public ActionResult UploadProfile(string mobile, string email, string SR_ID, string AppID, HttpPostedFileBase profileImage)
        {
            loggedInAdmin smc = new loggedInAdmin();
            string fileName = string.Empty;
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;
                if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(email))
                {
                    return Json(new { Output = "fail", Data = "", Message = "Mobile and Email are required." });
                }

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                if (profileImage != null)
                {
                    string extension = Path.GetExtension(profileImage.FileName);
                    fileName = $"{timestamp}{extension}";
                }


                SqlParameter[] prm1 = new SqlParameter[] {
                                    new SqlParameter("Type", "Profile"),
                                    new SqlParameter("SR_Email",email),
                                    new SqlParameter("SR_Ph",mobile),
                                    new SqlParameter("SR_ID",SR_ID),
                                    new SqlParameter("SR_Pic",fileName)
                                };
                DataTable dtdtl = new SQLHelper().ExecuteDataTable("SP_Update_App", prm1, CommandType.StoredProcedure);
                if (dtdtl.Rows.Count > 0)
                {
                    string Output = Convert.ToString(dtdtl.Rows[0]["output"]);
                    if (Output == "success")
                    {
                        if (profileImage != null && profileImage.ContentLength > 0)
                        {
                            string filePath = Path.Combine(Server.MapPath("~/UploadedImage"), fileName);
                            profileImage.SaveAs(filePath);
                        }
                        return Json(new { Output = "success", Data = "", Message = "Profile updated successfully." });
                    }
                    else
                    {
                        return Json(new { Output = "fail", Data = "", Message = "Some error found" });
                    }
                }
                else
                {
                    return Json(new { Output = "fail", Data = "", Message = "No data found" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error found" });
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(string Type, string UserType, string UserID, string OldPassword, string NewPassword, string AppID)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("UserType", UserType),
                    new SqlParameter("UserID", UserID),
                    new SqlParameter("OldPassword",OldPassword),
                    new SqlParameter("NewPassword", NewPassword),
                    new SqlParameter("AppID", AppID),
                };
                DataSet ds = new SQLHelper().ExecuteDataSet("SP_MasterLogin_App", prm1, CommandType.StoredProcedure);
                return Json(new { Output = "success", Data = ds.Tables[0].Rows[0]["Result"], Message = "data found" });
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error found." });
            }
        }
        #endregion LOGIN

        #region STUDENT
        public ActionResult ViewMonthAttendance(string MonthID, string ClassID, string SectionID, string StudentID, string AppID)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                object[] mixArray = new object[3];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("MonthText", MonthID),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SectionID", SectionID),
                    new SqlParameter("StudentID", StudentID),
                };
                DataTable dt_User = new SQLHelper().ExecuteDataTable("pr_RptAttendence_App", prm1, CommandType.StoredProcedure);
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
                            if ((column.ColumnName == "28" && val_28 == "0") || (column.ColumnName == "29" && val_29 == "0") ||
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
                    List<StudentReportModel> StudentList = Utility.ConvertDataTableToClassObjectList<StudentReportModel>(dt_User);
                    return Json(new { Output = "success", Data = StudentList, Message = "Data found." });
                }
                else
                {
                    return Json(new { Output = "fail", Data = "", Message = "No data found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error found." });
            }
        }

        [HttpPost]
        public ActionResult ViewNotification(string UserType, string ClassID, string AppID)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("UserType", UserType),
                    new SqlParameter("ClassID",ClassID),
                };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_Notice_App", prm1, CommandType.StoredProcedure);
                List<Notice> listNotice = Utility.ConvertDataTableToClassObjectList<Notice>(dt.Tables[0]);
                return Json(new { Output = "success", Data = listNotice, Message = "data found" });
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error found." });
            }
        }
        [HttpPost]
        public ActionResult Student_Communication(string ClassID, string SectionID, string PostType, string UserType, string UserID, string AppID, string Content,
            List<HttpPostedFileBase> images = null, List<HttpPostedFileBase> files = null)
        {
            loggedInAdmin smc = new loggedInAdmin();
            string fileName = string.Empty;
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                string decodedHtml = Encoding.UTF8.GetString(Convert.FromBase64String(Content));
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Insert"),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SectionID", SectionID),
                    new SqlParameter("PostMode", PostType),
                    new SqlParameter("UserType", UserType),
                    new SqlParameter("UserID", UserID),
                    new SqlParameter("PostData", decodedHtml),
                                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Post_App", prm1, CommandType.StoredProcedure);
                if (dt.Rows.Count > 0)
                {
                    string Output = Convert.ToString(dt.Rows[0]["output"]);
                    if (Output == "success")
                    {
                        if (images != null)
                        {
                            if (images.Count > 0)
                            {
                                foreach (var file in images)
                                {
                                    fileName = file.FileName;
                                    string filePath = Path.Combine(Server.MapPath("~/ImagesPost"), fileName);
                                    file.SaveAs(filePath);
                                }
                            }
                        }

                        if (files != null)
                        {
                            if (files.Count > 0)
                            {
                                foreach (var file in files)
                                {
                                    fileName = file.FileName;
                                    string filePath = Path.Combine(Server.MapPath("~/DocPost"), fileName);
                                    file.SaveAs(filePath);
                                }
                            }
                        }

                        return Json(new { Output = "success", Data = "", Message = "Post inserted successfully." });
                    }
                    else
                    {
                        return Json(new { Output = "fail", Data = "3", Message = "Data not inserted." });
                    }
                }
                else
                {
                    return Json(new { Output = "fail", Data = "2", Message = "No data found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = ex.Message, Message = "Some error found" });
            }
        }
        [HttpPost]
        public ActionResult ViewStudentPost(string Type, string UserType, string SR_ID, string UserID, string PostMode, string AppID)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;
                List<EditPostModel> List = new List<EditPostModel>();
                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("UserType", UserType),
                    new SqlParameter("SR_ID",SR_ID),
                    new SqlParameter("UserID",UserID),
                    new SqlParameter("PostMode",PostMode),
                };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_Post_App", prm1, CommandType.StoredProcedure);
                if (dt.Tables.Count > 0)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        List = Utility.ConvertDataTableToClassObjectList<EditPostModel>(dt.Tables[0]);
                    }
                }

                return Json(new { Output = "success", Data = List, Message = "Data found." });
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error found." });
            }
        }
        #endregion STUDENT

        #region TEACHER
        public ActionResult ViewEmpMonthAttendance(string MonthID, string EmpID, string AppID)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                object[] mixArray = new object[3];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("MonthText", MonthID),
                    new SqlParameter("EMEmpId", EmpID),
                };
                DataTable dt_User = new SQLHelper().ExecuteDataTable("pr_RptEmpAttendence_App", prm1, CommandType.StoredProcedure);
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
                            if ((column.ColumnName == "28" && val_28 == "0") || (column.ColumnName == "29" && val_29 == "0") ||
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
                    List<TeacherReportModel> StudentList = Utility.ConvertDataTableToClassObjectList<TeacherReportModel>(dt_User);
                    return Json(new { Output = "success", Data = StudentList, Message = "Data found." });
                }
                else
                {
                    return Json(new { Output = "fail", Data = "", Message = "No data found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error found." });
            }
        }

        [HttpPost]
        public ActionResult Get_Class(string AppID)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@type", "Select"),
            };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_Class", prm1, CommandType.StoredProcedure);

                List<ClassMstModels> ClassList = Utility.ConvertDataTableToClassObjectList<ClassMstModels>(dt.Tables[0]);
                return Json(new { Output = "success", Data = ClassList, Message = "Data found." });
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error found." });
            }
        }

        [HttpPost]
        public ActionResult Get_Section(string AppID, string ClassID)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("Type", "SelectSectionStream"),
                    new SqlParameter("SR_ClassID", ClassID),
            };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_StudentRegistration", prm1, CommandType.StoredProcedure);
                List<Section> ListSection = Utility.ConvertDataTableToClassObjectList<Section>(dt.Tables[1]);

                return Json(new { Output = "success", Data = ListSection, Message = "Data found." });
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error found." });
            }
        }

        [HttpPost]
        public ActionResult Get_TeacherWiseClass(string AppID, string EmpID)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@type", "TeacherWiseClass"),
                  new SqlParameter("@EmpID", EmpID),
            };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_Class", prm1, CommandType.StoredProcedure);

                List<ClassMstModels> ClassList = Utility.ConvertDataTableToClassObjectList<ClassMstModels>(dt.Tables[0]);
                return Json(new { Output = "success", Data = ClassList, Message = "Data found." });
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error found." });
            }
        }

        [HttpPost]
        public ActionResult Get_TeacherClassWiseSection(string AppID, string EmpID, string ClassID)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("Type", "TeacherClassWiseSection"),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("EmpID", EmpID),
            };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_Class", prm1, CommandType.StoredProcedure);
                if(dt.Tables.Count >0)
                {
                    if(dt.Tables[0].Rows.Count >0)
                    {
                        List<Section> ListSection = Utility.ConvertDataTableToClassObjectList<Section>(dt.Tables[0]);
                        return Json(new { Output = "success", Data = ListSection, Message = "Data found." });
                    }
                    else
                    {
                        return Json(new { Output = "fail", Data = "", Message = "No section found." });
                    }
                }
                else
                {
                    return Json(new { Output = "fail", Data = "", Message = "No section found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error found." });
            }
        }

        [HttpPost]
        public ActionResult DeleteTeacherPost(string Type, string PostID,  string UserID, string AppID)
        {
            loggedInAdmin smc = new loggedInAdmin();
            string fileName = string.Empty;
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("PostID", PostID),
                    new SqlParameter("UserID", UserID),
                                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Post_App", prm1, CommandType.StoredProcedure);
                if (dt.Rows.Count > 0)
                {
                    string Output = Convert.ToString(dt.Rows[0]["output"]);
                    if (Output == "deleteSuccessful")
                    {
                        return Json(new { Output = "success", Data = "success", Message = "Post deleted successfully." });
                    }
                    else
                    {
                        return Json(new { Output = "fail", Data = "fail", Message = "Post not deleted" });
                    }
                }
                else
                {
                    return Json(new { Output = "fail", Data = "fail", Message = "No data found" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = ex.Message, Message = "Some error found" });
            }
        }

        [HttpPost]
        public ActionResult Get_AttendanceNotification(AppRequest request)
        {
            DataSet ds = null;
            try
            {
                smc.AppID = request.AppId;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                SqlParameter[] prm1 = new SqlParameter[] {
            new SqlParameter("From", request.From),
            new SqlParameter("To", request.To),
            new SqlParameter("EmpID", request.UserId),
            new SqlParameter("AppID", request.AppId),
        };

                ds = (request.UserType == "Teacher")
                    ? new SQLHelper().ExecuteDataSet("pr_RptEMPPrint", prm1, CommandType.StoredProcedure)
                    : new SQLHelper().ExecuteDataSet("pr_RptSTUPrint", prm1, CommandType.StoredProcedure);

                if (ds == null || ds.Tables.Count == 0)
                    return Json(new { Output = "success", Data = new List<object>(), Message = "No data found." }, JsonRequestBehavior.AllowGet);

                var data = ds.Tables[0].AsEnumerable()
                    .Select(row => ds.Tables[0].Columns.Cast<DataColumn>()
                    .ToDictionary(col => col.ColumnName, col => row[col] == DBNull.Value ? null : row[col]))
                    .ToList();

                return Json(new { Output = "success", Data = data, Message = "Data found." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error found: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult viewStudentForAttendance(string ClassID, string SectionID, string AppID)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectStudent"),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SectionID", SectionID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_AttendanceSave", prm1, CommandType.StoredProcedure);
                if (dt.Columns.Contains("ErrorMsg"))
                {
                    string errorMsg = dt.Rows[0]["ErrorMsg"].ToString();
                    return Json(new { Output = "error", Data = "", Message = errorMsg });
                }
                else
                {
                    List<SelectStudent> STList = Utility.ConvertDataTableToClassObjectList<SelectStudent>(dt);
                    if (STList.Count > 0)
                    {
                        return Json(new { Output = "success", Data = STList, Message = "Record Found!" });
                    }
                    else
                    {
                        return Json(new { Output = "fail", Data = STList, Message = "No Record Found!" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult saveStudentAttendance(string ClassID, string SectionID, string AppID, List<AttendanceSave> strStudent)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;
                if (strStudent.Count > 0)
                {
                    DataTable dttAdr = GetAttdanceTable(strStudent);
                    SqlParameter[] prm1 = new SqlParameter[] {
                      new SqlParameter("@type", "Insert"),
                      new SqlParameter("@ClassID", ClassID),
                      new SqlParameter("@SectionID", SectionID),
                      new SqlParameter("@dtAttendance", dttAdr),
                      new SqlParameter("CreatedBy", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID)

                     };
                    var Output = new SQLHelper().ExecuteScalar("SP_AttendanceSave", prm1, CommandType.StoredProcedure);
                    return Json(new { Output = "success", Data = "", Message = "Data updated!" });
                }
                else
                {
                    return Json(new { Output = "fail", Data = "", Message = "Student list is blank!" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", Data = "", Message = "Some error occured!" });
            }
        }

        #endregion TEACHER

        #region FACE DEDUCTOR
        [HttpPost]
        public async Task<ActionResult> GetFaceIdentification(FaceIdentification faceIdentification)
        {
            try
            {
                smc.AppID = "18";
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;
                SqlParameter[] prm = new SqlParameter[]
                {
                    new SqlParameter("DeviceID", faceIdentification.deviceID),
                    new SqlParameter("DeviceSerialNo", faceIdentification.deviceSerialno),
                    new SqlParameter("EmployeeID", faceIdentification.employeeID),
                    new SqlParameter("PunchDate", faceIdentification.date),
                    new SqlParameter("ModeofPunch", faceIdentification.modeofPunch),
                    new SqlParameter("ModeofAttn", faceIdentification.modeofAttn),
                    new SqlParameter("PunchTime", faceIdentification.time),
                    new SqlParameter("IP", faceIdentification.ip),
                };
                int Cnt = new SQLHelper().ExecuteNonQuery("sp_FaceIdentification", prm, CommandType.StoredProcedure);

                return Json(new { isSuccess = Cnt > 0 ? "Y" : "N", outputMessage = Cnt > 0 ? "Added Successfuly" : "Data insertation failed" });
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = "N", outputMessage = "Some error found" });
            }
        }

        #endregion

        #region MEETUP APP
        [HttpPost]
        public async Task<ActionResult> InsertUpdateFCMToken(FcmTokenRequest request)
        {
            try
            {
                smc.AppID = request.AppId;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Token))
                    return Json(new { isSuccess = "fail", outputMessage = "UserId and Token are required" });

                SqlParameter[] prm = new SqlParameter[]
                {
                    new SqlParameter("UserId", request.UserId),
                    new SqlParameter("AppId", request.AppId),
                    new SqlParameter("FCMToken", request.Token),
                    new SqlParameter("DeviceType", request.DeviceType),
                    new SqlParameter("IsEnabled", request.IsEnabled),
                };
                int Cnt = new SQLHelper().ExecuteNonQuery("pr_SaveFCMToken_App", prm, CommandType.StoredProcedure);

                return Json(new { isSuccess = "success", outputMessage = "Token saved successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = "fail", outputMessage = "Some error found" });
            }
        }
       
        [HttpPost]
        public async Task<ActionResult> GetFCMToken(FcmTokenRequest request)
        {
            try
            {
                smc.AppID = request.AppId;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;

                if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Token))
                    return Json(new { isSuccess = "fail", outputMessage = "UserId and Token are required" });

                SqlParameter[] prm = new SqlParameter[]
                {
            new SqlParameter("UserId", request.UserId),
            new SqlParameter("AppId", request.AppId),
            new SqlParameter("FCMToken", request.Token),
                };

                // Execute stored procedure
                DataTable dt_User = new SQLHelper().ExecuteDataTable("pr_GetFCMToken_App", prm, CommandType.StoredProcedure);

                // Convert DataTable to List of Dictionaries (JSON-serializable)
                var data = dt_User.AsEnumerable()
                    .Select(row => dt_User.Columns.Cast<DataColumn>()
                        .ToDictionary(col => col.ColumnName, col => row[col] != DBNull.Value ? row[col] : null))
                    .ToList();

                // Return JSON directly
                return Json(new { isSuccess = "success", Data = data, outputMessage = "Data found." });
            }
            catch (Exception ex)
            {
                // Optionally log ex.Message somewhere
                return Json(new { isSuccess = "fail", Data = new List<object>(), outputMessage = "Some error occurred." });
            }
        }


        #endregion

        public DataTable GetAttdanceTable(List<AttendanceSave> stddt)
        {
            GenLib objLib = new GenLib();
            DataTable dt = new DataTable();
            dt = objLib.ToDataTable(stddt);
            return dt;
        }
    }

}