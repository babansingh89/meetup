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
    public class ReportController : Controller
    {
        #region Student Performance
        public ActionResult StudentPerformance()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            return View();
        }
        public ActionResult PrintStudent(string Class, string Section, string From, string To)
        {
            string strimgSrc = "";
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    //new SqlParameter("Type", "PrintAttendance"),
                    new SqlParameter("From", From),
                    new SqlParameter("To", To),
                    new SqlParameter("Class", Class),
                    new SqlParameter("Section", Section),
                    new SqlParameter("AppID", (Convert.ToString( System.Web.HttpContext.Current.Session["AppID"])))
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_STPrint", prm1, CommandType.StoredProcedure);
                List<printStPer> List = Utility.ConvertDataTableToClassObjectList<printStPer>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);

                //string OuterHTML = PrintStudent(List);
                //string PDFpath = GenerateLoanPDF(OuterHTML);
                //return Json(PDFpath, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        private string PrintStudent(List<printStPer> ls)
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
            str += "   <div class='col-md-2'>";
            if (path != "")
                str += "       <img src = '" + path + "' style = 'width:150px;height:160px;margin-bottom:15px;' /> ";


            str += "    </div>";
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
            str += "<p class='text-center'><b><u>Student Performance</u></b></p>";
            str += "</div>";
            str += "<div class='row'>";
            str += "<div class='col-md-12'>";
            str += "<table class='blueTable'><tr><th>Class</th><th>Section</th><th>Reg No</th><th>Student Name</th><th>DOB</th><th>Phone</th><th>Working Days</th><th>Present</th><th>Absent</th><th>%</th></tr>";

            for (int i = 0; i < ls.Count; i++)
            {
                if (ls[i].SR_StudentName != null)
                {

                    str += "<tr><td>" + ls[i].ClassName + "</<td><td>" + ls[i].SectionName + "</<td><td>" + ls[i].SR_RegNo + "</<td><td>" + ls[i].SR_StudentName + "</<td><td>" + ls[i].SR_DOB + "</<td><td>" + ls[i].SR_PhNo + "</<td><td>" + ls[i].WD + "</<td><td>" + ls[i].Pass + "</<td><td>" + ls[i].Fail + "</<td><td>" + ls[i].Per + "</<td> </tr>";
                }
            }

            str += " </table>";
            str += " </div>";
            str += " </div>";

            str += "</body>";
            str += "</html>";
            return str;
        }
        #endregion

        #region Month Wise Collection
        public ActionResult MonthWiseCollection()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            return View();

        }

        public ActionResult PrintMonthWiseCollection(string Class, string Section, string From, string To)
        {
            string strimgSrc = "";
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "PrintAttendance"),
                    new SqlParameter("Class", Class),
                    new SqlParameter("Section", Section),
                    new SqlParameter("From", From),
                    new SqlParameter("To", To),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_EMPPrint", prm1, CommandType.StoredProcedure);
                List<printStMonth> List = Utility.ConvertDataTableToClassObjectList<printStMonth>(dt);


                string OuterHTML = PrintMonthWiseCollection(List);
                string PDFpath = GenerateLoanPDF(OuterHTML);
                return Json(PDFpath, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        private string PrintMonthWiseCollection(List<printStMonth> ls)
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
            str += "   <div class='col-md-2'>";
            if (path != "")
                str += "       <img src = '" + path + "' style = 'width:150px;height:160px;margin-bottom:15px;' /> ";


            str += "    </div>";
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
            str += "<p class='text-center'><b><u>Month Wise Collection</u></b></p>";
            str += "</div>";
            str += "<div class='row'>";
            str += "<div class='col-md-12'>";
            str += "<table class='blueTable'><tr><th>Class</th><th>Section</th><th>Receipt No</th><th>Date</th><th>Reg No</th><th>Student</th><th>Total Amt</th><th>Dis Amt</th><th>Pay Amt</th><th>Due Amt</th></tr>";

            for (int i = 0; i < ls.Count; i++)
            {
                if (ls[i].SFC_ReceiptNo != null)
                {
                    str += "<tr><td>" + ls[i].ClassName + "</<td><td>" + ls[i].SectionName + "</<td><td>" + ls[i].SFC_ReceiptNo + "</<td><td>" + ls[i].SFC_Date + "</<td><td>" + ls[i].SR_RegNo + "</<td><td>" + ls[i].SR_StudentName + "</<td><td>" + ls[i].SFC_TotalAmount + "</<td><td>" + ls[i].SFC_DiscountAmount + "</<td><td>" + ls[i].SFC_PaymentAmount + "</td><td>" + ls[i].SFC_DueAmount + " </td></tr>";
                }
            }

            str += " </table>";
            str += " </div>";
            str += " </div>";

            str += "</body>";
            str += "</html>";
            return str;
        }
        #endregion

        #region Fees Head Wise Collection
        public ActionResult FeesHeadWiseCollection()
        {
            return View();
        }
        public ActionResult PrintHeadWiseCollection(string From, string To)
        {
            string strimgSrc = "";
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "PrintAttendance"),
                    new SqlParameter("From", From),
                    new SqlParameter("To", To),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_FeeHeadPrint", prm1, CommandType.StoredProcedure);


                if (dt.Rows.Count > 0)
                {
                    string OuterHTML = PrintHeadWiseCollection(dt);
                    string PDFpath = GenerateLoanPDF(OuterHTML);
                    return Json(PDFpath, JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        private string PrintHeadWiseCollection(DataTable dt)
        {
            dt.Columns.Add("Total", typeof(decimal));
            foreach (DataRow row in dt.Rows)
            {
                decimal rowSum = 0;
                foreach (DataColumn col in dt.Columns)
                {
                    if (!row.IsNull(col))
                    {
                        string stringValue = row[col].ToString();
                        decimal d;
                        if (decimal.TryParse(stringValue, out d))
                            rowSum += d;
                    }
                }
                row.SetField("Total", rowSum);
            }
            DataRow rowfooter = dt.NewRow();
            rowfooter[0] = "<b>Total:</b>";
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                rowfooter[i] = dt.Compute("Sum([" + dt.Columns[i].ColumnName + "])", "[" + dt.Columns[i].ColumnName + "]> 0").ToString();
            }
            dt.Rows.Add(rowfooter);
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
            str += "   <div class='col-md-2'>";
            if (path != "")
                str += "       <img src = '" + path + "' style = 'width:150px;height:160px;margin-bottom:15px;' /> ";


            str += "    </div>";
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
            str += "<p class='text-center'><b><u>Fees Head Wise Collection</u></b></p>";
            str += "</div>";
            str += "<div class='row'>";
            str += "<div class='col-md-12'>";

            str += "<table class='blueTable'><tr>";
            foreach (DataColumn myColumn in dt.Columns)
            {
                str += "<th>" + myColumn.ColumnName + "</th>";
            }
            str += "</tr>";

            //for (int i = 0; i < dt.Rows.Count; i++)
            //          {
            //str += "<tr><td>" + ls[i].EM_EmpCode + "</<td><td>" + ls[i].EM_EmpName + "</<td><td>" + ls[i].EM_EmpContactNo + "</<td><td>" + ls[i].EM_EmpDOJ + "</<td><td>" + ls[i].WD + "</<td><td>" + ls[i].Pass + "</<td><td>" + ls[i].Fail + "</<td><td>" + ls[i].Per + "</<td> </tr>";
            //            }
            foreach (DataRow myRow in dt.Rows)
            {
                str += "<tr>";
                foreach (DataColumn myColumn in dt.Columns)
                {
                    str += "<td>" + myRow[myColumn.ColumnName].ToString() + "</td>";
                }
                str += "</tr>";
            }
            str += " </table>";
            str += " </div>";
            str += " </div>";
            str += "</body>";
            str += "</html>";
            return str;
        }
        #endregion

        #region Student Due
        public ActionResult StudentDue()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            return View();
        }

        public ActionResult StudentDueWise(string Class, string Section)
        {
            if (string.IsNullOrEmpty(Class) || Convert.ToInt32(Class) < 1)
            {
                Class = null;
            }
            if (string.IsNullOrEmpty(Section) || Convert.ToInt32(Section) < 1)
            {
                Section = null;
            }
            string strimgSrc = "";
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Class", Class),
                    new SqlParameter("Section", Section),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_STDuePrint", prm1, CommandType.StoredProcedure);
                List<printStDue> List = Utility.ConvertDataTableToClassObjectList<printStDue>(dt);


                string OuterHTML = StudentDueWise(List);
                string PDFpath = GenerateLoanPDF(OuterHTML);
                return Json(PDFpath, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        private string StudentDueWise(List<printStDue> ls)
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
            str += "   <div class='col-md-2'>";
            if (path != "")
                str += "       <img src = '" + path + "' style = 'width:150px;height:160px;margin-bottom:15px;' /> ";


            str += "    </div>";
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
            str += "<p class='text-center'><b><u>Student Due</u></b></p>";
            str += "</div>";
            str += "<div class='row'>";
            str += "<div class='col-md-12'>";
            str += "<table class='blueTable'><tr><th>Class</th><th>Section</th><th>RegNo</th><th>Student</th><th>Roll No</th><th>Ph No</th><th>Due</th></tr>";

            for (int i = 0; i < ls.Count; i++)
            {
                if (ls[i].Due != null)
                {

                    str += "<tr><td>" + ls[i].className + "</<td><td>" + ls[i].SectionName + "</<td><td>" + ls[i].SR_RegNo + "</<td><td>" + ls[i].SR_StudentName + "</<td><td>" + ls[i].SR_CurrentRollNo + "</<td><td>" + ls[i].SR_PhNo + "</<td><td>" + ls[i].Due + "</<td></tr>";
                }
            }

            str += " </table>";
            str += " </div>";
            str += " </div>";

            str += "</body>";
            str += "</html>";
            return str;
        }
        #endregion

        #region Student Registration
        public ActionResult StudentRegistration()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            return View();
        }
        public ActionResult PrintStudentRegistration(string Class, string Section)
        {
            string strimgSrc = "";
            try
            {
                if (string.IsNullOrEmpty(Class) || Convert.ToInt32(Class) < 1)
                {
                    Class = null;
                }
                if (string.IsNullOrEmpty(Section) || Convert.ToInt32(Section) < 1)
                {
                    Section = null;
                }
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "PrintAttendance"),
                    new SqlParameter("Class",Class),
                    new SqlParameter("Section", Section),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_STREGPrint", prm1, CommandType.StoredProcedure);
                List<printStRegistration> List = Utility.ConvertDataTableToClassObjectList<printStRegistration>(dt);


                string OuterHTML = PrintStudentRegistration(List);
                string PDFpath = GenerateLoanPDF(OuterHTML);
                return Json(PDFpath, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        private string PrintStudentRegistration(List<printStRegistration> ls)
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
           
            str += "   <div class='row'>";
            str += "   <div class='col-md-2'>";
           
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
            str += "<p class='text-center'><b><u>Student Registration</u></b></p>";
            str += "</div>";
            str += "<div class='row'>";
            str += "<div class='col-md-12'>";
            str += "<table class='blueTable'><tr><th>Class</th><th>Section</th><th>Reg No</th><th>Student Name</th><th>Roll No</th><th>Father Name</th><th>DOB</th><th>Phone</th></tr>";

            for (int i = 0; i < ls.Count; i++)
            {
                if (ls[i].SR_StudentName != null)
                {

                    str += "<tr><td>" + ls[i].ClassName + "</<td><td>" + ls[i].SectionName + "</<td><td>" + ls[i].SR_RegNo + "</<td><td>" + ls[i].SR_StudentName + "</<td><td>" + ls[i].SR_CurrentRollNo + "</<td><td>" + ls[i].SR_FatherName + "</<td><td>" + ls[i].SR_DOB + "</<td><td>" + ls[i].SR_PhNo + "</<td> </tr>";
                }
            }

            str += " </table>";
            str += " </div>";
            str += " </div>";

            str += "</body>";
            str += "</html>";
            return str;
        }
        #endregion


        #region Studence Attendance
        public ActionResult EmployeeAttendance()
        {
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select_Designation"),
            };
            ViewBag.DesignationList = Utility.GetDropDownList("SP_Employee", "DesID", "DesName", prm2, "", "", "Select");
            return View();
        }

        public ActionResult BindTeacher(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select_Teacher"),
                   new SqlParameter("@EM_DesgId", Id),
            };
                //ViewBag.TeacherList = Utility.GetDropDownList("SP_Employee", "EM_EmpId", "EM_EmpName", prm1, "", "", "Select");
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_Employee", prm1, CommandType.StoredProcedure);
                List<Employee> List = Utility.ConvertDataTableToClassObjectList<Employee>(dt.Tables[0]);

                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

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
        private byte[] GetBytesFromImage(String imageFile)
        {
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromFile(imageFile);
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}