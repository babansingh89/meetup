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
    public class ExpensesController : Controller
    {
        #region Expenses
        public ActionResult Expenses()
        {
            return View();
        }
        public ActionResult saveExpensesMaster(string Id, string ExpensesName)
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
                    new SqlParameter("ExpensesID", Id),
                    new SqlParameter("ExpensesName", ExpensesName)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Expenses", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewExpensesMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ExpensesID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Expenses", prm1, CommandType.StoredProcedure);
                List<Expensescls> list = Utility.ConvertDataTableToClassObjectList<Expensescls>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteExpensesMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ExpensesID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Expenses", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ExpensesEntry
        public ActionResult ExpensesEntry()
        {
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ExpensesList = Utility.GetDropDownList("SP_Expenses", "ExpensesID", "ExpensesName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult saveExpensesEntryMaster(string Id, string ExpensesID, string Desc, string Amount, string EntryDate
          , string PayMode, string BM_Name, string DraftNo, string DraftDate, string BranchName)
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
                    new SqlParameter("ExpEntID", Id),
                    new SqlParameter("ExpensesID", ExpensesID),
                    new SqlParameter("Desc", Desc),
                    new SqlParameter("Amount", Amount),
                    new SqlParameter("EntryDate", EntryDate),
                    new SqlParameter("PayMode", PayMode),
                    new SqlParameter("BM_Name", BM_Name),
                    new SqlParameter("DraftNo", DraftNo),
                    new SqlParameter("DraftDate", DraftDate==""?null:DraftDate),
                    new SqlParameter("BranchName", BranchName),
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ExpensesEntry", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewExpensesEntryMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ExpEntID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ExpensesEntry", prm1, CommandType.StoredProcedure);
                List<Expensescls> list = Utility.ConvertDataTableToClassObjectList<Expensescls>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteExpensesEntryMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ExpEntID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ExpensesEntry", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ExpensesReport
        public ActionResult ExpensesReport()
        {
            return View();
        }
        public ActionResult PrintExpenses(string From, string To)
        {
            string strimgSrc = "";
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "PrintExpenses"),
                    new SqlParameter("From", From),
                    new SqlParameter("To", To),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ExpensesEntry", prm1, CommandType.StoredProcedure);
                List<Expensescls> List = Utility.ConvertDataTableToClassObjectList<Expensescls>(dt);


                string OuterHTML = generateIDCardhtml(List);
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
        public string generateIDCardhtml(List<Expensescls> ls)
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

            str += "<div class='row' style='margin-top:10px;'>";
            str += "<div class='col-md-2'></div>";
            str += "<div class='col-md-10'>";
            str += "<p class='text-center'><b><u>EXPENSES REPORT</u></b></p>";
            str += " </div></div>";
            str += " <div class='row' style='margin-top:10px;'>";
            str += "  <div class='col-md-12'>";
            str += "<table class='blueTable'><tr><th>SL No.</th><th>Expenses</th><th>Desc</th><th>EntryDate</th><th>Payment</th><th>PayMode</th></tr>";
          
            decimal amt = 0;
            for (int i = 0; i < ls.Count; i++)
            {
                int sl = i + 1;
                amt = amt + Convert.ToDecimal(ls[i].Amount);
                str += "<tr><td>" + sl + "</<td><td>" + ls[i].ExpensesName + "</<td><td>" + ls[i].Desc + "</<td><td>" + ls[i].EntryDate + "</<td><td>" + ls[i].Amount + "</<td><td>" + ls[i].PayMode + "</<td> </tr>";

            }
            str += "<tr><td colspan='4' align='right'>Total:</<td><td colspan='2'>" + amt + "</<td> </tr>";

            str += " </table>";
            str += " </div>";
            str += " </div>";
            str += " <div class='row' style='margin-top:50px;'><div class='col-md-12'><div style = 'border-top: 1px dashed black; margin-left: 80%; margin-right: 5%;'> </div></div> <p style='margin-left: 80%;'><b> Administrative Officer</b></p>  </div>";

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
    }
}