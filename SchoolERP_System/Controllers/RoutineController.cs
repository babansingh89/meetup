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
using NReco.PdfGenerator;


namespace SchoolERP_System.Controllers
{
    [SessionAuthorizedAttribute]
    public class RoutineController : Controller
    {
        #region Add Routine

        public ActionResult AddRoutine()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");

            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.SubjectList = Utility.GetDropDownList("SP_Subject", "SubjectID", "SubjectName", prm2, "", "", "Select");

            SqlParameter[] prm3 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectDropdown"),
            };
            ViewBag.TeacherList = Utility.GetDropDownList("SP_Employee", "EM_EmpId", "EM_EmpName", prm3, "", "", "Select");


            return View();
        }

        public ActionResult saveRoutine(string Id, string ClassID, string SectionID, string SubjectID, string TeacherID, string Days
                                        , string StHr, string StMn, string EnHr, string EnMn)
        {
            try
            {

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Id == "0" ? "Insert": "Update"),
                    new SqlParameter("RID", Id),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SectionID", SectionID),
                    new SqlParameter("SubjectID", SubjectID),
                    new SqlParameter("TeacherID", TeacherID),
                    new SqlParameter("Days", Days),
                    new SqlParameter("StHr", StHr),
                    new SqlParameter("StMn", StMn),
                    new SqlParameter("EnHr", EnHr),
                    new SqlParameter("EnMn", EnMn),
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Routine", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewRoutine(string Type, string ClassID, string SectionID, string TeacherID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SectionID", SectionID),
                    new SqlParameter("TeacherID", TeacherID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Routine", prm1, CommandType.StoredProcedure);
                List<ClassRotineModel> List = Utility.ConvertDataTableToClassObjectList<ClassRotineModel>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewRoutineEDIT(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectByID"),
                    new SqlParameter("RID", Id)
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Routine", prm1, CommandType.StoredProcedure);
                List<ClassRotineModelEdit> List = Utility.ConvertDataTableToClassObjectList<ClassRotineModelEdit>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteRoutine(string Id)
        {
            try
            {

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("RID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Routine", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Routine For Teacher
        public ActionResult ViewTeacherRoutine()
        {
            SqlParameter[] prm3 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectDropdown"),
            };
            ViewBag.TeacherList = Utility.GetDropDownList("SP_Employee", "EM_EmpId", "EM_EmpName", prm3, "", "", "Select");
            return View();
        }
        public ActionResult viewRoutineForTeacher(string Type, string TeacherID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("TeacherID", TeacherID)
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Routine", prm1, CommandType.StoredProcedure);
                List<ClassRotineModel> List = Utility.ConvertDataTableToClassObjectList<ClassRotineModel>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}