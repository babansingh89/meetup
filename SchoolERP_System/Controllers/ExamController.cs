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
    public class ExamController : Controller
    {
        #region ExamMaster
        public ActionResult ExamMaster()
        {
            return View();
        }

        public ActionResult saveExamMaster(string Id, string ExamName)
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
                    new SqlParameter("ExamID", Id),
                    new SqlParameter("ExamName", ExamName)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Exam", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewExamMaster(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ExamID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Exam", prm1, CommandType.StoredProcedure);
                List<Exam> list = Utility.ConvertDataTableToClassObjectList<Exam>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteExamMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ExamID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Exam", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ExamClassMapping
        public ActionResult ExamClassMapping()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ExamList = Utility.GetDropDownList("SP_Exam", "ExamID", "ExamName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult saveClassExamMapping(string Id, string ExamID, string ClassID)
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
                    new SqlParameter("ExamID", ExamID),
                    new SqlParameter("ClassID", ClassID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ClassExamMapping", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewClassExamMapping(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ExamID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ClassExamMapping", prm1, CommandType.StoredProcedure);
                List<Exam> list = Utility.ConvertDataTableToClassObjectList<Exam>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteClassExamMapping(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ExamID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ClassExamMapping", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region SubjectMaster
        public ActionResult SubjectMaster()
        {
            return View();
        }

        public ActionResult saveSubjectMaster(string SubjectID, string SubjectName, string SubjectCode)
        {
            try
            {
                string Type = "";
                if (SubjectID == "" || SubjectID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SubjectID", SubjectID),
                    new SqlParameter("SubjectName", SubjectName),
                    new SqlParameter("SubjectCode", SubjectCode)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Subject", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewSubjectMaster(string SubjectID, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("SubjectID", SubjectID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_Subject", prm1, CommandType.StoredProcedure);
                List<SubjectMstModels> List = Utility.ConvertDataTableToClassObjectList<SubjectMstModels>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteSubjectMaster(string SubjectID)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("SubjectID", SubjectID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_Subject", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region ClassWiseSubject
        public ActionResult ClassWiseSubject()
        {
            SqlParameter[] prm1 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ClassList = Utility.GetDropDownList("SP_Class", "ClassID", "ClassName", prm1, "", "", "Select");
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.SubjectList = Utility.GetDropDownList("SP_Subject", "SubjectID", "SubjectName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult saveClassSectionMapping()
        {
            try
            {
                string Id = Request.Form.Get("Id");
                string SubjectID = Request.Form.Get("SubjectID");
                string ClassID = Request.Form.Get("ClassID");
                var DocPath = System.Web.HttpContext.Current.Request.Files["DocPath"];
                string imgpath = null;
                if (DocPath != null)
                {
                    var fileName = Path.GetFileName(DocPath.FileName);
                    var extention = Path.GetExtension(DocPath.FileName);
                    var filenamewithoutextension = Path.GetFileNameWithoutExtension(DocPath.FileName);
                    string FileName = DateTime.Now.GetHashCode().ToString("x") + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + System.IO.Path.GetExtension(fileName);
                    imgpath = Path.Combine(Server.MapPath("/SyllabusDoc/" + FileName));
                    DocPath.SaveAs(imgpath);
                    imgpath = FileName;
                }
                string Type = "";
                if (Id == "" || Id == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ClsSubID", Id),
                    new SqlParameter("SubjectID", SubjectID),
                    new SqlParameter("DocPath", imgpath),
                    new SqlParameter("ClassID", ClassID)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ClassSubjectMapWithSyllabus", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult viewClassSubjectMapping(string Id, string Type)
        {
            try
            {
                if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Student")
                {
                    Type = "SelectByStudent";
                    Id = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID;
                }
                else if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Parents")
                {
                    Id = ((loggedInParents)System.Web.HttpContext.Current.Session["loggedInParents"]).UserID;
                    Type = "SelectByStudent";
                }
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ClsSubID",Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ClassSubjectMapWithSyllabus", prm1, CommandType.StoredProcedure);
                List<CSubj> list = Utility.ConvertDataTableToClassObjectList<CSubj>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deleteClassSubjectMapping(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ClsSubID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ClassSubjectMapWithSyllabus", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region MarksEntry
        public ActionResult MarksEntry()
        {
            return View();
        }

        public ActionResult StudentMarks()
        {
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ExamList = Utility.GetDropDownList("SP_Exam", "ExamID", "ExamName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult BindStream(string Id, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", Type),
                    new SqlParameter("ID", Id),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_StudentMarks", prm1, CommandType.StoredProcedure);
                List<CSubj> List = Utility.ConvertDataTableToClassObjectList<CSubj>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult BindStudent(string EId, string CId, string SId, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", Type),
                    new SqlParameter("EId", EId),
                    new SqlParameter("CId", CId),
                    new SqlParameter("SId", SId),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_StudentMarks", prm1, CommandType.StoredProcedure);
                List<Studentmarks> List = Utility.ConvertDataTableToClassObjectList<Studentmarks>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewMarksEntry(string Id, string Type)
        {
            try
            {
                DataTable dt = null;
                if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Student")
                {
                    SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", "StudentView"),
                    new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID)
                };
                    dt = new SQLHelper().ExecuteDataTable("SP_StudentMarksStudent", prm1, CommandType.StoredProcedure);
                }
                else if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Parents")
                {
                    SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", "StudentView"),
                    new SqlParameter("UserID", ((loggedInParents)System.Web.HttpContext.Current.Session["loggedInParents"]).UserID)
                };
                    dt = new SQLHelper().ExecuteDataTable("SP_StudentMarksStudent", prm1, CommandType.StoredProcedure);

                }
                else
                {
                    SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", Type)
                };
                    dt = new SQLHelper().ExecuteDataTable("SP_StudentMarks", prm1, CommandType.StoredProcedure);
                }
                List<ViewStudentmarks> List = Utility.ConvertDataTableToClassObjectList<ViewStudentmarks>(dt);
                return Json(List, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public DataTable GetdtFeeTable(List<Studentmarks> stddt)
        {
            GenLib objLib = new GenLib();
            DataTable dt = new DataTable();
            dt = objLib.ToDataTable(stddt);
            dt.Columns.Remove("SR_RegNo");
            dt.Columns.Remove("SR_StudentName");
            dt.Columns.Remove("SR_CurrentRollNo");
            dt.Columns.Remove("SectionName");
            return dt;
        }
        public ActionResult saveStdentMarks(List<Studentmarks> StudentID, string EId, string CId, string SId, string max, string min)
        {
            try
            {
                DataTable dt = GetdtFeeTable(StudentID);

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("type", "Stmarks"),
                    new SqlParameter("StMarks", dt),
                    new SqlParameter("EId", EId),
                    new SqlParameter("CId", CId),
                    new SqlParameter("SId", SId),
                    new SqlParameter("max", max),
                    new SqlParameter("min", min)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_StudentMarks", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewMarksEntryResult(string StdId)
        {

            try
            {
                DataSet dt = null;
                string[] parts = StdId.Split('_');
                if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Student")
                {
                    SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "PrintResult"),
                    new SqlParameter("EId", parts[1]),
                    new SqlParameter("CId", parts[0]),
                    new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                };
                    dt = new SQLHelper().ExecuteDataSet("SP_StudentMarksStudent", prm1, CommandType.StoredProcedure);
                }
                else if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Parents")
                {
                    SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "PrintResult"),
                    new SqlParameter("EId", parts[1]),
                    new SqlParameter("CId", parts[0]),
                    new SqlParameter("UserID",((loggedInParents)System.Web.HttpContext.Current.Session["loggedInParents"]).UserID),
                };
                    dt = new SQLHelper().ExecuteDataSet("SP_StudentMarksStudent", prm1, CommandType.StoredProcedure);
                }
                else if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "SuperAdmin")
                {
                    SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "PrintResult"),
                    new SqlParameter("EId", parts[1]),
                    new SqlParameter("CId", parts[0])
                    //new SqlParameter("UserID", ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID),
                };
                    dt = new SQLHelper().ExecuteDataSet("SP_StudentMarks", prm1, CommandType.StoredProcedure);
                }

                List<ExamMaster> EM = Utility.ConvertDataTableToClassObjectList<ExamMaster>(dt.Tables[0]);
                List<ExamDetails> ED = Utility.ConvertDataTableToClassObjectList<ExamDetails>(dt.Tables[1]);
                if (EM.Count == 0 && ED.Count == 0)
                    return Json("Error", JsonRequestBehavior.AllowGet);
                string OuterHTML = generateResulthtml(EM, ED);
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
        private byte[] GetBytesFromImage(String imageFile)
        {
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromFile(imageFile);
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        public string generateResulthtml(List<ExamMaster> EM, List<ExamDetails> ED)
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
            int cnt = 1;
            foreach (var obj in EM)
            {
                List<ExamDetails> EDobj = ED.Where(x => x.StudentID == obj.StudentID).ToList();

                str += "<div style = 'margin: 500px 50px 15px 50px; padding: 10px 10px 10px 10px; border: 1px solid black;'>";

                str += "   <div class='row'>";
                str += "   <div class='col-md-2'>";
                if (path != "")
                    str += "       <img src = '" + path + "' style = 'width:150px;height:108px;margin-bottom:15px;' /> ";
                str += "    </div>";
                str += "    <div class='col-md-10'>";
                str += "     <div class='divHeader'>";
                str += "       <p class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).Name + "</b></p>";
                str += "      <p class='text-center'><b>" + ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).SchAddress + "</b></p>";
                str += "    </div>";
                str += "  </div>";
                str += " </div>";


                str += "<div class='row'><div class='col-md-2'></div><div class='col-md-10'><p class='text-center'><b><u>Exam Report</u></b></p><div class='col-md-8'>";
                str += "<p><span><b> Reg No: </b></span><span> " + obj.SR_RegNo + "</span ></p><p><span><b>Class: </b></span><span>" + obj.ClassName + "</span></p><p><span><b>Exam: </b></span><span>" + obj.ExamName + "</span></p>";
                str += "</div><div class='col-md-4'>";
                str += "<p><span><b>Name: </b></span><span>" + obj.SR_StudentName + "</span></p><p><span><b>Roll No.: </b></span><span> " + obj.SR_CurrentRollNo + " </span></p><p><span><b>Section: </b></span><span>" + obj.SectionName + "</span></p>";
                str += "</div></div></div>";
                int i = 1;
                decimal TM = 0, MM = 0;
                //str += "<div class='row'><div class='col-md-2'></div><div class='col-md-10'><table class='blueTable'><tr><th>#</th><th>Subject</th><th>Marks</th></tr>";
                str += "<div class='row'  style='margin-top:10px;'><div class='col-md-12'><table class='blueTable'><tr><th>#</th><th>Subject</th><th>Marks</th></tr>";
                foreach (var objED in EDobj)
                {
                    str += "<tr><td>" + i + "</td><td>" + objED.SubjectName + "</td><td>" + objED.Theory + "</td></tr>";
                    TM += Convert.ToDecimal(objED.Theory);
                    MM += Convert.ToDecimal(objED.MaxMarks);
                    i++;
                }
                str += "<tr><td colspan='2' align='right' style='font-weight:bold;padding: 7px 10px 7px 0px;'>Total Marks:</<td><td>" + TM + " Out of " + MM + "</<td></tr>";
                str += "<tr><td colspan='2' align='right' style='font-weight:bold;padding: 7px 10px 7px 0px;'>Percentage:</<td><td>" + Math.Round(((TM * 100) / MM), 2) + " %</<td></tr>";
                str += "<tr><td colspan='2' align='right' style='font-weight:bold;padding: 7px 10px 7px 0px;'>Remarks:</<td><td></<td></tr>";
                str += "</table></div></div>";
                str += "<div class='row' style='margin-top:60px;'><div class='col-md-12'><div style = 'border-top: 1px dashed black; margin-left: 80%; margin-right: 1%;' > </div></div><p style = 'margin-left: 80%;'><b> Administrative Officer </b></p></div></div>";
                cnt++;
            }
            str += "</body>";
            str += "</html>";

            return str;
        }
        #endregion

        #region ExamReports
        public ActionResult ExamReports()
        {
            return View();
        }
        #endregion

        #region Exam Timetable
        public ActionResult ExamTimetable()
        {
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "Select"),
            };
            ViewBag.ExamList = Utility.GetDropDownList("SP_Exam", "ExamID", "ExamName", prm2, "", "", "Select");
            return View();
        }

        public ActionResult saveExamTimetableMaster(string ETID, string ExamID, string SubjectID, string ClassID, string ExamDate,
                     string StHr, string StMn, string EnHr, string EnMn)
        {
            try
            {
                string Type = "";
                if (ETID == "" || ETID == "0")
                    Type = "Insert";
                else
                    Type = "Update";
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ETID", ETID),
                    new SqlParameter("ExamID", ExamID),
                    new SqlParameter("SubjectID", SubjectID),
                    new SqlParameter("ClassID", ClassID),
                    new SqlParameter("ExamDate", ExamDate),
                    new SqlParameter("StHr", StHr),
                    new SqlParameter("StMn", StMn),
                    new SqlParameter("EnHr", EnHr),
                    new SqlParameter("EnMn", EnMn),
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ExamTimetable", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult viewExamTimetableMaster(string ETID, string ExamID, string ClassID, string SubjectID, string Type)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ETID",ETID),
                    new SqlParameter("ExamID",ExamID),
                    new SqlParameter("ClassID",ClassID),
                    new SqlParameter("SubjectID",SubjectID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ExamTimetable", prm1, CommandType.StoredProcedure);
                List<ExamTimetable> list = Utility.ConvertDataTableToClassObjectList<ExamTimetable>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteExamTimetableMaster(string Id)
        {
            try
            {
                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", "Delete"),
                    new SqlParameter("ETID", Id)
                };
                string Output = Convert.ToString(new SQLHelper().ExecuteScalar("SP_ExamTimetable", prm1, CommandType.StoredProcedure));
                return Json(Output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ExamTimeSchedule()
        {
            string UserID = "";
            if (((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).userType == "Parents")
            {
                UserID = ((loggedInParents)System.Web.HttpContext.Current.Session["loggedInParents"]).UserID;
            }
            else
            {
                UserID = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID;
            }
            SqlParameter[] prm2 = new SqlParameter[] {
                  new SqlParameter("@Type", "SelectStudent"),
                  new SqlParameter("@ExamID",  UserID),
            };
            ViewBag.ExamList = Utility.GetDropDownList("SP_ClassExamMapping", "ExamID", "ExamName", prm2, "", "", "Select");
            return View();
        }
        public ActionResult viewExamTimetableStudent(string ExamID, string Type)
        {
            try
            {
                string UserID = "";

                UserID = ((loggedInAdmin)System.Web.HttpContext.Current.Session["loggedInAdmin"]).UserID;

                SqlParameter[] prm1 = new SqlParameter[] {
                    new SqlParameter("Type", Type),
                    new SqlParameter("ExamID",ExamID),
                    new SqlParameter("SubjectID",UserID),
                };
                DataTable dt = new SQLHelper().ExecuteDataTable("SP_ExamTimetable", prm1, CommandType.StoredProcedure);
                List<ExamTimetable> list = Utility.ConvertDataTableToClassObjectList<ExamTimetable>(dt);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}