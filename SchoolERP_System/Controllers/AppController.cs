using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using SchoolERP_System.Models;
using SchoolERP_System.Helper;
using System.IO;
using System.Text;
using SchoolERP_System.Models;
using System.Threading.Tasks;


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
            string fileName= string.Empty;
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;
                if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(email))
                {
                    return Json(new { Output = "fail", Data = "", Message = "Mobile and Email are required." });
                }

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                if(profileImage !=null)
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
        public ActionResult Student_Communication(string ClassID,string SectionID, string PostType, string AppID, string Content, List<HttpPostedFileBase> images = null, List<HttpPostedFileBase> files = null)
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
        public ActionResult ViewStudentPost(string UserType, string SR_ID, string PostMode, string AppID)
        {
            try
            {
                smc.AppID = AppID;
                System.Web.HttpContext.Current.Session["loggedInAdmin"] = smc;
                List<EditPostModel> List = new List<EditPostModel>();
                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", UserType),
                    new SqlParameter("SR_ID",SR_ID),
                    new SqlParameter("PostMode",PostMode),
                };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_Post_App", prm1, CommandType.StoredProcedure);
                if(dt.Tables.Count > 0)
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
                //List<SelectListItem> ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");

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

                return Json(new { Output = "success", IsSuccess = Cnt > 0  ? "Y" : "N", Message = Cnt > 0 ? "Data inserted." : "Data insertation failed." });
            }
            catch (Exception ex)
            {
                return Json(new { Output = "fail", IsSuccess = "N", Message = "Some error found." });
            }
        }

        #endregion
    }

}