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
using Razorpay.Api;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text;


namespace SchoolERP_System.Controllers
{
    [SessionAuthorizedAttribute]
    public class TransactionController : Controller
    {
        #region Fee Collection
        public ActionResult StudentFeesCollection(string _id)
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                new SqlParameter("Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("Type", "Select"),
            };
            ViewBag.SectionList = Utility.GetDropDownList("SP_Section", "SectionID", "SectionName", prm2, "", "", "Select");
            SqlParameter[] prm3 = new SqlParameter[] {
                  new SqlParameter("Type", "Select"),
            };
            ViewBag.StreamList = Utility.GetDropDownList("SP_Stream", "StreamID", "StreamName", prm3, "", "", "Select");

            if(_id !=null)
            {
                ViewBag.RegNo = _id;
            }
            return View();
        }
        public ActionResult GetStudentFee(string SR_No, string OptedMonth)
        {
            try
            {
                object[] mixArray = new object[3];
                SqlParameter[] prm1 = new SqlParameter[] {
                      new SqlParameter("type", "FeeDetails"),
                    new SqlParameter("RegNo", SR_No),
                    new SqlParameter("PayMonth", OptedMonth),
                    new SqlParameter("AppID", (Convert.ToString( System.Web.HttpContext.Current.Session["AppID"])))
                };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_StudentDetailsPrint", prm1, CommandType.StoredProcedure);
                List<FeeDetails> List = Utility.ConvertDataTableToClassObjectList<FeeDetails>(dt.Tables[0]);
                List<FeesDetails> Lists = Utility.ConvertDataTableToClassObjectList<FeesDetails>(dt.Tables[1]);
                List<FeesDetails> Is_PaymentDone = Utility.ConvertDataTableToClassObjectList<FeesDetails>(dt.Tables[2]);
                mixArray[0] = List;
                mixArray[1] = Lists;
                mixArray[2] = Is_PaymentDone;
                return Json(mixArray, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public DataTable GetdtFeeTable(List<FeeCollection> stddt)
        {
            GenLib objLib = new GenLib();
            DataTable dt = new DataTable();
            dt = objLib.ToDataTable(stddt);
            return dt;
        }
        public DataTable GetdtpayTable(List<Pay_Mode_Details> stddt)
        {
            GenLib objLib = new GenLib();
            DataTable dt = new DataTable();
            dt = objLib.ToDataTable(stddt);
            return dt;
        }
        public ActionResult SaveStudentFeesCollection(string SR_ID, string Date, string TotalNetPayable, string TotalLateFee, string TotalDiscount, 
         string OtherAmount,  string GrossPaymentAmount, string PayMode, string PayMonth, string TransactionNo, string TransactionDate, string Remarks)
        {
            try
            {
                string appid = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID;

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Insert"),
                    new SqlParameter("SR_ID", SR_ID),
                    new SqlParameter("TotalNetAmount", TotalNetPayable),
                    new SqlParameter("TotalLateAmount", TotalLateFee),
                    new SqlParameter("TotalDiscountAmount", TotalDiscount),
                     new SqlParameter("TotalOtherAmount", OtherAmount),
                    new SqlParameter("TotalPaymentAmount", GrossPaymentAmount),
                    new SqlParameter("PayMode", PayMode),
                    new SqlParameter("PayMonth", PayMonth),

                    new SqlParameter("TransactionNo", TransactionNo),
                    new SqlParameter("TransactionDate", TransactionDate),
                    new SqlParameter("Remarks", Remarks),
                    new SqlParameter("AppID", (Convert.ToString( System.Web.HttpContext.Current.Session["AppID"])))

                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_FeeCollection", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult saveStudentFeesCollection1(string SR_ID, string Date, string TotalPayable, string Discount, string PaymentAmount, string DueAmount, List<FeeCollection> DataFee, List<Pay_Mode_Details> Datapay, bool isSMS, string ph)
        {

            try
            {
                DataTable dtFee = GetdtFeeTable(DataFee);
                DataTable dtpay = GetdtpayTable(Datapay);

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Insert"),
                    new SqlParameter("SR_ID", SR_ID),
                    new SqlParameter("Date", Date),
                    new SqlParameter("TotalPayable", TotalPayable),
                    new SqlParameter("Discount", Discount),
                    new SqlParameter("PaymentAmount", PaymentAmount),
                    new SqlParameter("DueAmount", DueAmount),
                    new SqlParameter("DataFee", dtFee),
                    new SqlParameter("Datapay", dtpay)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_FeeCollection", prm1, CommandType.StoredProcedure));
                if (isSMS)
                {
                    HelperClass help = new HelperClass();
                    DataTable dtsms = help.ExecuteSMSSettings();
                    string SMSText = "Fee recieved Amount " + PaymentAmount + " !Thank You.";
                    string msg = help.sendSms(ph, SMSText, dtsms.Rows[0]["username"].ToString(), dtsms.Rows[0]["sendername"].ToString(), dtsms.Rows[0]["apikey"].ToString());
                    SqlParameter[] prm2 = new SqlParameter[] {
                    new SqlParameter("Type", "SMSInsert"),
                    new SqlParameter("StudentID", SR_ID),
                    new SqlParameter("SMSText", SMSText),
                    new SqlParameter("SMSType", "FEE")
                };
                    string Output1 = Convert.ToString(new SQLHelper().ExecuteScalar("SP_SMSInsert", prm2, CommandType.StoredProcedure));

                }
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Auto_RegNo(string SR_No)
        {
            try
            {
                object[] mixArray = new object[2];
                SqlParameter[] prm1 = new SqlParameter[] {
                      new SqlParameter("type", "AutoRegNo"),
                    new SqlParameter("RegNo", SR_No),
                    new SqlParameter("AppID", (Convert.ToString( System.Web.HttpContext.Current.Session["AppID"])))
                };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_StudentDetailsPrint", prm1, CommandType.StoredProcedure);
                List<FeeDetails> List = Utility.ConvertDataTableToClassObjectList<FeeDetails>(dt.Tables[0]);
                mixArray[0] = List;
                return Json(mixArray, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Fee Collection List
        public ActionResult StudentFeesCollectionList()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");

            return View();
        }
        public ActionResult ViewFeecollection(string RegNo, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("RegNo", RegNo)
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_StudentFeeCollection", prm1, CommandType.StoredProcedure);
                List<FeeCollectionDetails> List = Utility.ConvertDataTableToClassObjectList<FeeCollectionDetails>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteCollectionFees(string Id)
        {
            try
            {

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_FeeCollectionDetails", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
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
        public ActionResult FeePrintPDF(string StdId)
        {
            string strimgSrc = "";
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "PrintFee"),
                    new SqlParameter("ID", StdId)
                };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_FeeCollectionDetails", prm1, CommandType.StoredProcedure);
                List<FeeCollectionDetailsPrint> List = Utility.ConvertDataTableToClassObjectList<FeeCollectionDetailsPrint>(dt.Tables[0]);
                List<FeeCollectionPrint> Listtable = Utility.ConvertDataTableToClassObjectList<FeeCollectionPrint>(dt.Tables[1]);
                string OuterHTML = generateIDCardhtml(List[0], Listtable);
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
        public string generateIDCardhtml(FeeCollectionDetailsPrint stdlist, List<FeeCollectionPrint> ls)
        {
            string path = "";
            string str = "";
            str += "<html>";
            str += "<head>";;
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
            str += "   <div class='col-md-2'></div>";
            str += "  <div class='col-md-10'>";
            str += "  <p class='text-center'><b><u>Fees Receipt</u></b></p>";
            str += "  <div class='col-md-8'>";
            str += "  <p><span><b>Rep.No: </b></span><span> " + stdlist.SFC_ReceiptNo + "</span></p>";
            str += "    <p><span><b>Student Name: </b></span><span> " + stdlist.SR_StudentName + "</span></p>";
            str += "    <p><span><b>Class: </b></span><span> " + stdlist.ClassName + "</span></p>";
            str += "   <p><span><b>Roll No: </b></span><span> " + stdlist.SR_CurrentRollNo + "</span></p>";
            str += "    </div>";
            str += "   <div class='col-md-4'>";
            str += "    <p><span><b>Date.: </b></span><span> " + stdlist.SFC_Date + " </span></p>";
            str += "    <p><span><b>Student ID: </b></span><span> " + stdlist.SR_RegNo + "</span></p>";
            str += "    <p><span><b>Section: </b></span><span> " + stdlist.SectionName + "</span></p>";
            str += "    <p><span><b>Fee Amount: </b></span><span>" + stdlist.SFC_PaymentAmount + "</span></p>";
            str += " </div>";
            str += " </div>";
            str += " </div>";

            str += " <div class='row'>";
            str += "  <div class='col-md-12'>";
            str += "<table class='blueTable'><tr><th>SL No.</th><th>FeeHead Name</th><th>Month Year</th><th>Discount</th><th>Payment</th></tr>";
            decimal dis = 0;
            decimal amt = 0;
            for (int i = 0; i < ls.Count; i++)
            {
                if (ls[i].FeeHeadName != null)
                {
                    int sl = i + 1;
                    dis = dis + Convert.ToDecimal(ls[i].SFCT_DiscountAmount);
                    amt = amt + Convert.ToDecimal(ls[i].SFCT_PaymentAmount);
                    str += "<tr><td>" + sl + "</<td><td>" + ls[i].FeeHeadName + "</<td><td>" + ls[i].MonthYear + "</<td><td>" + ls[i].SFCT_DiscountAmount + "</<td><td>" + ls[i].SFCT_PaymentAmount + "</<td> </tr>";
                }
            }
            str += "<tr><td colspan='3' align='right'>Total:</<td><td>" + dis + "</<td><td>" + amt + "</<td> </tr>";

            str += " </table>";
            str += " </div>";
            str += " </div>";
            str += " <div class='row' style='margin-top:50px;'><div class='col-md-12'><div style = 'border-top: 1px dashed black; margin-left: 80%; margin-right: 5%;'> </div></div> <p style='margin-left: 80%;'><b> Administrative Officer</b></p>  </div>";

            //        str += " <div class='row' style='margin-top:50px;'>";
            //str += "  <div class='col-md-6'></div>";
            //str += " <div class='col-md-3'>";
            //str += " <div style = 'border-top: 1px dashed black;'></ div > ";
            //str += "<p><b> Administrative Officer</b></p>";
            //str += " </div>";
            //str += "<div class='col-md-2'></div>";
            //str += "</div>";
            //str += "</div>";
            str += "</body>";
            str += "</html>";
            return str;
        }
        #endregion

        #region Defaulter List
        public ActionResult DefaulterList()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.MSGList = Utility.GetDropDownList("SP_ST", "STID", "STName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult viewDefaulterList(string STID, string section)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "DueBYClassSection"),
                    new SqlParameter("STID", STID),
                    new SqlParameter("STName", section),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ST", prm1, CommandType.StoredProcedure);
                List<SelectStudent> STList = Utility.ConvertDataTableToClassObjectList<SelectStudent>(dt);
                return Json(STList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult saveDefaulterList(List<SMSStudent> StudentID, string SMSText, string SMSType)
        {
            try
            {
                HelperClass help = new HelperClass();
                var strID = string.Join(",", StudentID.Select(i => i.StudentID));
                var strNo = string.Join(",", StudentID.Select(i => i.Mobile));
                // string msg = help.sendSms(strNo, SMSText);
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SMSInsert"),
                    new SqlParameter("StudentID", strID),
                    new SqlParameter("SMSText", SMSText),
                    new SqlParameter("SMSType", SMSType)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_SMSInsert", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Fee Collection By parents 
        public ActionResult FeesCollection()
        {
            return View();
        }
        public ActionResult viewFeecollectionByparents(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ID",((loggedInParents)System.Web.HttpContext.Current.Session["loggedInParents"]).StudentID)
            };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_FeeCollectionDetails", prm1, CommandType.StoredProcedure);
                List<FeeCollectionDetails> List = Utility.ConvertDataTableToClassObjectList<FeeCollectionDetails>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult FeesCollectionByParents()
        {

            return View();
        }
        public ActionResult VerifyRazorpaySignature(string razorpay_order_id, string razorpay_payment_id, string razorpay_signature)
        {
            try
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>();

                attributes.Add("razorpay_payment_id", razorpay_payment_id);
                attributes.Add("razorpay_order_id", razorpay_order_id);
                attributes.Add("razorpay_signature", razorpay_signature);

                Utils.verifyPaymentSignature(attributes);


                //if (generated_signature == razorpay_signature)
                //{
                //    return Json("payment is successful", JsonRequestBehavior.AllowGet);
                //}


                return Json("Sucess", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        // --   PAY u

        public ActionResult FeesPayementByParents()
        {
            string surl = ((HttpContext.Request.ServerVariables["HTTPS"] != "" && HttpContext.Request.ServerVariables["HTTP_HOST"] != "off") || HttpContext.Request.ServerVariables["SERVER_PORT"] == "443") ? "https://" : "http://";
            surl += HttpContext.Request.ServerVariables["HTTP_HOST"] + HttpContext.Request.ServerVariables["REQUEST_URI"] + "/Response.aspx";
            Session.Add("surl", surl);

            Random r = new Random();
            string txnid = "Txn" + r.Next(100, 9999);
            Session.Add("txnid", txnid);

            return View();
        }
        public string Generatetxnid()
        {

            Random rnd = new Random();
            string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
            string txnid1 = strHash.ToString().Substring(0, 20);

            return txnid1;
        }
        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }
        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2").ToLower());
            }
            return result.ToString();
        }
        #endregion

        #region Bengali Fees collection List
        public ActionResult StudentFeesCollectionList_Bengali()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");

            return View();
        }
        public ActionResult StudentFeesCollection_Bengali(string _id)
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                new SqlParameter("Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("Type", "Select"),
            };
            ViewBag.SectionList = Utility.GetDropDownList("SP_Section", "SectionID", "SectionName", prm2, "", "", "Select");
            SqlParameter[] prm3 = new SqlParameter[] {
                  new SqlParameter("Type", "Select"),
            };
            ViewBag.StreamList = Utility.GetDropDownList("SP_Stream", "StreamID", "StreamName", prm3, "", "", "Select");

            if (_id != null)
            {
                ViewBag.RegNo = _id;
            }
            return View();
        }
        public ActionResult SaveStudentFeesCollection_Bengali(string SR_ID, string Date, string TotalNetPayable, string TotalLateFee, string TotalDiscount,
 string OtherAmount, string GrossPaymentAmount, string PayMode, string PayMonth, string TransactionNo, string TransactionDate, string Remarks)
        {
            try
            {
                string appid = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID;

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Insert"),
                    new SqlParameter("SR_ID", SR_ID),
                    new SqlParameter("TotalNetAmount", TotalNetPayable),
                    new SqlParameter("TotalLateAmount", TotalLateFee),
                    new SqlParameter("TotalDiscountAmount", TotalDiscount),
                     new SqlParameter("TotalOtherAmount", OtherAmount),
                    new SqlParameter("TotalPaymentAmount", GrossPaymentAmount),
                    new SqlParameter("PayMode", PayMode),
                    new SqlParameter("PayMonth", PayMonth),

                    new SqlParameter("TransactionNo", TransactionNo),
                    new SqlParameter("TransactionDate", TransactionDate),
                    new SqlParameter("Remarks", Remarks),
                    new SqlParameter("AppID", (Convert.ToString( System.Web.HttpContext.Current.Session["AppID"])))

                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_FeeCollection_Bengali", prm1, CommandType.StoredProcedure));
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