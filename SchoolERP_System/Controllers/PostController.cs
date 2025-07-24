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
    public class PostController : Controller
    {
        #region Create Post
        public ActionResult PostAll()
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
        public ActionResult BindpostSection(string Id)
        {
            try
            {
                List<Section> ListSection = null;
                if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType.ToString() == "SuperAdmin")
                {
                    SqlParameter[] prm2 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectSection"),
                    new SqlParameter("ClassID", Id),
                     };
                    DataTable dt = new SQLHelper().ExecuteDataTable("SP_EmpClassMapping", prm2, CommandType.StoredProcedure);
                    ListSection = Utility.ConvertDataTableToClassObjectList<Section>(dt);
                }
                else
                {
                    SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectAssignSection"),
                    new SqlParameter("ClassID", Id),
                    new SqlParameter("UserID",  ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                  new SqlParameter("userType",  ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
                };
                    DataTable dt = new SQLHelper().ExecuteDataTable("SP_EmpClassMapping", prm1, CommandType.StoredProcedure);
                    ListSection = Utility.ConvertDataTableToClassObjectList<Section>(dt);
                }
                return Json(ListSection, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [ValidateInput(false)]
        public ActionResult PostSave()
        {

            try
            {
                string ID = Request.Form.Get("ID");
                string Type = Request.Form.Get("Type");
                string Class = Request.Form.Get("Class");
                string Section = Request.Form.Get("Section");
                string PostData = Request.Form.Get("PostData");
                string Action = "";
                if (ID == "" || ID == "0")
                    Action = "Insert";
                else
                    Action = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Action),
                    new SqlParameter("PostID", ID),
                    new SqlParameter("ClassID", Class),
                    new SqlParameter("SectionID", Section),
                    new SqlParameter("PostMode", Type),
                    new SqlParameter("PostData", PostData),
                    new SqlParameter("UserType", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
                    new SqlParameter("CreatedBy", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID)

                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Post", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditViewPostTeacher(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectByID"),
                    new SqlParameter("PostID", Id)
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Post", prm1, CommandType.StoredProcedure);
                List<EditPostModel> ListSection = Utility.ConvertDataTableToClassObjectList<EditPostModel>(dt);
                return Json(ListSection, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public string UploadImage()
        {
            var request = System.Web.HttpContext.Current.Request;
            var baseUrl = request.Url.Scheme + "://" + request.Url.Authority;
            var _SR_Pic = System.Web.HttpContext.Current.Request.Files["file"];
            var _type = Request.Form.Get("Type");
            string imgpath = null;
            if (_SR_Pic != null)
            {
                var fileName = Path.GetFileName(_SR_Pic.FileName);
                var extention = Path.GetExtension(_SR_Pic.FileName);
                var filenamewithoutextension = Path.GetFileNameWithoutExtension(_SR_Pic.FileName);
                string FileName = DateTime.Now.GetHashCode().ToString("x") + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + System.IO.Path.GetExtension(fileName);
                if (_type.ToString() == "img")
                {
                    imgpath = Path.Combine(Server.MapPath("/ImagesPost/" + FileName));
                    baseUrl += "/ImagesPost/";
                }
                else
                {
                    imgpath = Path.Combine(Server.MapPath("/DocPost/" + FileName));
                    baseUrl += "/DocPost/";
                }
                _SR_Pic.SaveAs(imgpath);
                imgpath = FileName;
            }
            //return "http://www.meetupschool.com/EmpImage/c526632a_14042020154506.jpg";


            return baseUrl + imgpath;
        }

        public ActionResult BindStudent(string ID, string type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", type),
                    new SqlParameter("ClassID", ID)
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Post", prm1, CommandType.StoredProcedure);
                List<SR> ListSection = Utility.ConvertDataTableToClassObjectList<SR>(dt);

                return Json(ListSection, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region View Post
        public ActionResult ViewPost()
        {
            return View();
        }

        public ActionResult ViewPostTeacher(string Action, string PostType)
        {
            try
            {
                string UserID = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID;
                string UserType = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType;
                if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Parents")
                {
                    UserID = ((loggedInParents)System.Web.HttpContext.Current.Session["loggedInParents"]).UserID;
                    UserType = "Student";
                }
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Action),
                    new SqlParameter("PostMode", PostType),
                    new SqlParameter("UserID",  UserID),
                    new SqlParameter("userType",  UserType),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Post", prm1, CommandType.StoredProcedure);
                List<ViewPostModel> ListSection = Utility.ConvertDataTableToClassObjectList<ViewPostModel>(dt);
                return Json(ListSection, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeletePost(string Id)
        {
            try
            {

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("PostID", Id),
                    new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Post", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ViewFeed()
        {
            return View();
        }

        #endregion

        #region PostIND
        [ValidateInput(false)]
        public ActionResult PostSaveForTag()
        {
            try
            {
                string ID = Request.Form.Get("ID");
                string StudentID = Request.Form.Get("StudentID");
                string PostData = Request.Form.Get("PostData");

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "InsertIND"),
                    new SqlParameter("PostINDID", ID),
                    new SqlParameter("StudentID", StudentID),
                    new SqlParameter("PostData", PostData),
                    new SqlParameter("UserType", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType),
                    new SqlParameter("CreatedBy", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID)

                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_PostIND", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region View Post Individual
        public ActionResult ViewPostIndividual()
        {
            return View();
        }

        public ActionResult ViewPostTeacherIndividual(string Action)
        {
            try
            {
                string UserID = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID;
                string UserType = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType;

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Action),
                    new SqlParameter("UserID", UserID),
                    new SqlParameter("userType", UserType),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_PostIND", prm1, CommandType.StoredProcedure);
                List<ViewPostINDModel> ListSection = Utility.ConvertDataTableToClassObjectList<ViewPostINDModel>(dt);
                return Json(ListSection, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EditViewPostTeacherIndividual(string Id)
        {
            try
            {
                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectByID"),
                    new SqlParameter("PostINDID", Id)
                };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_PostIND", prm1, CommandType.StoredProcedure);
                List<ViewPostINDModel> ListPost = Utility.ConvertDataTableToClassObjectList<ViewPostINDModel>(dt.Tables[0]);
                List<SR> ListSR = Utility.ConvertDataTableToClassObjectList<SR>(dt.Tables[1]);
                mixArray[0] = ListPost;
                return Json(mixArray, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeletePostIndividual(string Id)
        {
            try
            {

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("PostINDID", Id),
                    new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_PostIND", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ViewFeedIndividual()
        {
            return View();
        }

        #endregion

        #region Open Question
        public ActionResult OpenQuestion()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectClass"),
                new SqlParameter("StudentID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
            };
            ViewBag.TeacherList = Utility.GetDropDownList("SP_OQ", "EM_EmpId", "EM_EmpName", prm1, "", "", "Select");
            return View();
        }
        [ValidateInput(false)]
        public ActionResult saveOpenQuestion(string Id, string TID, string Subject, string PostData)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Insert"),
                    new SqlParameter("OPID", Id),
                    new SqlParameter("TeacherID", TID),
                    new SqlParameter("StudentID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                    new SqlParameter("Subject", Subject),
                    new SqlParameter("PostData", PostData),
                     new SqlParameter("UserType",((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_OQ", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewOpenQuestion(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("OPID",Id),
                    new SqlParameter("UserID",((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_OQ", prm1, CommandType.StoredProcedure);
                List<OQ> list = Utility.ConvertDataTableToClassObjectList<OQ>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewOpenQuestionByID(string Id, string Type)
        {

            object[] mixArray = new object[2];
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("OPID",Id),
                };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_OQ", prm1, CommandType.StoredProcedure);
                List<OQ> list = Utility.ConvertDataTableToClassObjectList<OQ>(dt.Tables[0]);
                List<OQ> listDetails = Utility.ConvertDataTableToClassObjectList<OQ>(dt.Tables[1]);
                mixArray[0] = list;
                mixArray[1] = listDetails;
                return Json(mixArray, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteOpenQuestion(string Id, string type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", type),
                    new SqlParameter("OPID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_OQ", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Close Question
        public ActionResult CloseQuestion()
        {
            return View();
        }

        public ActionResult ViewQuestion()
        {
            return View();
        }
        public ActionResult RetrunView()
        {
            if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Student")
            {
                return RedirectToAction("OpenQuestion");
            }
            return RedirectToAction("CloseQuestion");
        }
        [ValidateInput(false)]
        public ActionResult UpdateOpenQuestion(string Id, string PostData)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "UpdateBoth"),
                    new SqlParameter("OPID", Id),
                    new SqlParameter("PostData", PostData),
                     new SqlParameter("UserType",((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_OQ", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}