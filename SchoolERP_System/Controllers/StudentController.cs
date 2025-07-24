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
    public class StudentController : Controller
    {
        #region Enquiry
        public ActionResult Enquiry()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            return View();
        }
        public ActionResult saveEnquiry(string EnquiryID, string ClassID, string E_Type, string E_Date, string StudentName, string Gender, string DOB, string FatherName, string Email, string Mobile, string NextFollowDate, string NextFollowUpNote, List<EnquiryDetails> Datadt)
        {
            try
            {

                GenLib objLib = new GenLib();
                DataTable dt = new DataTable();
                if (Datadt.Count >= 1)
                    dt = objLib.ToDataTable(Datadt);

                string Type = "";
                if (EnquiryID == "" || EnquiryID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("EnquiryID", EnquiryID),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("E_Date", E_Date),
                    new SqlParameter("E_Type", E_Type),
                    new SqlParameter("StudentName", StudentName),
                    new SqlParameter("Gender", Gender),
                    new SqlParameter("DOB", DOB),
                    new SqlParameter("FatherName", FatherName),
                    new SqlParameter("Email", Email),
                    new SqlParameter("Mobile", Mobile),
                    new SqlParameter("NextFollowDate", NextFollowDate),
                    new SqlParameter("NextFollowUpNote", NextFollowUpNote),
                    new SqlParameter("myTableType", dt)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Enquiry", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewEnquiry(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("EnquiryID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Enquiry", prm1, CommandType.StoredProcedure);
                List<Enquiry> List = Utility.ConvertDataTableToClassObjectList<Enquiry>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteEnquiry(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("EnquiryID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Enquiry", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region PreAdmitted
        public ActionResult PreAdmitted()
        {
            return View();
        }
        public ActionResult ViewPreAdmitted(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SR_ClassID", Id),
                    new SqlParameter("AppID", (Convert.ToString( System.Web.HttpContext.Current.Session["AppID"])))
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_StudentRegistration", prm1, CommandType.StoredProcedure);
                List<SR> List = Utility.ConvertDataTableToClassObjectList<SR>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeletePreAdmitted(string Id)
        {
            try
            {

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("SR_ID", Id),
                    new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_StudentRegistration", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult StudentRegistration()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");

            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.RouteList = Utility.GetDropDownList("SP_Route", "RouteID", "RouteName", prm2, "", "", "Select");

            SqlParameter[] prm3 = new SqlParameter[] {
                  new SqlParameter("@Type", "AppInfo"),
                  new SqlParameter("AppID", (Convert.ToString( System.Web.HttpContext.Current.Session["AppID"])))
            };
            ViewBag.ActiveMonth = Utility.GetDropDownList("SP_Route", "ID", "VALUE", prm3, "", "", "Select");

            return View();
        }
        public ActionResult BindStream(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "SelectSectionStream"),
                    new SqlParameter("SR_ClassID", Id),
                };
                DataSet dt = new SQLHelper().ExecuteDataSet("SP_StudentRegistration", prm1, CommandType.StoredProcedure);
                if (Type == "S")
                {
                    List<Streamcl> List = Utility.ConvertDataTableToClassObjectList<Streamcl>(dt.Tables[0]);
                    return Json(List, JsonRequestBehavior.AllowGet);
                }

                List<Section> ListSection = Utility.ConvertDataTableToClassObjectList<Section>(dt.Tables[1]);

                return Json(ListSection, JsonRequestBehavior.AllowGet);
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
                string _SR_Date = Request.Form.Get("_SR_Date");
                string _SR_StudentName = Request.Form.Get("_SR_StudentName");
                string _SR_FatherName = Request.Form.Get("_SR_FatherName");
                string _SR_MotherName = Request.Form.Get("_SR_MotherName");
                string _SR_FatherOccupation = Request.Form.Get("_SR_FatherOccupation");
                string _SR_MotherOccupation = Request.Form.Get("_SR_MotherOccupation");
                string _SR_Shift = Request.Form.Get("_SR_Shift");
                string _SR_SMS = Request.Form.Get("_SR_SMS");
                string _SR_Email = Request.Form.Get("_SR_Email");
                string _SR_PhNo = Request.Form.Get("_SR_PhNo");
                string _SR_AlternetContactNo = Request.Form.Get("_SR_AlternetContactNo");
                string _SR_DOB = Request.Form.Get("_SR_DOB");
                string _SR_Gender = Request.Form.Get("_SR_Gender");
                string _SR_PunchingCode = Request.Form.Get("_SR_PunchingCode");
                string _SR_LocalGurdianName = Request.Form.Get("_SR_LocalGurdianName");
                string _SR_LocalGurdianMobile = Request.Form.Get("_SR_LocalGurdianMobile");
                string _SR_Addrees = Request.Form.Get("_SR_Addrees");
                string _SR_ClassID = Request.Form.Get("_SR_ClassID");
                string _SR_CategoryID = Request.Form.Get("_SR_CategoryID");
                string _SR_StreamID = Request.Form.Get("_SR_StreamID");
                string _SR_SectionID = Request.Form.Get("_SR_SectionID");
                string _IsDiscount = Request.Form.Get("_IsDiscount");
                string _IsAvailBus = Request.Form.Get("_IsAvailBus");
                string _RouteID = Request.Form.Get("_RouteID");
                string _SR_EN_ID = Request.Form.Get("_SR_EN_ID");
                string _SR_Caste = Request.Form.Get("_SR_Caste");
                string _SR_OptedBusMonth = Request.Form.Get("_SR_OptedBusMonth"); 
                string _SR_OptedHostelMonth = Request.Form.Get("_SR_OptedHostelMonth");

                var _SR_Pic = System.Web.HttpContext.Current.Request.Files["_SR_Pic"];
                string imgpath = null;
                if (_SR_Pic != null)
                {
                    var fileName = Path.GetFileName(_SR_Pic.FileName);
                    var extention = Path.GetExtension(_SR_Pic.FileName);
                    var filenamewithoutextension = Path.GetFileNameWithoutExtension(_SR_Pic.FileName);
                    string FileName = DateTime.Now.GetHashCode().ToString("x") + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + System.IO.Path.GetExtension(fileName);
                    imgpath = Path.Combine(Server.MapPath("/UploadedImage/" + FileName));
                    _SR_Pic.SaveAs(imgpath);
                    imgpath = FileName;
                }

                var _SR_AddressPic = System.Web.HttpContext.Current.Request.Files["_SR_AddressPic"];
                string imgpath1 = null;
                if (_SR_AddressPic != null)
                {
                    var fileName = Path.GetFileName(_SR_AddressPic.FileName);
                    var extention = Path.GetExtension(_SR_AddressPic.FileName);
                    var filenamewithoutextension = Path.GetFileNameWithoutExtension(_SR_AddressPic.FileName);
                    string FileName = DateTime.Now.GetHashCode().ToString("x") + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + System.IO.Path.GetExtension(fileName);
                    imgpath1 = Path.Combine(Server.MapPath("/UploadedImage/" + FileName));
                    _SR_AddressPic.SaveAs(imgpath1);
                    imgpath1 = FileName;
                }

                var _SR_CirtificatePic = System.Web.HttpContext.Current.Request.Files["_SR_CirtificatePic"];
                string imgpath2 = null;
                if (_SR_CirtificatePic != null)
                {
                    var fileName = Path.GetFileName(_SR_CirtificatePic.FileName);
                    var extention = Path.GetExtension(_SR_CirtificatePic.FileName);
                    var filenamewithoutextension = Path.GetFileNameWithoutExtension(_SR_CirtificatePic.FileName);
                    string FileName = DateTime.Now.GetHashCode().ToString("x") + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + System.IO.Path.GetExtension(fileName);
                    imgpath2 = Path.Combine(Server.MapPath("/UploadedImage/" + FileName));
                    _SR_CirtificatePic.SaveAs(imgpath2);
                    imgpath2 = FileName;
                }
                string Type = "";
                bool enq = false;
                if (_SR_ID.Contains("E_"))
                {
                    enq = true;
                    string[] str = _SR_ID.Split('_');
                    _SR_ID = str[1];
                    Type = "Insert";
                }
                else if (_SR_ID == "" || _SR_ID == "0")
                {
                    Type = "Insert";
                }
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                     new SqlParameter("SR_ID", _SR_ID),
                     new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID),
                     new SqlParameter("SR_Date", _SR_Date),
                     new SqlParameter("SR_StudentName", _SR_StudentName),
                     new SqlParameter("SR_FatherName", _SR_FatherName),
                     new SqlParameter("SR_MotherName", _SR_MotherName),
                     new SqlParameter("SR_FatherOccupation", _SR_FatherOccupation.Replace("Select","")),
                     new SqlParameter("SR_MotherOccupation", _SR_MotherOccupation.Replace("Select","")),
                     new SqlParameter("SR_SMS", _SR_SMS),
                     new SqlParameter("SR_Email", _SR_Email),
                     new SqlParameter("SR_PhNo", _SR_PhNo),
                     new SqlParameter("SR_AlternetContactNo", _SR_AlternetContactNo),
                     new SqlParameter("SR_DOB", _SR_DOB),
                     new SqlParameter("SR_Gender", _SR_Gender),
                     new SqlParameter("SR_PunchingCode", _SR_PunchingCode),
                     new SqlParameter("SR_LocalGurdianName", _SR_LocalGurdianName),
                     new SqlParameter("SR_LocalGurdianMobile", _SR_LocalGurdianMobile),
                     new SqlParameter("SR_Addrees", _SR_Addrees),
                     new SqlParameter("SR_ClassID", _SR_ClassID),
                     new SqlParameter("SR_CategoryID", _SR_CategoryID),
                     new SqlParameter("SR_StreamID", _SR_StreamID==null),
                     new SqlParameter("SR_SectionID", _SR_SectionID),
                     new SqlParameter("SR_Pic", imgpath),
                     new SqlParameter("SR_AddressPic", imgpath1),
                     new SqlParameter("SR_CirtificatePic", imgpath2),
                     new SqlParameter("IsDiscount", _IsDiscount),
                     new SqlParameter("IsAvailBus", _IsAvailBus),
                     new SqlParameter("RouteID", _RouteID.Replace("-1",null)),
                     new SqlParameter("SR_EN_ID", _SR_EN_ID),
                     new SqlParameter("SR_Caste", _SR_Caste.Replace("Select","")),
                     new SqlParameter("SR_OptedBusMonth", _SR_OptedBusMonth),
                     new SqlParameter("SR_OptedHostelMonth", _SR_OptedHostelMonth),
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_StudentRegistration", prm1, CommandType.StoredProcedure));

                if (enq && Output == "InsertSuccessful")
                {
                    SqlParameter[] prm2 = new SqlParameter[] {
                    new SqlParameter("Type", "UpdateEnq"),
               new SqlParameter("SR_ID", _SR_ID),
                };
                    string Output1 = Convert.ToString(new SQLHelper().ExecuteScalar("SP_StudentRegistration", prm2, CommandType.StoredProcedure));
                }
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Registration
        public ActionResult Registration()
        {
            return View();
        }
        public ActionResult PrintPDF(string StdId)
        {
            string strimgSrc = "";
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Print"),
                    new SqlParameter("SR_ID", StdId),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_StudentDetailsPrint", prm1, CommandType.StoredProcedure);
                List<SR> List = Utility.ConvertDataTableToClassObjectList<SR>(dt);

                if (List[0].SR_Pic != "")
                {
                    // List[0].SR_Pic = "a7cfb8bb_07112019205429.jpg";
                    byte[] ImgByte = GetBytesFromImage(Server.MapPath("/UploadedImage/" + List[0].SR_Pic));
                    var base64 = Convert.ToBase64String(ImgByte);
                    strimgSrc = String.Format("data:image/gif;base64,{0}", base64);
                }
                else
                {
                    byte[] ImgByte = GetBytesFromImage(Server.MapPath("/UploadedImage/teststudent.png"));
                    var base64 = Convert.ToBase64String(ImgByte);
                    strimgSrc = String.Format("data:image/gif;base64,{0}", base64);
                }
                string OuterHTML = generateHtml(List[0], strimgSrc);
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

        public string generateHtml(SR stdlist, string ImageByte)
        {
            string str = "";
            str += "<html>";
            str += "<head>";
            str += "</head>";
            str += "<body>";
            str += "<h4 style='margin-bottom: -18px;' class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).Name + "</b></h4>";
            str += "<h4 class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchAddress + "</b></h4>";
            str += "<h4 class='text-center'><b>Student Detail</b></h4>";
            str += "<div class='row' style='margin:15px 50px 15px 50px;padding: 10px 10px 10px 10px;border:1px solid black;'>";
            str += "<div class='col-md-6'>";
            str += "<img src='" + ImageByte + "' style='width:150px;height:160px;margin-bottom:15px;' />";
            str += "<p><span><b>Address: </b></span><span>" + stdlist.SR_Addrees + "</span></p>";
            str += "<p><span><b>Roll No.: </b></span><span>" + stdlist.SR_CurrentRollNo + "</span></p>";
            str += "<p><span><b>Class Name: </b></span><span>" + stdlist.ClassName + "</span></p>";
            str += "<p><span><b>Father Name: </b></span><span>" + stdlist.SR_FatherName + "</span></p>";
            str += "<p><span><b>Mother Name: </b></span><span>" + stdlist.SR_MotherName + "</span></p>";
            str += "<p><span><b>Local Guardian Name: </b></span><span>" + stdlist.SR_LocalGurdianName + "</span></p>";
            str += "</div>";
            str += "<div class='col-md-6'>";
            str += "<p><span><b>Student Name: </b></span><span>" + stdlist.SR_StudentName + "</span></p>";
            str += "<p><span><b>Registration No.: </b></span><span>" + stdlist.SR_RegNo + "</span></p>";
            str += "<p><span><b>Registration Date: </b></span><span>" + stdlist.SR_Date + "</span></p>";
            str += "<p><span><b>School Reg.No: </b></span><span>" + stdlist.SR_RegNo + "</span></p>";

            str += "<p><span><b>D.O.B: </b></span><span>" + stdlist.SR_DOB + "</span></p>";
            str += "<p><span><b>Phone No. </b></span><span>" + stdlist.SR_PhNo + "</span></p>";


            str += "<p><span><b>Gender: </b></span><span>" + stdlist.SR_Gender + "</span></p>";

            str += "<p><span><b>Section Name: </b></span><span>" + stdlist.SR_SectionID + "</span></p>";
            str += "<p><span><b>Father Occupation: </b></span>" + stdlist.SR_FatherOccupation + "<span></span></p>";
            str += "<p><span><b>Mother Occupation: </b></span><span>" + stdlist.SR_MotherOccupation + "</span></p>";
            str += "<p><span><b>Local Guardian Phone: </b></span><span>" + stdlist.SR_LocalGurdianMobile + "</span></p>";
            str += "<p><span><b>User ID: </b></span><span>" + stdlist.UserID + "</span></p>";
            str += "</div>";
            str += "</div>";
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

        public ActionResult EditStudent(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SR_ID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_StudentRegistration", prm1, CommandType.StoredProcedure);
                if (Type == "SelectByID")
                {
                    List<SR> List = Utility.ConvertDataTableToClassObjectList<SR>(dt);
                    return Json(List, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<Enquiry> List = Utility.ConvertDataTableToClassObjectList<Enquiry>(dt);
                    return Json(List, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GenerateLogin(string Id)
        {
            try
            {

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "GenerateLogin"),
                    new SqlParameter("SR_ID", Id),
                    new SqlParameter("AppID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).AppID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_StudentLogin", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region RollNoSettings
        public ActionResult RollNoSettings()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            return View();
        }
        public ActionResult viewRollNoSettings(string ClassID, string SectionID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                      new SqlParameter("type", "SelectStudent"),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SectionID", SectionID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_RollNoSettings", prm1, CommandType.StoredProcedure);
                List<RollSettings> List = Utility.ConvertDataTableToClassObjectList<RollSettings>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult saveStudentRollNo(List<RollSettings> Datadt)
        {
            try
            {
                DataTable dttAdr = GetAttdanceTable(Datadt);

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Update"),
                    new SqlParameter("myTableType", dttAdr)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_RollNoSettings", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public DataTable GetAttdanceTable(List<RollSettings> stddt)
        {
            GenLib objLib = new GenLib();
            DataTable dt = new DataTable();
            dt = objLib.ToDataTable(stddt);
            dt.Columns.Remove("SR_StudentName");
            dt.Columns.Remove("SR_RegNo");
            return dt;
        }
        #endregion

        #region AdminCard
        public ActionResult AdminCard()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            return View();
        }
        public ActionResult BindExam(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "ExamByClass"),
                    new SqlParameter("ClassID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_StudentDetailsPrint", prm1, CommandType.StoredProcedure);

                List<Exam> List = Utility.ConvertDataTableToClassObjectList<Exam>(dt);

                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintAdmitCard(string ClassID, string SectionID, string ExamID)
        {
            string strimgSrc = "";
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "PrintAdmit"),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SectionID", SectionID),
                    new SqlParameter("ExamID", ExamID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_StudentDetailsPrint", prm1, CommandType.StoredProcedure);
                List<printAdmitCls> List = Utility.ConvertDataTableToClassObjectList<printAdmitCls>(dt);


                string OuterHTML = generateAdmithtml(List);
                //string PDFpath = GenerateLoanPDF(OuterHTML);
                //return Json(PDFpath, JsonRequestBehavior.AllowGet);
                var htmlToPdf = new HtmlToPdfConverter();
                var output = htmlToPdf.GeneratePdf(OuterHTML);
                string FolderPath = ConfigurationManager.AppSettings["stdDetailsPDF"];
                string LocalFileNameWithPath = (FolderPath + "Preview.pdf");
                string FileNameWithPath = Server.MapPath(LocalFileNameWithPath);
                if (System.IO.File.Exists(FileNameWithPath))
                    System.IO.File.Delete(FileNameWithPath);
                System.IO.File.WriteAllBytes(FileNameWithPath, output);
                return Json(LocalFileNameWithPath, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public string generateAdmithtml(List<printAdmitCls> stdlist)
        {
            string path = "";
            string str = "";
            str += "<html>";
            str += "<head>";
            str += "</head>";
            str += "<body>";
            for (int i = 0; i < stdlist.Count; i++)
            {
                if (i != 0)
                {
                    if (i % 3 == 0)
                    {
                       // str += "<div class='card' style='margin-top:450px;'><button class='buttonH'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).Name + "</b></button><button class='buttonG'>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchAddress + "</button> <div class=row> <div class='col-md-8' style='margin: 10 0 5 0;'>  <span><b><i>IDENTITY CARD</i></b></span></div>  </div> <div class=row> <div class='col-md-8' style='margin: 15 0 5 0;'>  <span><b>" + stdlist[i].SR_StudentName + "</b></span></div>  </div> <div class=row>  <div class='col-md-6'>  <span><b>  Class: </b></span><span>" + stdlist[i].ClassName + "</span></br>  </div> <div class='col-md-6'>    <span><b>Roll No: </b></span><span>" + stdlist[i].SR_CurrentRollNo + "</span> </div>  </div>  <div class=row>  <div class='col-md-8'>  <span><b> Contact No: </b></span><span>" + stdlist[i].SR_PhNo + "</span></div>  </div>   <div class=row>  <div class='col-md-8'>  <span><b> Address: </b></span><span>" + stdlist[i].SR_Addrees + "</span></div>  </div>  <p><button class='buttonI'>PRINCIPAL</button></p></div>";
                        #region html
                        str += "<div style='margin: 200px 50px 15px 50px; padding: 10px 10px 10px 10px; border: 1px solid black;'>";
                        str += "<div class='row'>";
                        str += "<div class='col-md-2'>";
                        str += "<img src = '" + path + "' style='width:130px;height:120px;margin-bottom:15px;'/> ";
                        str += " </div>";
                        str += "<div class='col-md-10'>";
                        str += "<div class='divHeader'>";
                        str += "       <p class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).Name + "</b></p>";
                        str += "      <p class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchAddress + "</b></p>";
                        str += "    </div>";
                        str += " </div>";
                        str += " </div>";
                        str += "<div class='row'>";
                        str += "<div class='col-md-2'></div>";
                        str += "<div class='col-md-10'>";
                        str += "<p class='text-center'><b><u>Admit Card</u></b></p>";
                        str += "<div class='col-md-8'>";
                        str += "<p><span><b> Reg No: </b></span><span>" + stdlist[i].SR_RegNo + "</span></p>";
                        str += "<p><span><b>Class: </b></span><span> " + stdlist[i].SR_StudentName + "</span></p>";
                        str += "<p><span><b>Exam: </b></span><span> " + stdlist[i].ExamName + "</span></p>";
                        str += "</div>";
                        str += "<div class='col-md-4'>";
                        str += "<p><span><b>Name: </b></span><span>" + stdlist[i].SR_StudentName + "</span></p>";
                        str += "<p><span><b>Roll No.: </b></span><span> " + stdlist[i].SR_CurrentRollNo + " </span></p>";
                        str += "<p><span><b>Section: </b></span><span> " + stdlist[i].SectionName + "</span></p>";
                        str += "</div>";
                        str += "</div>";
                        str += "</div>";
                        str += " <div class='row' style='margin-top:25px;'><div class='col-md-12'><div style = 'border-top: 1px dashed black; margin-left: 80%; margin-right: 5%;'> </div></div> <p style='margin-left: 80%;'><b> Administrative Officer</b></p>  </div>";
                        str += "</div>";
                        #endregion
                    }
                    else
                    {
                        #region html
                        str += "<div style = 'margin: 8px 50px 15px 50px; padding: 10px 10px 10px 10px; border: 1px solid black;'>";
                        str += "<div class='row'>";
                        str += "<div class='col-md-2'>";
                        str += "<img src = '" + path + "' style='width:130px;height:120px;margin-bottom:15px;'/> ";
                        str += " </div>";
                        str += "<div class='col-md-10'>";
                        str += "<div class='divHeader'>";
                        str += "       <p class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).Name + "</b></p>";
                        str += "      <p class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchAddress + "</b></p>";
                        str += "    </div>";
                        str += " </div>";
                        str += " </div>";
                        str += "<div class='row'>";
                        str += "<div class='col-md-2'></div>";
                        str += "<div class='col-md-10'>";
                        str += "<p class='text-center'><b><u>Admit Card</u></b></p>";
                        str += "<div class='col-md-8'>";
                        str += "<p><span><b> Reg No: </b></span><span>" + stdlist[i].SR_RegNo + "</span></p>";
                        str += "<p><span><b>Class: </b></span><span> " + stdlist[i].SR_StudentName + "</span></p>";
                        str += "<p><span><b>Exam: </b></span><span> " + stdlist[i].ExamName + "</span></p>";
                        str += "</div>";
                        str += "<div class='col-md-4'>";
                        str += "<p><span><b>Name: </b></span><span>" + stdlist[i].SR_StudentName + "</span></p>";
                        str += "<p><span><b>Roll No.: </b></span><span> " + stdlist[i].SR_CurrentRollNo + " </span></p>";
                        str += "<p><span><b>Section: </b></span><span> " + stdlist[i].SectionName + "</span></p>";
                        str += "</div>";
                        str += "</div>";
                        str += "</div>";
                        str += " <div class='row' style='margin-top:25px;'><div class='col-md-12'><div style = 'border-top: 1px dashed black; margin-left: 80%; margin-right: 5%;'> </div></div> <p style='margin-left: 80%;'><b> Administrative Officer</b></p>  </div>";
                        str += "</div>";
                        #endregion
                    }
                }
                else
                {
                    #region html
                    str += "<div style = 'margin: 8px 50px 15px 50px; padding: 10px 10px 10px 10px; border: 1px solid black;'>";
                    str += "<div class='row'>";
                    str += "<div class='col-md-2'>";
                    str += "<img src = '" + path + "' style='width:130px;height:120px;margin-bottom:15px;'/> ";
                    str += " </div>";
                    str += "<div class='col-md-10'>";
                    str += "<div class='divHeader'>";
                    str += "       <p class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).Name + "</b></p>";
                    str += "      <p class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchAddress + "</b></p>";
                    str += "    </div>";
                    str += " </div>";
                    str += " </div>";
                    str += "<div class='row'>";
                    str += "<div class='col-md-2'></div>";
                    str += "<div class='col-md-10'>";
                    str += "<p class='text-center'><b><u>Admit Card</u></b></p>";
                    str += "<div class='col-md-8'>";
                    str += "<p><span><b> Reg No: </b></span><span>" + stdlist[i].SR_RegNo + "</span></p>";
                    str += "<p><span><b>Class: </b></span><span> " + stdlist[i].SR_StudentName + "</span></p>";
                    str += "<p><span><b>Exam: </b></span><span> " + stdlist[i].ExamName + "</span></p>";
                    str += "</div>";
                    str += "<div class='col-md-4'>";
                    str += "<p><span><b>Name: </b></span><span>" + stdlist[i].SR_StudentName + "</span></p>";
                    str += "<p><span><b>Roll No.: </b></span><span> " + stdlist[i].SR_CurrentRollNo + " </span></p>";
                    str += "<p><span><b>Section: </b></span><span> " + stdlist[i].SectionName + "</span></p>";
                    str += "</div>";
                    str += "</div>";
                    str += "</div>";
                    str += " <div class='row' style='margin-top:25px;'><div class='col-md-12'><div style = 'border-top: 1px dashed black; margin-left: 80%; margin-right: 5%;'> </div></div> <p style='margin-left: 80%;'><b> Administrative Officer</b></p>  </div>";
                    str += "</div>";
                    #endregion 
                }
            }
                str += "</body>";
            str += "</html>";

            return str;
        }
        #endregion

        #region Promotion
        public ActionResult Promotion()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectBind"),
            };
            ViewBag.SessionList = Utility.GetDropDownList("SP_Session", "SessionID", "SessionName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult viewPromotion(string ClassID, string SessionID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                      new SqlParameter("type", "SelectStudent"),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SessionID", SessionID)
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Promotion", prm1, CommandType.StoredProcedure);
                List<Promotion> List = Utility.ConvertDataTableToClassObjectList<Promotion>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult savePromotionMaster(string ClassID, string SessionID, List<SMSStudent> StudentID)
        {
            try
            {
                var strID = string.Join(",", StudentID.Select(i => i.StudentID));

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Insert"),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SessionID", SessionID ),
                    new SqlParameter("StudentID", strID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Promotion", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region IDCard
        public ActionResult IDCard()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            return View();
        }
        public ActionResult PrintIDCard(string ClassID, string SectionID)
        {
            string strimgSrc = "";
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "PrintIDCard"),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("SectionID", SectionID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_StudentDetailsPrint", prm1, CommandType.StoredProcedure);
                List<printIDCls> List = Utility.ConvertDataTableToClassObjectList<printIDCls>(dt);

                string OuterHTML = generateIDCardhtml(List);
                var htmlToPdf = new HtmlToPdfConverter();
                var output = htmlToPdf.GeneratePdf(OuterHTML);
                string FolderPath = ConfigurationManager.AppSettings["stdDetailsPDF"];
                string LocalFileNameWithPath = (FolderPath + "Preview.pdf");
                string FileNameWithPath = Server.MapPath(LocalFileNameWithPath);
                if (System.IO.File.Exists(FileNameWithPath))
                    System.IO.File.Delete(FileNameWithPath);
                System.IO.File.WriteAllBytes(FileNameWithPath, output);
 
                return Json(LocalFileNameWithPath, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public string generateIDCardhtml(List<printIDCls> stdlist)
        {

            string str = "";
                     str += "<html>";
            str += "<head>";
            str += "</head>";
            str += "<body>";
            for (int i = 0; i < stdlist.Count; i++)
            {
                
                if (i != 0)
                {
                    if (i % 3 == 0)
                    {
                        str += "<div class='card' style='margin-top:450px;'><button class='buttonH'><b>"+ ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).Name + "</b></button><button class='buttonG'>"+ ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchAddress+ "</button> <div class=row> <div class='col-md-8' style='margin: 10 0 5 0;'>  <span><b><i>IDENTITY CARD</i></b></span></div>  </div> <div class=row> <div class='col-md-8' style='margin: 15 0 5 0;'>  <span><b>" + stdlist[i].SR_StudentName + "</b></span></div>  </div> <div class=row>  <div class='col-md-6'>  <span><b>  Class: </b></span><span>" + stdlist[i].ClassName + "</span></br>  </div> <div class='col-md-6'>    <span><b>Roll No: </b></span><span>" + stdlist[i].SR_CurrentRollNo + "</span> </div>  </div>  <div class=row>  <div class='col-md-8'>  <span><b> Contact No: </b></span><span>" + stdlist[i].SR_PhNo + "</span></div>  </div>   <div class=row>  <div class='col-md-8'>  <span><b> Address: </b></span><span>" + stdlist[i].SR_Addrees + "</span></div>  </div>  <p><button class='buttonI'>PRINCIPAL</button></p></div>";
                        //str += "<div style='margin-top:150px;'></div>";
                    }
                    else { str += "<div class='card'><button class='buttonH'><b>"+ ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).Name + "</b></button><button class='buttonG'>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchAddress + "</button> <div class=row> <div class='col-md-8' style='margin: 10 0 5 0;'>  <span><b><i> IDENTITY CARD</i></b></span></div>  </div> <div class=row> <div class='col-md-8' style='margin: 15 0 5 0;'>  <span><b>" + stdlist[i].SR_StudentName + "</b></span></div>  </div> <div class=row>  <div class='col-md-6'>  <span><b>  Class: </b></span><span>" + stdlist[i].ClassName + "</span></br>  </div> <div class='col-md-6'>    <span><b>Roll No: </b></span><span>" + stdlist[i].SR_CurrentRollNo + "</span> </div>  </div>  <div class=row>  <div class='col-md-8'>  <span><b> Contact No: </b></span><span>" + stdlist[i].SR_PhNo + "</span></div>  </div>   <div class=row>  <div class='col-md-8'>  <span><b> Address: </b></span><span>" + stdlist[i].SR_Addrees + "</span></div>  </div>  <p><button class='buttonI'>PRINCIPAL</button></p></div>"; }
                }
                else { str += "<div class='card'><button class='buttonH'><b>"+ ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).Name + "</b></button><button class='buttonG'>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchAddress + "</button> <div class=row> <div class='col-md-8' style='margin: 10 0 5 0;'>  <span><b><i> IDENTITY CARD</i></b></span></div>  </div> <div class=row> <div class='col-md-8' style='margin: 15 0 5 0;'>  <span><b>" + stdlist[i].SR_StudentName + "</b></span></div>  </div> <div class=row>  <div class='col-md-6'>  <span><b>  Class: </b></span><span>" + stdlist[i].ClassName + "</span></br>  </div> <div class='col-md-6'>    <span><b>Roll No: </b></span><span>" + stdlist[i].SR_CurrentRollNo + "</span> </div>  </div>  <div class=row>  <div class='col-md-8'>  <span><b> Contact No: </b></span><span>" + stdlist[i].SR_PhNo + "</span></div>  </div>   <div class=row>  <div class='col-md-8'>  <span><b> Address: </b></span><span>" + stdlist[i].SR_Addrees + "</span></div>  </div>  <p><button class='buttonI'>PRINCIPAL</button></p></div>"; }
            }
            str += "</body>";
            str += "</html>";
            return str;



        }
        #endregion
    }
}