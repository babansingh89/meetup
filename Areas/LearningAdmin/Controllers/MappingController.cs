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
    public class MappingController : Controller
    {
        #region SpecializationWithSubject
        public ActionResult SpecializationWithSubject()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.SpecializationList = Utility.GetDropDownList("SP_Specialization", "SC_ID", "SC_Name", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.SubjectList = Utility.GetDropDownList("SP_Subject", "SM_Id", "SM_SubjectName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult saveSSMaster(string SS_ID, string Spec_ID, string Sub_ID)
        {
            try
            {
                string Type = "";
                if (SS_ID == "" || SS_ID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SS_ID", SS_ID),
                    new SqlParameter("Spec_ID", Spec_ID),
                    new SqlParameter("Sub_ID", Sub_ID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Spec_Sub", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewSSMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SS_ID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Spec_Sub", prm1, CommandType.StoredProcedure);
                List<SS> List = Utility.ConvertDataTableToClassObjectList<SS>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteSSMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("SS_ID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Spec_Sub", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region SubjectWithUnit
        public ActionResult SubjectWithUnit()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.UnitList = Utility.GetDropDownList("SP_Unit", "UM_Id", "UM_UnitName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectDropdown"),
            };
            ViewBag.SubjectList = Utility.GetDropDownList("SP_Sub_Unit", "Sub_ID", "SM_SubjectName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult saveSUMaster(string SU_ID, string Sub_ID,string Unit_ID)
        {
            try
            {
                string Type = "";
                if (SU_ID == "" || SU_ID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SU_ID", SU_ID),
                    new SqlParameter("Unit_ID", Unit_ID),
                    new SqlParameter("Sub_ID", Sub_ID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Sub_Unit", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewSUMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SU_ID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Sub_Unit", prm1, CommandType.StoredProcedure);
                List<SU> List = Utility.ConvertDataTableToClassObjectList<SU>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteSUMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("SU_ID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Sub_Unit", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region UnitWithTopic
        public ActionResult UnitWithTopic()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectDropdown"),
            };
            ViewBag.UnitList = Utility.GetDropDownList("SP_Unit_Topic", "UM_Id", "UM_UnitName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.TopicList = Utility.GetDropDownList("SP_Topic", "TM_Id", "TM_Name", prm2, "", "", "Select");
            return View();
        }
        public ActionResult saveUTMaster(string UP_ID, string Unit_ID, string Topic_ID)
        {
            try
            {
                string Type = "";
                if (UP_ID == "" || UP_ID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("UP_ID", UP_ID),
                    new SqlParameter("Unit_ID", Unit_ID),
                    new SqlParameter("Topic_ID", Topic_ID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Unit_Topic", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewUTMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("UP_ID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Unit_Topic", prm1, CommandType.StoredProcedure);
                List<UP> List = Utility.ConvertDataTableToClassObjectList<UP>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteUTMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("UP_ID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Unit_Topic", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region TopicWithVideo
        public ActionResult TopicWithVideo()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.VideoList = Utility.GetDropDownList("SP_Video_Master", "V_ID", "V_Name", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectDropdown"),
            };
            ViewBag.TopicList = Utility.GetDropDownList("SP_Topic_Video", "TM_Id", "TM_Name", prm2, "", "", "Select");
            return View();
        }
        public ActionResult saveTVMaster(string TV_ID, string Video_ID, string Topic_ID)
        {
            try
            {
                string Type = "";
                if (TV_ID == "" || TV_ID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("TV_ID", TV_ID),
                    new SqlParameter("Video_ID", Video_ID),
                    new SqlParameter("Topic_ID", Topic_ID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Topic_Video", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewTVMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("TV_ID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Topic_Video", prm1, CommandType.StoredProcedure);
                List<TV> List = Utility.ConvertDataTableToClassObjectList<TV>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteTVMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("TV_ID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Topic_Video", prm1, CommandType.StoredProcedure));
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