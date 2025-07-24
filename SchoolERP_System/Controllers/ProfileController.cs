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

namespace SchoolERP_System.Controllers
{
    [SessionAuthorizedAttribute]
    public class ProfileController : Controller
    {
        #region Teacher
        public ActionResult TeacherProfile()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ddlDesignationList = Utility.GetDropDownList("SP_Desgination", "DesID", "DesName", prm1, "", "", "Select");
            return View();
        }
        #endregion

        #region Parents
        public ActionResult ParentsProfile()
        {
            return View();
        }
        public ActionResult SaveParents()
        {
            try
            {
                string P_EmpId = Request.Form.Get("P_EmpId");
                string P_EmpName = Request.Form.Get("P_EmpName");
                string P_DOB = Request.Form.Get("P_DOB");
                string P_Gender = Request.Form.Get("P_Gender");
                string P_EmpFathers = Request.Form.Get("P_EmpFathers");
                string P_EmpAddress = Request.Form.Get("P_EmpAddress");
                string P_Email = Request.Form.Get("P_Email");
                string P_Password = Request.Form.Get("P_Password");
                string P_PanNo = Request.Form.Get("P_PanNo");
                string P_AdharNo = Request.Form.Get("P_AdharNo");
                var P_ProfilePic = System.Web.HttpContext.Current.Request.Files["P_ProfilePic"];
                string imgpath = null;
                string Type = "UpdateProfile";

                SqlParameter[] prm1 = new SqlParameter[] {
                new SqlParameter("Type", Type),
                new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                new SqlParameter("UserType", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
                new SqlParameter("P_EmpContactNo", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                new SqlParameter("P_EmpId",P_EmpId),
                new SqlParameter("P_EmpName",P_EmpName),
                new SqlParameter("P_DOB",P_DOB),
                new SqlParameter("P_Gender",P_Gender),
                new SqlParameter("P_EmpFathers",P_EmpFathers),
                new SqlParameter("P_EmpAddress",P_EmpAddress),
                new SqlParameter("P_Email",P_Email),
                new SqlParameter("P_PanNo",P_PanNo),
                new SqlParameter("P_AdharNo",P_AdharNo),
                new SqlParameter("UserPassword",P_Password),
                new SqlParameter("P_ProfilePic", imgpath)
                      };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Parents", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ViewParents(string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                    new SqlParameter("UserType", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
                    new SqlParameter("P_EmpContactNo", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Parents", prm1, CommandType.StoredProcedure);
                List<ParentsModel> List = Utility.ConvertDataTableToClassObjectList<ParentsModel>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ParentsSettings()
        {
            return View();
        }
        public ActionResult ViewParentsStudent(string Type)
        {
            try
            {
                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_StudentMarksStudent", prm1, CommandType.StoredProcedure);
                List<ParenrsStudent> List = Utility.ConvertDataTableToClassObjectList<ParenrsStudent>(dt);
              
                mixArray[1] = List;
                return Json(mixArray, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ResetDefalut(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "ResetLogin"),
                       new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                    new SqlParameter("SR_ID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_StudentMarksStudent", prm1, CommandType.StoredProcedure));
                if (Output.ToString() == "Yes")
                {
                    SqlParameter[] prm2 = new SqlParameter[] {
                      new SqlParameter("Type", "ParentsLogin"),
                     new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                   };
                    DataTable dtParents = new SQLHelper().ExecuteDataTable("SP_StudentMarksStudent", prm2, CommandType.StoredProcedure);
                    loggedInParents smp = new loggedInParents();
                    smp.SessionID = Convert.ToString(dtParents.Rows[0]["SR_SessionID"]);
                    smp.StudentID = Convert.ToString(dtParents.Rows[0]["SR_ID"]);
                    smp.ClassID = Convert.ToString(dtParents.Rows[0]["SR_ClassID"]);
                    smp.SectionID = Convert.ToString(dtParents.Rows[0]["SR_SectionID"]);
                    smp.UserID = Convert.ToString(dtParents.Rows[0]["UserID"]);
                    System.Web.HttpContext.Current.Session["loggedInParents"] = smp;
                }


                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Student
        public ActionResult StudentProfile()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            return View();
        }

        public ActionResult EditStudent(string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                    new SqlParameter("UserType", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
                    new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_StudentProfile", prm1, CommandType.StoredProcedure);

                List<SR> List = Utility.ConvertDataTableToClassObjectList<SR>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveStudentRegistration()
        {
            try
            {
                string _SR_ID = Request.Form.Get("_SR_ID");
                string _SR_StudentName = Request.Form.Get("_SR_StudentName");
                string _SR_Email = Request.Form.Get("_SR_Email");
                string _SR_AlternetContactNo = Request.Form.Get("_SR_AlternetContactNo");
                string _SR_DOB = Request.Form.Get("_SR_DOB");
                string _SR_Gender = Request.Form.Get("_SR_Gender");
                string _SR_Addrees = Request.Form.Get("_SR_Addrees");
                string _UserPassword = Request.Form.Get("_UserPassword");
                var _SR_Pic = System.Web.HttpContext.Current.Request.Files["_SR_Pic"];
                string imgpath = null;

                string Type = "Update";

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                     new SqlParameter("SR_ID", _SR_ID),
                     new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                     new SqlParameter("SR_StudentName", _SR_StudentName),
                     new SqlParameter("SR_Email", _SR_Email),
                     new SqlParameter("SR_AlternetContactNo", _SR_AlternetContactNo),
                     new SqlParameter("SR_DOB", _SR_DOB),
                     new SqlParameter("SR_Gender", _SR_Gender),
                     new SqlParameter("SR_Addrees", _SR_Addrees),
                     new SqlParameter("UserPassword", _UserPassword),
                     new SqlParameter("UserType", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
                     new SqlParameter("UserID",  ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                     new SqlParameter("SR_Pic", imgpath)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_StudentProfile", prm1, CommandType.StoredProcedure));

                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Accountant
        public ActionResult AccountantProfile()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ddlDesignationList = Utility.GetDropDownList("SP_Desgination", "DesID", "DesName", prm1, "", "", "Select");
            return View();
        }
        #endregion

        #region Librarian
        public ActionResult LibrarianProfile()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ddlDesignationList = Utility.GetDropDownList("SP_Desgination", "DesID", "DesName", prm1, "", "", "Select");

            return View();
        }

        public ActionResult SaveEmployee()
        {
            try
            {
                string EM_EmpId = Request.Form.Get("EM_EmpId");
                string EM_EmpName = Request.Form.Get("EM_EmpName");
                string EM_DOB = Request.Form.Get("EM_DOB");
                string EM_Gender = Request.Form.Get("EM_Gender");
                string EM_EmpFathers = Request.Form.Get("EM_EmpFathers");
                string EM_EmpAddress = Request.Form.Get("EM_EmpAddress");
                string EM_Email = Request.Form.Get("EM_Email");
                string EM_Password = Request.Form.Get("EM_Password");
                string EM_PanNo = Request.Form.Get("EM_PanNo");
                string EM_AdharNo = Request.Form.Get("EM_AdharNo");
                var EM_ProfilePic = System.Web.HttpContext.Current.Request.Files["EM_ProfilePic"];
                string imgpath = null;

                string Type = "UpdateProfile";

                SqlParameter[] prm1 = new SqlParameter[] {
                new SqlParameter("Type", Type),
                new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                new SqlParameter("UserType", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
                new SqlParameter("EM_EmpContactNo", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                new SqlParameter("EM_EmpId",EM_EmpId),
                new SqlParameter("EM_EmpName",EM_EmpName),
                new SqlParameter("EM_DOB",EM_DOB),
                new SqlParameter("EM_Gender",EM_Gender),
                new SqlParameter("EM_EmpFathers",EM_EmpFathers),
                new SqlParameter("EM_EmpAddress",EM_EmpAddress),
                new SqlParameter("EM_Email",EM_Email),
                new SqlParameter("EM_PanNo",EM_PanNo),
                new SqlParameter("EM_AdharNo",EM_AdharNo),
                new SqlParameter("Password",EM_Password),
                new SqlParameter("EM_ProfilePic", imgpath)
                      };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Employee", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ViewEmployee(string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                    new SqlParameter("UserType", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
                    new SqlParameter("EM_EmpContactNo", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Employee", prm1, CommandType.StoredProcedure);
                List<Employee> List = Utility.ConvertDataTableToClassObjectList<Employee>(dt);
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