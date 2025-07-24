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
    public class FeeSettingsERPController : Controller
    {
        #region RouteWiseFees
        public ActionResult RouteWiseFees()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.Route = Utility.GetDropDownList("SP_Route", "RouteID", "RouteName", prm1, "", "", "Select");

            return View();
        }

        public ActionResult saveRouteWiseFees(string Id, string RouteID, string Route_Fee, string Fee_Date)
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
                    new SqlParameter("RouteFeeID", Id),
                    new SqlParameter("RouteID", RouteID),
                    new SqlParameter("Route_Fee", Route_Fee),
                    new SqlParameter("Fee_Date", Fee_Date)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Route_Wise_fees", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewRouteWiseFees(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("RouteFeeID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Route_Wise_fees", prm1, CommandType.StoredProcedure);
                List<RouteWiseFees> classList = Utility.ConvertDataTableToClassObjectList<RouteWiseFees>(dt);
                return Json(classList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteRouteWiseFees(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("RouteFeeID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Route_Wise_fees", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region FeesHeadMaster
        public ActionResult FeesHeadMaster()
        {
            return View();
        }

        public ActionResult saveFeesHeadMaster(string Id, string FeeHeadName,  string Is_Bus_Fees, string Is_Admission_Fees, string Is_Session_Fees, string Is_Hostel_Fees, string Is_Other_Fees, string Otherfees_Month, string Is_Bengali_Fees, string Active)
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
                    new SqlParameter("FeeHeadID", Id),
                    new SqlParameter("FeeHeadName", FeeHeadName),
                    //new SqlParameter("At_Admission", At_Admission),
                    //new SqlParameter("Total_of_Installment", Total_of_Installment),
                    //new SqlParameter("Installment_TOA", Installment_TOA),
                    new SqlParameter("Is_Bus_Fees", Is_Bus_Fees),
                    new SqlParameter("Is_Admission_Fees", Is_Admission_Fees),
                    new SqlParameter("Is_Session_Fees", Is_Session_Fees),
                    new SqlParameter("Is_Other_Fees", Is_Other_Fees),
                    new SqlParameter("Other_Fees_Month", Otherfees_Month),
                    new SqlParameter("Is_Hostel_Fees", Is_Hostel_Fees),
                    new SqlParameter("Is_Bengali_Fees", Is_Bengali_Fees),
                    new SqlParameter("Is_Active", Active),
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_FeeHead", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewFeesHeadMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("FeeHeadID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_FeeHead", prm1, CommandType.StoredProcedure);
                List<FeeHead> List = Utility.ConvertDataTableToClassObjectList<FeeHead>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteFeesHeadMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("FeeHeadID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_FeeHead", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ClasswiseFees
        public ActionResult ClasswiseFees()
        {
            return View();
        }

        public ActionResult saveClasswiseFees(string Id, List<ClassWiseFeeHeadDtl> Datadt)
        {
            try
            {
                DataTable dttAdr = GetAttdanceTable(Datadt);
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    //new SqlParameter("CategoryID", CategoryID),
                    //new SqlParameter("StreamID", StreamID),
                    //new SqlParameter("ClassID", ClassID),
                    new SqlParameter("myTableType", dttAdr)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ClassWiseFee", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public DataTable GetAttdanceTable(List<ClassWiseFeeHeadDtl> stddt)
        {
            GenLib objLib = new GenLib();
            DataTable dt = new DataTable();
            dt = objLib.ToDataTable(stddt);
            return dt;
        }
        public ActionResult viewClasswiseFees(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ClassID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ClassWiseFee", prm1, CommandType.StoredProcedure);
                List<ClassWiseFeeHead> List = Utility.ConvertDataTableToClassObjectList<ClassWiseFeeHead>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewClasswiseFeesEdit(string Id, string Type)
        {
            try
            {
                string[] data = Id.Split('-');

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ClassID", data[0]),
                    new SqlParameter("CategoryID", data[1]),
                    new SqlParameter("FeeHeadID", data[2])
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ClassWiseFee", prm1, CommandType.StoredProcedure);
                List<ClassWiseFeeHead> List = Utility.ConvertDataTableToClassObjectList<ClassWiseFeeHead>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewClasswiseFee(string Id, string Type)
        {
            try
            {
                string[] data = Id.Split('-');

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type)
                    //new SqlParameter("ClassID", data[0]),
                    //new SqlParameter("CategoryID", data[1]),
                    //new SqlParameter("StreamID",0)
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ClassWiseFee", prm1, CommandType.StoredProcedure);
                List<ClassWiseFeeHead> List = Utility.ConvertDataTableToClassObjectList<ClassWiseFeeHead>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteClasswiseFees(string Id)
        {
            try
            {
                string[] data = Id.Split('-');
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ClassID", data[0]),
                    new SqlParameter("CategoryID", data[1]),
                    new SqlParameter("FeeHeadID", data[2])
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ClassWiseFee", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddClasswiseFees()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.FeeHeadList = Utility.GetDropDownList("SP_FeeHead", "FeeHeadID", "FeeHeadName", prm2, "", "", "Select");
            
            return View();
        }

        public ActionResult BindStream(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectStream"),
                    new SqlParameter("ClassID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ClassWiseFee", prm1, CommandType.StoredProcedure);
                List<Streamcl> List = Utility.ConvertDataTableToClassObjectList<Streamcl>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult BindInstallment(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectByID"),
                    new SqlParameter("FeeHeadID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_FeeHead", prm1, CommandType.StoredProcedure);
                List<FeeHead> List = Utility.ConvertDataTableToClassObjectList<FeeHead>(dt);
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