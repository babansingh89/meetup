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
    public class HRController : Controller
    {
        #region  Employee Master
        public ActionResult EmployeeMaster()
        {
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm2, "", "", "Select");
            return View();
        }

        public ActionResult ViewEmployee(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("EM_EmpId", Id),

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
        public ActionResult DeleteEmployee(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("EM_EmpId", Id),
                     new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Employee", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AddEmployee()
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
                string EM_EmpDOJ = Request.Form.Get("EM_EmpDOJ");
                string EM_UserType = Request.Form.Get("EM_UserType");
                string EM_EmpName = Request.Form.Get("EM_EmpName");
                string EM_DOB = Request.Form.Get("EM_DOB");
                string EM_Gender = Request.Form.Get("EM_Gender");
                string EM_EmpDesignationId = Request.Form.Get("EM_EmpDesignationId");
                string EM_EmpFathers = Request.Form.Get("EM_EmpFathers");
                string EM_EmpAddress = Request.Form.Get("EM_EmpAddress");
                string EM_EmpContactNo = Request.Form.Get("EM_EmpContactNo");
                string EM_TotalSal = Request.Form.Get("EM_TotalSal");
                string EM_Degree = Request.Form.Get("EM_Degree");
                string EM_PanNo = Request.Form.Get("EM_PanNo");
                string EM_AdharNo = Request.Form.Get("EM_AdharNo");
                string EM_DA = Request.Form.Get("EM_DA");
                string EM_Basic = Request.Form.Get("EM_Basic");
                string EM_HRA = Request.Form.Get("EM_HRA");
                string EM_Conveyance = Request.Form.Get("EM_Conveyance");
                string EM_MedicalAllowance = Request.Form.Get("EM_MedicalAllowance");
                string EM_TelephoneAllowance = Request.Form.Get("EM_TelephoneAllowance");
                string EM_Leave = Request.Form.Get("EM_Leave");
                string EM_PunchCode = Request.Form.Get("EM_PunchCode");
                var EM_ProfilePic = System.Web.HttpContext.Current.Request.Files["EM_ProfilePic"];
                string imgpath = null;
                if (EM_ProfilePic != null)
                {
                    var fileName = Path.GetFileName(EM_ProfilePic.FileName);
                    var extention = Path.GetExtension(EM_ProfilePic.FileName);
                    var filenamewithoutextension = Path.GetFileNameWithoutExtension(EM_ProfilePic.FileName);
                    string FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + System.IO.Path.GetExtension(fileName);
                    imgpath = Path.Combine(Server.MapPath("/EmpImage/" + FileName));
                    EM_ProfilePic.SaveAs(imgpath);
                    imgpath = FileName;
                }

                string Type = "";
                if (EM_EmpId == "" || EM_EmpId == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                new SqlParameter("Type", Type),
                new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                new SqlParameter("EM_EmpId",EM_EmpId),
                new SqlParameter("EM_EmpDOJ",EM_EmpDOJ),
                new SqlParameter("EM_UserType",EM_UserType),
                new SqlParameter("EM_EmpName",EM_EmpName),
                new SqlParameter("EM_DOB",EM_DOB),
                new SqlParameter("EM_Gender",EM_Gender),
                new SqlParameter("EM_EmpDesignationId",EM_EmpDesignationId),
                new SqlParameter("EM_EmpFathers",EM_EmpFathers),
                new SqlParameter("EM_EmpAddress",EM_EmpAddress),
                new SqlParameter("EM_EmpContactNo",EM_EmpContactNo),
                new SqlParameter("EM_TotalSal",EM_TotalSal==""?null:EM_TotalSal),
                new SqlParameter("EM_Degree",EM_Degree),
                new SqlParameter("EM_PanNo",EM_PanNo),
                new SqlParameter("EM_AdharNo",EM_AdharNo),
                new SqlParameter("EM_DA",EM_DA==""?null:EM_DA),
                new SqlParameter("EM_Basic",EM_Basic==""?null:EM_Basic),
                new SqlParameter("EM_HRA",EM_HRA==""?null:EM_HRA),
                new SqlParameter("EM_Conveyance",EM_Conveyance==""?null:EM_Conveyance),
                new SqlParameter("EM_MedicalAllowance",EM_MedicalAllowance==""?null:EM_MedicalAllowance),
                new SqlParameter("EM_TelephoneAllowance",EM_TelephoneAllowance==""?null:EM_TelephoneAllowance),
                new SqlParameter("EM_Leave",EM_Leave==""?null:EM_TotalSal),
                new SqlParameter("EM_PunchCode",EM_PunchCode),
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

        public ActionResult ResetPassword(string Id,string userType,string passsword)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("UserID", Id),
                    new SqlParameter("UserType", userType),
                    new SqlParameter("UserPassword", passsword),
                    new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID)
                };
                string Output = Convert.ToString(new SQLHelperMaster().ExecuteScalar("SP_ResetLogin", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LoginPassword(string Id, string userType, string passsword)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "GenrateLogin"),
                    new SqlParameter("UserID", Id),
                    new SqlParameter("UserType", userType),
                    new SqlParameter("UserPassword", passsword),
                    new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID)
                };
                string Output = Convert.ToString(new SQLHelperMaster().ExecuteScalar("SP_ResetLogin", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Loan & Advance Process
        public ActionResult LoanAdvanceProcess()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ddlEmployeeList = Utility.GetDropDownList("SP_Employee", "EM_EmpId", "EM_EmpName", prm1, "", "", "Select");

            return View();
        }
        public ActionResult saveLoan(string ID, string Year, string Month, string IN, string Intrest, string Amt,
            string Type, string EmployeeID)
        {
            try
            {
                string TypeSp = "";
                if (ID == "" || ID == "0")
                    TypeSp = "Save";
                else
                    TypeSp = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", TypeSp),
                    new SqlParameter("LoanID", ID),
                    new SqlParameter("LoanType", Type),
                    new SqlParameter("EmpID", EmployeeID),
                    new SqlParameter("Amount", Amt),
                    new SqlParameter("Intrest", Intrest),
                    new SqlParameter("AdvMonth", Month),
                    new SqlParameter("AdvYear", Year=="Select"?null:Year),
                    new SqlParameter("NoOfInstall", IN)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Loan", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewLoan(string ID, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("LoanID",ID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Loan", prm1, CommandType.StoredProcedure);
                List<LoanMaster> list = Utility.ConvertDataTableToClassObjectList<LoanMaster>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteLoan(string ID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("LoanID", ID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Loan", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Salary Process
        public ActionResult SalaryProcess()
        {
            return View();
        }

        public ActionResult viewSalaryProcess(string Month, string Year)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectByID"),
                    new SqlParameter("MonthID",Month),
                    new SqlParameter("YearID",Year),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_SalaryProcess", prm1, CommandType.StoredProcedure);
                List<SalaryProcess> list = Utility.ConvertDataTableToClassObjectList<SalaryProcess>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult saveSalaryProcess(List<SalaryProcess> Emp, string strMonth, string strYear)
        {
            try
            {
                DataTable dttAdr = GetTable(Emp);

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Insert"),
                    new SqlParameter("MonthID",strMonth),
                    new SqlParameter("YearID",strYear),
                    new SqlParameter("EmpSalary",dttAdr),
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_SalaryProcess", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public DataTable GetTable(List<SalaryProcess> stddt)
        {
            GenLib objLib = new GenLib();
            DataTable dt = new DataTable();
            dt = objLib.ToDataTable(stddt);
            return dt;
        }
        #endregion

        #region Salary Report
        public ActionResult SalaryReport()
        {
            return View();
        }
        public ActionResult viewSalaryReport(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type)
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_SalaryProcess", prm1, CommandType.StoredProcedure);
                List<SalaryProcess> list = Utility.ConvertDataTableToClassObjectList<SalaryProcess>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PrintSalary(string ID)
        {
            string[] str = ID.Split('_');
            try
            {

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Print"),
                    new SqlParameter("MonthID",str[0]),
                    new SqlParameter("YearID",str[1]),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_SalaryProcess", prm1, CommandType.StoredProcedure);
                List<SalaryProcess> list = Utility.ConvertDataTableToClassObjectList<SalaryProcess>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region  Attendence
        public ActionResult Attendence()
        {
            return View();
        }

        public ActionResult viewAttendacne(string From)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectByID"),
                    new SqlParameter("From",From)
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_EmpAttendacne", prm1, CommandType.StoredProcedure);
                List<EmpAttendacne> list = Utility.ConvertDataTableToClassObjectList<EmpAttendacne>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult saveAttendacne(List<SaveEmpAttendacne> Emp, string From)
        {
            try
            {
                DataTable dttAdr = GetAttendacneTable(Emp);

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Insert"),
                    new SqlParameter("From",From),
                    new SqlParameter("EmpAtt",dttAdr),
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_EmpAttendacne", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public DataTable GetAttendacneTable(List<SaveEmpAttendacne> stddt)
        {
            GenLib objLib = new GenLib();
            DataTable dt = new DataTable();
            dt = objLib.ToDataTable(stddt);
            return dt;
        }
        #endregion

        #region AttendenceReport
        public ActionResult AttendenceReport()
        {
            return View();
        }

        public ActionResult PrintEMP(string From, string To)
        {
            string strimgSrc = "";
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "PrintAttendance"),
                    new SqlParameter("From", From),
                    new SqlParameter("To", To),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_EMPPrint", prm1, CommandType.StoredProcedure);
                List<printEmpAttendance> List = Utility.ConvertDataTableToClassObjectList<printEmpAttendance>(dt);


                string OuterHTML = generateEMPhtml(List);
                string PDFpath = GenerateLoanPDF(OuterHTML);
                return Json(PDFpath, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        private byte[] GetBytesFromImage(String imageFile)
        {
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromFile(imageFile);
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        private string generateEMPhtml(List<printEmpAttendance> ls)
        {
            string path = "";
            if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchoolLogo == "")
            {

            }
            else
            {
                byte[] ImgByte = GetBytesFromImage(Server.MapPath("/SchImage/" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchoolLogo));
                var base64 = Convert.ToBase64String(ImgByte);
                path = String.Format("data:image/gif;base64,{0}", base64);
            }
            string str = "";
            str += "<html>";
            str += "<head>";
            str += "</head>";
            str += "<body>";
            str += "<div style = 'margin: 15px 50px 15px 50px; padding: 10px 10px 10px 10px; border: 1px solid black;'>";

            str += "   <div class='row'>";
            str += "    <div class='col-md-10'>";
            str += "     <div class='divHeader'>";
            str += "       <p class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).Name + "</b></p>";
            str += "      <p class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchAddress + "</b></p>";
            str += "    </div>";
            str += "  </div>";
            str += " </div>";
            str += " <div class='row'>";
            str += "<div class='col-md-2'></div>";
            str += "<div class='col-md-10'>";
            str += "<p class='text-center'><b><u>Employee Attendacne</u></b></p>";
            str += "</div>";
            str += "<div class='row'>";
            str += "<div class='col-md-12'>";
            str += "<table class='blueTable'><tr><th>Emp Code</th><th>EMP Name</th><th>Phone</th><th>DOJ</th><th>Working Days</th><th>Present</th><th>Absent</th><th>%</th></tr>";

            for (int i = 0; i < ls.Count; i++)
            {
                if (ls[i].EM_EmpName != null)
                {

                    str += "<tr><td>" + ls[i].EM_EmpCode + "</<td><td>" + ls[i].EM_EmpName + "</<td><td>" + ls[i].EM_EmpContactNo + "</<td><td>" + ls[i].EM_EmpDOJ + "</<td><td>" + ls[i].WD + "</<td><td>" + ls[i].Pass + "</<td><td>" + ls[i].Fail + "</<td><td>" + ls[i].Per + "</<td> </tr>";
                }
            }

            str += " </table>";
            str += " </div>";
            str += " </div>";

            str += "</body>";
            str += "</html>";
            return str;
        }

        public string GenerateLoanPDF(string OuterHTML)
        {
            string htmlString = OuterHTML;
            bool canAssembleDocument = true;
            bool canCopyContent = true;
            bool canEditAnnotations = true;
            bool canEditContent = true;
            bool canFillFormFields = true;
            bool canPrint = true;
            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);

            int webPageWidth = 1024;
            int webPageHeight = 0;

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            converter.Options.SecurityOptions.CanAssembleDocument = canAssembleDocument;
            converter.Options.SecurityOptions.CanCopyContent = canCopyContent;
            converter.Options.SecurityOptions.CanEditAnnotations = canEditAnnotations;
            converter.Options.SecurityOptions.CanEditContent = canEditContent;
            converter.Options.SecurityOptions.CanFillFormFields = canFillFormFields;
            converter.Options.SecurityOptions.CanPrint = canPrint;

            #region Header and Footer
            string headerUrl = Server.MapPath("/Content/Image/Lhead_Head.png");
            bool showHeaderOnFirstPage = true;
            bool showHeaderOnOddPages = true;
            bool showHeaderOnEvenPages = true;
            int headerHeight = 80;
            bool showFooterOnFirstPage = true;
            bool showFooterOnOddPages = true;
            bool showFooterOnEvenPages = true;
            // header settings
            converter.Options.DisplayHeader = showHeaderOnFirstPage ||
                showHeaderOnOddPages || showHeaderOnEvenPages;
            converter.Header.DisplayOnFirstPage = showHeaderOnFirstPage;
            converter.Header.DisplayOnOddPages = showHeaderOnOddPages;
            converter.Header.DisplayOnEvenPages = showHeaderOnEvenPages;
            converter.Header.Height = headerHeight;

            PdfHtmlSection headerHtml = new PdfHtmlSection(headerUrl);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(headerHtml);

            // footer settings
            converter.Options.DisplayFooter = showFooterOnFirstPage ||
                showFooterOnOddPages || showFooterOnEvenPages;
            converter.Footer.DisplayOnFirstPage = showFooterOnFirstPage;
            converter.Footer.DisplayOnOddPages = showFooterOnOddPages;
            converter.Footer.DisplayOnEvenPages = showFooterOnEvenPages;

            PdfTextSection text = new PdfTextSection(0, 10,
                "Page: {page_number} of {total_pages}  ",
                new System.Drawing.Font("Arial", 8));
            text.HorizontalAlign = PdfTextHorizontalAlign.Right;
            converter.Footer.Add(text);
            #endregion
            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(htmlString, "");
            // save pdf document
            byte[] pdf = doc.Save();
            doc.Close();

            string FolderPath = ConfigurationManager.AppSettings["stdDetailsPDF"];
            string LocalFileNameWithPath = (FolderPath + "Preview.pdf");
            string FileNameWithPath = Server.MapPath(LocalFileNameWithPath);
            if (System.IO.File.Exists(FileNameWithPath))
                System.IO.File.Delete(FileNameWithPath);
            System.IO.File.WriteAllBytes(FileNameWithPath, pdf);
            return LocalFileNameWithPath;
        }
        #endregion

        #region Employee Assign Class
        public ActionResult SaveAggigEMP(string EmpID, string Class, string Section)
        {
            string Type = "Insert";
            SqlParameter[] prm = new SqlParameter[] {
                  new SqlParameter("@type", Type),
                  new SqlParameter("@EmpID", EmpID),
                  new SqlParameter("@ClassID", Class),
                  new SqlParameter("@SectionID", Section)
            };
            string OutPut = Convert.ToString(new SQLHelper().ExecuteScalar("SP_EmpClassMapping", prm, CommandType.StoredProcedure));
            return Json(OutPut, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowAssignEmpData(string EmpID)
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@type", "Select"),
                  new SqlParameter("@EmpID", EmpID)
            };
            DataTable dt = new SQLHelper().ExecuteDataTable("SP_EmpClassMapping", prm1, CommandType.StoredProcedure);            List<EmpAssignModel> AssignList = Utility.ConvertDataTableToClassObjectList<EmpAssignModel>(dt);
          
            return Json(AssignList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteEmpClass(string EmpID, string Class)
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@type", "Delete"),
                  new SqlParameter("@EmpID", EmpID),
                  new SqlParameter("@ClassID", Class),
            };
            string OutPut = Convert.ToString(new SQLHelper().ExecuteScalar("SP_EmpClassMapping", prm1, CommandType.StoredProcedure));
            return Json(OutPut, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}