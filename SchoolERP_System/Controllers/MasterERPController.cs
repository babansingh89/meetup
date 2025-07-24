using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using SchoolERP_System.Models;
using SchoolERP_System.Helper;

namespace SchoolERP_System.Controllers
{
    [SessionAuthorizedAttribute]
    public class MasterERPController : Controller
    {
        #region SessionMaster
        public ActionResult SessionMaster()
        {
            return View();
        }

        public ActionResult saveSessionMaster(string Id, string FromDate, string ToDate)
        {
            try
            {
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SessionID", Id),
                    new SqlParameter("FromDate", FromDate),
                    new SqlParameter("ToDate", ToDate)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Session", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewSessionMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SessionID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Session", prm1, CommandType.StoredProcedure);
                List<Session> list = Utility.ConvertDataTableToClassObjectList<Session>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteSessionMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("SessionID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Session", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SetDefaultSession(string SessionID)
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@SessionID", SessionID)
            };
            string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_UpdateSessionStatus", prm1, CommandType.StoredProcedure));
            return Json(Output, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ClassMaster
        public ActionResult ClassMaster()
        {
            return View();
        }

        public ActionResult saveClassMaster(string ClassId, string ClassName, string ClassStrength)
        {
            try
            {
                string Type = "";
                if (ClassId == "" || ClassId == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ClassID", ClassId),
                    new SqlParameter("ClassName", ClassName),
                    new SqlParameter("ClassStrength", ClassStrength)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Class", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewClassMaster(string ClassId, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ClassId", ClassId),
                };
                DataTable dt_Class = new SQLHelper().ExecuteDataTable("SP_Class", prm1, CommandType.StoredProcedure);
                List<ClassMstModels> classList = Utility.ConvertDataTableToClassObjectList<ClassMstModels>(dt_Class);
                return Json(classList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteClassMaster(string ClassId)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ClassId", ClassId)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Class", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region SectionMaster
        public ActionResult SectionMaster()
        {
            return View();
        }

        public ActionResult saveSectionMaster(string Id, string SectionName)
        {
            try
            {
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SectionID", Id),
                    new SqlParameter("SectionName", SectionName)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Section", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewSectionMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SectionID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Section", prm1, CommandType.StoredProcedure);
                List<Section> list = Utility.ConvertDataTableToClassObjectList<Section>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteSectionMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("SectionID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Section", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ClassSectionMapping
        public ActionResult ClassSectionMapping()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.SectionList = Utility.GetDropDownList("SP_Section", "SectionID", "SectionName", prm2, "", "", "Select");

            return View();
        }

        public ActionResult saveClassSectionMapping(string Id, string SectionID, string ClassID)
        {
            try
            {
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SectionID", SectionID),
                    new SqlParameter("ClassID", ClassID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ClassSectionMapping", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewClassSectionMappingr(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ClassID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ClassSectionMapping", prm1, CommandType.StoredProcedure);
                List<CS> list = Utility.ConvertDataTableToClassObjectList<CS>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteClassSectionMapping(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ClassID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ClassSectionMapping", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region StreamMaster
        public ActionResult StreamMaster()
        {
            return View();
        }

        public ActionResult saveStreamMaster(string Id, string StreamName)
        {
            try
            {
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("StreamID", Id),
                    new SqlParameter("StreamName", StreamName)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Stream", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewStreamMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("StreamID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Stream", prm1, CommandType.StoredProcedure);
                List<Streamcl> list = Utility.ConvertDataTableToClassObjectList<Streamcl>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteStreamMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("StreamID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Stream", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region RouteMaster
        public ActionResult RouteMaster()
        {
            return View();
        }

        public ActionResult saveRouteMaster(string Id, string RouteName)
        {
            try
            {
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("RouteID", Id),
                    new SqlParameter("RouteName", RouteName)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Route", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewRouteMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("RouteID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Route", prm1, CommandType.StoredProcedure);
                List<Route> list = Utility.ConvertDataTableToClassObjectList<Route>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteRouteMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("RouteID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Route", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ClassStreamMapping
        public ActionResult ClassStreamMapping()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.StreamList = Utility.GetDropDownList("SP_Stream", "StreamID", "StreamName", prm2, "", "", "Select");

            return View();
        }

        public ActionResult saveClassStreamMapping(string Id, string StreamID, string ClassID)
        {
            try
            {
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("StreamID", StreamID),
                    new SqlParameter("ClassID", ClassID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ClassStreamMapping", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewClassStreamMappingr(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ClassID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ClassStreamMapping", prm1, CommandType.StoredProcedure);
                List<CSt> list = Utility.ConvertDataTableToClassObjectList<CSt>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteClassStreamMapping(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ClassID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ClassStreamMapping", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region HolidayMaster
        public ActionResult HolidayMaster()
        {
            return View();
        }

        public ActionResult saveHolidayMaster(string Id, string FromDate, string ToDate, string Reason)
        {
            try
            {
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("HolidayID ", Id),
                    new SqlParameter("Reason ", Reason),
                    new SqlParameter("FromDate", FromDate),
                    new SqlParameter("ToDate", ToDate)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Holiday", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewHolidayMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("HolidayID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Holiday", prm1, CommandType.StoredProcedure);
                List<Holiday> list = Utility.ConvertDataTableToClassObjectList<Holiday>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteHolidayMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("HolidayID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Holiday", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Desgination
        public ActionResult Desgination()
        {
            return View();
        }

        public ActionResult saveDesginationMaster(string DesID, string DesName, string DesCode)
        {
            try
            {
                string Type = "";
                if (DesID == "" || DesID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("DesID", DesID),
                    new SqlParameter("DesName", DesName),
                    new SqlParameter("DesCode", DesCode)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Desgination", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewDesginationMaster(string DesID, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("DesID", DesID),
                };
                DataTable dt_Class = new SQLHelper().ExecuteDataTable("SP_Desgination", prm1, CommandType.StoredProcedure);
                List<Desgination> List = Utility.ConvertDataTableToClassObjectList<Desgination>(dt_Class);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteDesginationMaster(string DesID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("DesID", DesID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Desgination", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Shift
        public ActionResult Shift()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            return View();
        }

        public ActionResult saveShiftMaster(string ShiftID, string ClassID, string ShiftFrom, string ShiftTo)
        {
            try
            {
                string Type = "";
                if (ShiftID == "" || ShiftID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ShiftID", ShiftID),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("ShiftFrom", ShiftFrom),
                    new SqlParameter("ShiftTo", ShiftTo)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Shift", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewShiftMaster(string ShiftID, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ShiftID", ShiftID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Shift", prm1, CommandType.StoredProcedure);
                List<Shift> List = Utility.ConvertDataTableToClassObjectList<Shift>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteShiftMaster(string ShiftID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ShiftID", ShiftID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Shift", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region EventsMaster
        public ActionResult EventsMaster()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "All");

            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select_Designation"),
            };
            ViewBag.DesignationList = Utility.GetDropDownList("SP_Employee", "DesID", "DesName", prm2, "", "", "Select");


            return View();
        }

        public ActionResult saveEventsMaster(string Id, string ViewTo, string ClassID, string NTitle, string NDate, string ToDate, string NDescription)
        { 
            try
            {
                if (ViewTo.ToString() != "Student" && ViewTo.ToString() != "All")
                    ClassID = null;
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("NID", Id),
                    new SqlParameter("ViewTo", ViewTo),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("NTitle", NTitle),
                    new SqlParameter("NDate", NDate),
                    new SqlParameter("TDate", ToDate),
                    new SqlParameter("NDescription", NDescription)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Notice", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewEventsMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("NID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Notice", prm1, CommandType.StoredProcedure);
                List<Notice> list = Utility.ConvertDataTableToClassObjectList<Notice>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteEventsMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("NID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Notice", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region View 
        public ActionResult ViewHoliday()
        {
            return View();
        }
        public ActionResult ViewStaff()
        {
            return View();
        }
        public ActionResult ViewStudent()
        {
            return View();
        }
        public ActionResult ViewRoutine()
        {
            return View();
        }
        public ActionResult viewRoutineForTeacher(string Type)
        {
            try
            {
                string UserID = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID;
                string UserType = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType;
                if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Student")
                {
                    Type = "SelectByStudent";
                }
                else if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Parents")
                {
                    UserID = ((loggedInParents)System.Web.HttpContext.Current.Session["loggedInParents"]).UserID;
                    UserType = "Student"; Type = "SelectByStudent";
                }
                SqlParameter[] prm1 = new SqlParameter[] {
                 new SqlParameter("Type", Type),
                new SqlParameter("UserID", UserID),
                new SqlParameter("UserType",UserType ),

                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_RoutineView", prm1, CommandType.StoredProcedure);
                List<ClassRotineModel> List = Utility.ConvertDataTableToClassObjectList<ClassRotineModel>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewResult()
        {
            return View();
        }
        public ActionResult ViewSubjectSyllabus()
        {
            return View();
        }
        public ActionResult ExamTimetable()
        {
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ExamList = Utility.GetDropDownList("SP_Exam", "ExamID", "ExamName", prm2, "", "", "Select");
            return View();
        }
        #endregion
    }
}