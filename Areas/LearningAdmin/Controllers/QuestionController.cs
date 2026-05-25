using SchoolERP_System.Areas.LearningAdmin.Helper;
using SchoolERP_System.Areas.LearningAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolERP_System.Areas.LearningAdmin.Controllers
{
    [SessionAuthorizedAttribute]
    public class QuestionController : Controller
    {

        public ActionResult QuestionMaster()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.SpecializationList = Utility.GetDropDownList("SP_Specialization", "SC_ID", "SC_Name", prm1, "", "", "Select");

            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.LevelList = Utility.GetDropDownList("SP_Deifficulty_Level", "DL_ID", "DL_Name", prm2, "", "", "Select");

            ViewBag.SubjectList = null;
            return View();
        }
        public ActionResult BindSubject(string ID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Subject"),
                    new SqlParameter("ID", ID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_BindingDropdown", prm1, CommandType.StoredProcedure);
                List<Subject> List = Utility.ConvertDataTableToClassObjectList<Subject>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult BindUnit(string ID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Unit"),
                    new SqlParameter("ID", ID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_BindingDropdown", prm1, CommandType.StoredProcedure);
                List<Unit> List = Utility.ConvertDataTableToClassObjectList<Unit>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult BindTopic(string ID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Topic"),
                    new SqlParameter("ID", ID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_BindingDropdown", prm1, CommandType.StoredProcedure);
                List<Topic> List = Utility.ConvertDataTableToClassObjectList<Topic>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult saveQuestion(string ID, string Spec_ID, string Sub_ID, string Unit_ID, string Topic_ID, string Level_ID, string Type_ID, string Question, string Paragaraph, string Solution,
                                         string chk1, string chk2, string chk3, string chk4, string txt1, string txt2, string txt3, string txt4)
        {
            try
            {
                string Type = "";
                if (ID == "" || ID == "0")
                    Type = "Save";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("QM_ID", ID),
                    new SqlParameter("QM_SC_ID", Spec_ID),
                    new SqlParameter("QM_SubjectID", Sub_ID),
                    new SqlParameter("QM_UnitID", Unit_ID),
                    new SqlParameter("QM_TopicID", Topic_ID),
                    new SqlParameter("QM_Level_ID", Level_ID),
                    new SqlParameter("QM_Type", Type_ID),
                    new SqlParameter("QM_Question", Question),
                    new SqlParameter("QM_Paragaraph", Paragaraph),
                    new SqlParameter("QM_Solution", Solution),
                    new SqlParameter("QM_SlNo1", chk1),
                    new SqlParameter("QM_SlNo2", chk2),
                    new SqlParameter("QM_SlNo3", chk3),
                    new SqlParameter("QM_SlNo4", chk4),
                    new SqlParameter("QM_Option1", txt1),
                    new SqlParameter("QM_Option2", txt2),
                    new SqlParameter("QM_Option3", txt3),
                    new SqlParameter("QM_Option4", txt4)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Question", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewQuestion()
        {         
            return View();
        }

        public ActionResult ViewQuestionList(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("QM_ID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Question", prm1, CommandType.StoredProcedure);
                if (Type == "SelectByID")
                {
                    List<VQ_EDIT> List = Utility.ConvertDataTableToClassObjectList<VQ_EDIT>(dt);
                    return Json(List, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<VQ> List = Utility.ConvertDataTableToClassObjectList<VQ>(dt);
                    ViewBag.data = List;
                    return Json(List, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteQuestion(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("QM_ID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Question", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}