using SchoolERP_System.Models;
using SchoolERP_System.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;

namespace SchoolERP_System.Controllers
{
    [IdentityBasicAuthentication]
    public class SchoolAPIController : ApiController
    {
        SQLHelperAppID SDhelp = new SQLHelperAppID();
        SQLHelperMaster Mhelp = new SQLHelperMaster();
        HelperClass help = new HelperClass();

        #region Login

        [Route("api/SchoolAPI/Login")]
        [HttpPost]
        public HttpResponseMessage Login(List<ApiParams> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else
                {

                    SqlParameter[] prmm = new SqlParameter[]
                               {
                                       new SqlParameter("UserType", requestData[0].UserType.Trim()),
                                       new SqlParameter("UserID", requestData[0].UserID.Trim()),
                                       new SqlParameter("UserPassword", requestData[0].UserPassword.Trim())
                               };

                    DataTable objresult = Mhelp.ExecuteDataTable("WebApi_SP_Login", prmm, CommandType.StoredProcedure);

                    if (objresult.Rows[0][0].ToString() == "Yes")
                    {
                        obj.success = 1;
                        if (requestData[0].UserPassword.Trim() == "123456")
                        {
                            obj.message = Convert.ToString("FirstTime");
                        }
                        else { obj.message = Convert.ToString("Success"); }

                        obj.data = objresult;
                    }
                    else
                    {
                        obj.success = 0;
                        obj.message = objresult.Rows[0][0].ToString();
                    }
                }

                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }

        #endregion

        #region Enquiry

        [Route("api/SchoolAPI/Enquiry")]
        [HttpPost]
        public HttpResponseMessage Enquiry(List<ApiEnquiry> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();

            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else
                {

                    DataTable dt = new DataTable();
                    if (requestData[0].enqData != null && requestData[0].enqData.Count >= 1)
                        dt = objLib.ToDataTable(requestData[0].enqData);



                    SqlParameter[] prmm = new SqlParameter[]
                    { new SqlParameter("Type", requestData[0].Type.Trim()),
                    new SqlParameter("EnquiryID", requestData[0].EnquiryID??null),
                    new SqlParameter("ClassID", requestData[0].ClassID ?? null),
                    new SqlParameter("E_Date", requestData[0].E_Date ?? null),
                    new SqlParameter("E_Type", requestData[0].E_Type ?? null),
                    new SqlParameter("StudentName", requestData[0].StudentName ?? null),
                    new SqlParameter("Gender", requestData[0].Gender ?? null),
                    new SqlParameter("DOB", requestData[0].DOB ?? null),
                    new SqlParameter("FatherName", requestData[0].FatherName ?? null),
                    new SqlParameter("Email", requestData[0].Email ?? null),
                    new SqlParameter("Mobile", requestData[0].Mobile ?? null),
                    new SqlParameter("NextFollowDate", requestData[0].NextFollowDate ?? null),
                    new SqlParameter("NextFollowUpNote", requestData[0].NextFollowUpNote ?? null),
                    new SqlParameter("myTableType", dt ?? null)

                               };
                    DataTable objresult = null;
                    if (requestData[0].Type.Trim() != "Insert" && requestData[0].Type.Trim() != "Update")
                    {
                        objresult = SDhelp.ExecuteDataTable(Convert.ToInt32(requestData[0].AppID.Trim()), "WebApi_SP_Enquiry", prmm, CommandType.StoredProcedure);
                    }
                    else
                    {
                        objresult = SDhelp.ExecuteDataTableWithTable(Convert.ToInt32(requestData[0].AppID.Trim()), "WebApi_SP_Enquiry", prmm, CommandType.StoredProcedure);
                    }

                    if (objresult.Rows[0][0].ToString() == "Yes" && (requestData[0].Type.Trim().Contains("Delete") || requestData[0].Type.Trim().Contains("Update") || requestData[0].Type.Trim().Contains("Insert")))
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else if (objresult.Rows.Count > 0)
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else
                    {
                        obj.success = 0;
                        obj.message = objresult.Rows[0][0].ToString();
                    }
                }
                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }

        #endregion

        #region ProPreAdmittedfile

        [Route("api/SchoolAPI/PreAdmitted")]
        [HttpPost]
        public HttpResponseMessage PreAdmitted(List<ApiPreAdmitted> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else
                {
                    string imgpath = null;
                    if (requestData[0].SR_Pic != null && requestData[0].SR_Pic.Length > 0)
                    {
                        string FileName = DateTime.Now.GetHashCode().ToString("x") + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".png";


                        string tempFileWithPath = HttpContext.Current.Server.MapPath("/UploadedImage/" + FileName);
                        if (System.IO.File.Exists(tempFileWithPath))
                        {
                            System.IO.File.Delete(tempFileWithPath);
                        }
                        if (requestData[0].SR_Pic != null && requestData[0].SR_Pic.Length > 0)
                        {
                            System.IO.File.WriteAllBytes(tempFileWithPath, requestData[0].SR_Pic);
                        }
                        imgpath = FileName;
                    }

                    SqlParameter[] prmm = new SqlParameter[]
                               {
                                       new SqlParameter("type", requestData[0].Type.Trim()),
                                       new SqlParameter("SR_ID", requestData[0].SR_ID??null),
                                       new SqlParameter("SR_StudentName", requestData[0].SR_StudentName??null),
                                       new SqlParameter("SR_Gender", requestData[0].SR_Gender??null),
                                       new SqlParameter("SR_DOB", requestData[0].SR_DOB??null),
                                       new SqlParameter("SR_PhNo", requestData[0].SR_PhNo??null),
                                       new SqlParameter("SR_ClassID", requestData[0].SR_ClassID??null),
                                       new SqlParameter("SR_SectionID", requestData[0].SR_SectionID??null),
                                       new SqlParameter("SR_CategoryID", requestData[0].SR_CategoryID??null),
                                       new SqlParameter("SR_PunchingCode", requestData[0].SR_PunchingCode??null),
                                       new SqlParameter("SR_Pic", imgpath),
                               };

                    DataTable objresult = SDhelp.ExecuteDataTable(Convert.ToInt32(requestData[0].AppID.Trim()), "WebApi_SP_PreAdmitted", prmm, CommandType.StoredProcedure);

                    if (objresult.Rows[0][0].ToString() == "Yes" && (requestData[0].Type.Trim().Contains("Delete") || requestData[0].Type.Trim().Contains("Update") || requestData[0].Type.Trim().Contains("Insert")))
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else if (objresult.Rows.Count > 0)
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else
                    {
                        obj.success = 0;
                        obj.message = objresult.Rows[0][0].ToString();
                    }
                }

                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }

        #endregion

        #region Profile

        [Route("api/SchoolAPI/Profile")]
        [HttpPost]
        public HttpResponseMessage Profile(List<ApiProfile> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else
                {
                    string imgpath = null;
                    if (requestData[0].SchLogo != null && requestData[0].SchLogo.Length > 0)
                    {
                        string FileName = DateTime.Now.GetHashCode().ToString("x") + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".png";


                        string tempFileWithPath = HttpContext.Current.Server.MapPath("/SchImage/" + FileName);
                        if (System.IO.File.Exists(tempFileWithPath))
                        {
                            System.IO.File.Delete(tempFileWithPath);
                        }
                        if (requestData[0].SchLogo != null && requestData[0].SchLogo.Length > 0)
                        {
                            System.IO.File.WriteAllBytes(tempFileWithPath, requestData[0].SchLogo);
                        }
                        imgpath = FileName;
                    }

                    SqlParameter[] prmm = new SqlParameter[]
                               {
                                       new SqlParameter("AppID", requestData[0].AppID.Trim()),
                                       new SqlParameter("SchName", requestData[0].SchName.Trim()),
                                       new SqlParameter("SchAddress", requestData[0].SchAddress.Trim()),
                                       new SqlParameter("Contact", requestData[0].Contact.Trim()),
                                       new SqlParameter("EmailID", requestData[0].EmailID.Trim()),
                                       new SqlParameter("UserPassword", requestData[0].UserPassword.Trim()),
                                       new SqlParameter("SchoolLogo", imgpath),
                               };

                    DataTable objresult = Mhelp.ExecuteDataTable("WebApi_SP_Profile", prmm, CommandType.StoredProcedure);

                    if (objresult.Rows[0][0].ToString() == "Yes")
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else
                    {
                        obj.success = 0;
                        obj.message = objresult.Rows[0][0].ToString();
                    }
                }

                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        #endregion

        #region Common

        [Route("api/SchoolAPI/Common")]
        [HttpPost]
        public HttpResponseMessage Common(List<ApiCommon> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else
                {

                    SqlParameter[] prmm = new SqlParameter[]
                               {
                                       new SqlParameter("type", requestData[0].Type.Trim()),
                                       new SqlParameter("ID", requestData[0].ID??null)
                               };

                    DataTable objresult = SDhelp.ExecuteDataTable(Convert.ToInt32(requestData[0].AppID.Trim()), "WebApi_SP_Common", prmm, CommandType.StoredProcedure);

                    if (objresult.Rows.Count > 0 && objresult.Rows[0][0].ToString() == "Yes" && (requestData[0].Type.Trim().Contains("Delete") || requestData[0].Type.Trim().Contains("Update") || requestData[0].Type.Trim().Contains("Insert")))
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else if (objresult.Rows.Count > 0)
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else
                    {
                        obj.success = 0;
                        obj.message = Convert.ToString("Unable to reach");
                    }
                }

                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }

        #endregion

        #region Support

        [Route("api/SchoolAPI/Support")]
        [HttpPost]
        public HttpResponseMessage Support(List<ApiSupport> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else
                {

                    SqlParameter[] prmm = new SqlParameter[]
                               {
                                       new SqlParameter("type", requestData[0].Type.Trim()),
                                       new SqlParameter("AppID", requestData[0].AppID.Trim()),
                                       new SqlParameter("SupportID", requestData[0].SupportID??null),
                                       new SqlParameter("IssueTitle", requestData[0].IssueTitle??null),
                                       new SqlParameter("IssueDesc", requestData[0].IssueDesc??null),
                                       new SqlParameter("IssueBy", requestData[0].IssueBy??null),
                                       new SqlParameter("UserID", requestData[0].UserID??null),

                               };

                    DataTable objresult = Mhelp.ExecuteDataTable("WebApi_SP_Support", prmm, CommandType.StoredProcedure);

                    if (objresult.Rows.Count > 0 && objresult.Rows[0][0].ToString() == "Yes" && (requestData[0].Type.Trim().Contains("Delete") || requestData[0].Type.Trim().Contains("Update") || requestData[0].Type.Trim().Contains("Insert")))
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else if (objresult.Rows.Count > 0)
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else
                    {
                        obj.success = 0;
                        obj.message = Convert.ToString("No Record Found!");
                        obj.data = objresult;
                    }
                }

                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }

        #endregion

        #region ResetPassword

        [Route("api/SchoolAPI/ResetPassword")]
        [HttpPost]
        public HttpResponseMessage ResetPassword(List<ApiParams> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else if (requestData[0].UserPassword.Trim() == "123456")
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper password to process");
                }
                else
                {

                    SqlParameter[] prmm = new SqlParameter[]
                               {
                                       new SqlParameter("AppID", requestData[0].AppID.Trim()),
                                       new SqlParameter("UserType", requestData[0].UserType.Trim()),
                                       new SqlParameter("UserID", requestData[0].UserID.Trim()),
                                       new SqlParameter("UserPassword", requestData[0].UserPassword.Trim())
                               };

                    DataTable objresult = Mhelp.ExecuteDataTable("SP_ResetLogin", prmm, CommandType.StoredProcedure);

                    if (objresult.Rows[0][0].ToString() == "resetSuccessful")
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else
                    {
                        obj.success = 0;
                        obj.message = objresult.Rows[0][0].ToString();
                    }
                }

                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }

        #endregion

        #region Get Profile

        [Route("api/SchoolAPI/GetProfile")]
        [HttpPost]
        public HttpResponseMessage GetProfile(List<ApiParams> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else
                {

                    SqlParameter[] prmm = new SqlParameter[]
                               {
                                       new SqlParameter("Type", "Profile"),
                                       new SqlParameter("AppID", requestData[0].AppID.Trim()),
                                       new SqlParameter("UserType", requestData[0].UserType.Trim()),
                                       new SqlParameter("UserID", requestData[0].UserID.Trim())
                               };

                    DataTable objresult = SDhelp.ExecuteDataTable(Convert.ToInt32(requestData[0].AppID.Trim()), "WebApi_SP_GetProfile", prmm, CommandType.StoredProcedure);

                    if (objresult.Rows.Count > 0 && objresult.Rows[0][0].ToString() == "Yes" && (requestData[0].Type.Trim().Contains("Delete") || requestData[0].Type.Trim().Contains("Update") || requestData[0].Type.Trim().Contains("Insert")))
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else if (objresult.Rows.Count > 0)
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else
                    {
                        obj.success = 0;
                        obj.message = Convert.ToString("Unable to reach");
                    }
                }

                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }


        [Route("api/SchoolAPI/UpdateProfile")]
        [HttpPost]
        public HttpResponseMessage UpdateProfile(List<UpdateProfileParams> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else
                {
                    string imgpath = null;
                    DataTable objresult = null;
                    string tempFileWithPath = "";

                    #region IMage
                    if (requestData[0].ProfileLogo != null && requestData[0].ProfileLogo.Length > 0)
                    {
                        string FileName = DateTime.Now.GetHashCode().ToString("x") + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".png";

                        if (requestData[0].UserType.Trim() == "Teacher" || requestData[0].UserType.Trim() == "Accountant" || requestData[0].UserType.Trim() == "Librarian")
                        {
                            tempFileWithPath = HttpContext.Current.Server.MapPath("/EmpImage/" + FileName);
                        }
                        else if (requestData[0].UserType.Trim() == "Student")
                        {
                            tempFileWithPath = HttpContext.Current.Server.MapPath("/UploadedImage/" + FileName);
                        }
                        else if (requestData[0].UserType.Trim() == "Parents")
                        {
                            tempFileWithPath = HttpContext.Current.Server.MapPath("/ParentsImage/" + FileName);
                        }
                        if (System.IO.File.Exists(tempFileWithPath))
                        {
                            System.IO.File.Delete(tempFileWithPath);
                        }
                        if (requestData[0].ProfileLogo != null && requestData[0].ProfileLogo.Length > 0)
                        {
                            System.IO.File.WriteAllBytes(tempFileWithPath, requestData[0].ProfileLogo);
                        }
                        imgpath = FileName;
                    }
                    #endregion


                    #region EMP
                    if (requestData[0].UserType.Trim() == "Teacher" || requestData[0].UserType.Trim() == "Accountant" || requestData[0].UserType.Trim() == "Librarian")
                    {
                        SqlParameter[] prm1 = new SqlParameter[] {
                new SqlParameter("Type", "UpdateEmp"),
                new SqlParameter("AppID", requestData[0].AppID.Trim()),
                new SqlParameter("UserType", requestData[0].UserType.Trim()),
                new SqlParameter("UserID", requestData[0].UserID.Trim()),
                new SqlParameter("EM_EmpId",requestData[0].EM_EmpId.Trim()),
                new SqlParameter("EM_EmpName",requestData[0].EM_EmpName.Trim()),
                new SqlParameter("EM_DOB",requestData[0].EM_DOB.Trim()),
                new SqlParameter("EM_Gender",requestData[0].EM_Gender.Trim()),
                new SqlParameter("EM_EmpFathers",requestData[0].EM_EmpFathers.Trim()),
                new SqlParameter("EM_EmpAddress",requestData[0].EM_EmpAddress.Trim()),
                new SqlParameter("EM_Email",requestData[0].EM_Email.Trim()),
                new SqlParameter("EM_PanNo",requestData[0].EM_PanNo.Trim()),
                new SqlParameter("EM_AdharNo",requestData[0].EM_AdharNo.Trim()),
                new SqlParameter("Password",requestData[0].EM_Password.Trim()),
                new SqlParameter("EM_ProfilePic", imgpath)
                      };
                        objresult = SDhelp.ExecuteDataTable(Convert.ToInt32(requestData[0].AppID.Trim()), "WebApi_SP_GetProfile", prm1, CommandType.StoredProcedure);
                    }
                    #endregion
                    #region Student
                    if (requestData[0].UserType.Trim() == "Student")
                    {
                        SqlParameter[] prm1 = new SqlParameter[] {
                     new SqlParameter("Type", "UpdateStudent"),
                     new SqlParameter("SR_ID", requestData[0].SR_ID),
                     new SqlParameter("AppID", requestData[0].AppID),
                     new SqlParameter("SR_StudentName", requestData[0].SR_StudentName),
                     new SqlParameter("SR_Email", requestData[0].SR_Email),
                     new SqlParameter("SR_AlternetContactNo", requestData[0].SR_AlternetContactNo),
                     new SqlParameter("SR_DOB", requestData[0].SR_DOB),
                     new SqlParameter("SR_Gender", requestData[0].SR_Gender),
                     new SqlParameter("SR_Addrees", requestData[0].SR_Addrees),
                     new SqlParameter("UserPassword", requestData[0].UserPassword),
                     new SqlParameter("UserType", requestData[0].UserType),
                     new SqlParameter("UserID",  requestData[0].UserID),
                     new SqlParameter("SR_Pic", imgpath)
                };
                        objresult = SDhelp.ExecuteDataTable(Convert.ToInt32(requestData[0].AppID.Trim()), "WebApi_SP_GetProfile", prm1, CommandType.StoredProcedure);
                    }
                    #endregion
                    #region Parents
                    if (requestData[0].UserType.Trim() == "Parents")
                    {
                        SqlParameter[] prm1 = new SqlParameter[] {
                new SqlParameter("Type", "UpdateParents"),
                new SqlParameter("AppID", requestData[0].AppID),
                new SqlParameter("UserType", requestData[0].UserType),
                new SqlParameter("P_EmpContactNo", requestData[0].UserID),
                new SqlParameter("P_EmpId",requestData[0].P_EmpId),
                new SqlParameter("P_EmpName",requestData[0].P_EmpName),
                new SqlParameter("P_DOB",requestData[0].P_DOB),
                new SqlParameter("P_Gender",requestData[0].P_Gender),
                new SqlParameter("P_EmpFathers",requestData[0].P_EmpFathers),
                new SqlParameter("P_EmpAddress",requestData[0].P_EmpAddress),
                new SqlParameter("P_Email",requestData[0].P_Email),
                new SqlParameter("P_PanNo",requestData[0].P_PanNo),
                new SqlParameter("P_AdharNo",requestData[0].P_AdharNo),
                new SqlParameter("UserPassword",requestData[0].P_Password),
                new SqlParameter("P_ProfilePic", imgpath)
                      };
                        objresult = SDhelp.ExecuteDataTable(Convert.ToInt32(requestData[0].AppID.Trim()), "WebApi_SP_GetProfile", prm1, CommandType.StoredProcedure);
                    }
                    #endregion


                    if (objresult.Rows.Count > 0 && objresult.Rows[0][0].ToString() == "Yes")
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else if (objresult.Rows.Count > 0)
                    {
                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        obj.data = objresult;
                    }
                    else
                    {
                        obj.success = 0;
                        obj.message = Convert.ToString("Unable to reach");
                    }
                }

                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }
        #endregion

        #region Dashboard
        [Route("api/SchoolAPI/Dashboard")]
        [HttpPost]
        public HttpResponseMessage Dashboard(List<ApiAttendacne> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else
                {
                    SqlParameter[] prmm = new SqlParameter[]
                                   {
                                       new SqlParameter("Type", requestData[0].Type.Trim()),
                                       new SqlParameter("ID", requestData[0].UserID??null)
                                   };

                    DataSet objresult = SDhelp.ExecuteDataSet(Convert.ToInt32(requestData[0].AppID.Trim()), "SP_Dashboard", prmm, CommandType.StoredProcedure);


                    if (objresult.Tables[0].Rows.Count > 0)
                    {

                        obj.success = 1;
                        obj.message = Convert.ToString("Success");
                        if (requestData[0].Type.Trim() == "Dashboard")
                        {
                            obj.data = objresult.Tables[0];
                        }
                        else { obj.dataSet = objresult; }

                    }
                    else
                    {
                        obj.success = 0;
                        obj.message = Convert.ToString("Unable to process, try agian!");
                    }

                }

                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }
        #endregion

        #region ViewByTeacher
        [Route("api/SchoolAPI/ViewByTeacher")]
        [HttpPost]
        public HttpResponseMessage ViewByTeacher(List<ApiAttendacne> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else
                {
                    string SP = "";
                    if (requestData[0].Type.Trim() == "SelectHoliday")
                    {
                        requestData[0].Type = "Select";
                        SP = "SP_Holiday";
                    }
                    else if (requestData[0].Type.Trim() == "SelectSyllabus")
                    {
                        SP = "SP_ClassSubjectMapWithSyllabus";
                    }
                    else if (requestData[0].Type.Trim() == "SelectStudent")
                    {
                        requestData[0].Type = "SelectAdmit";
                        SP = "SP_StudentRegistration";
                    }
                    else if (requestData[0].Type.Trim() == "SelectStaff")
                    {
                        requestData[0].Type = "Select";
                        SP = "SP_Employee";
                    }
                    else if (requestData[0].Type.Trim() == "ViewRoutine")
                    {
                        requestData[0].Type = "SelectByTeacher";
                        SP = "SP_RoutineView";
                    }
                    else if (requestData[0].Type.Trim() == "GetExam")
                    {
                        requestData[0].Type = "Select";
                        SP = "SP_Exam";
                    }
                    else if (requestData[0].Type.Trim() == "GetClassByExam")
                    {
                        requestData[0].Type = "BindClass";
                        SP = "SP_ExamTimetable";
                    }
                    else if (requestData[0].Type.Trim() == "GetExamSchedule")
                    {
                        requestData[0].Type = "Select";
                        SP = "SP_ExamTimetable";
                    }
                    else if (requestData[0].Type.Trim() == "ViewResult")
                    {
                        requestData[0].Type = "SelectMarkswithsbject";
                        SP = "SP_StudentMarks";
                    }
                    else if (requestData[0].Type.Trim() == "PrintResult")
                    {
                        SP = "SP_StudentMarksTeacher";
                    }
                    SqlParameter[] prmm = new SqlParameter[]
                                   {
                                       new SqlParameter("Type", requestData[0].Type.Trim()),
                                       new SqlParameter("UserID", requestData[0].UserID??null),
                                       new SqlParameter("UserType", requestData[0].UserType??null),
                                       new SqlParameter("ExamID", requestData[0].ExamID??null),
                                       new SqlParameter("ClassID", requestData[0].ClassID??null)
                                   };

                    if (requestData[0].Type.Trim() == "PrintResult")
                    {
                        DataSet objresult = SDhelp.ExecuteDataSet(Convert.ToInt32(requestData[0].AppID.Trim()), SP, prmm, CommandType.StoredProcedure);


                        if (objresult.Tables[0].Rows.Count > 0)
                        {

                            obj.success = 1;
                            obj.message = Convert.ToString("Success");
                            obj.dataSet = objresult;
                        }
                        else
                        {
                            obj.success = 0;
                            obj.message = Convert.ToString("Unable to process, try agian!");
                        }
                    }
                    else
                    {
                        DataTable objresult = SDhelp.ExecuteDataTable(Convert.ToInt32(requestData[0].AppID.Trim()), SP, prmm, CommandType.StoredProcedure);


                        if (objresult.Rows.Count > 0)
                        {

                            obj.success = 1;
                            obj.message = Convert.ToString("Success");
                            obj.data = objresult;
                        }
                        else
                        {
                            obj.success = 0;
                            obj.message = Convert.ToString("Unable to process, try agian!");
                        }
                    }

                }

                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }
        #endregion

        #region ViewByStudent
        [Route("api/SchoolAPI/ViewByStudent")]
        [HttpPost]
        public HttpResponseMessage ViewByStudent(List<ApiAttendacne> requestData)
        {
            ResponseObj obj = new ResponseObj();
            GenLib objLib = new GenLib();
            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                if (requestData.Count == 0)
                {
                    obj.success = 0;
                    obj.message = Convert.ToString("Pass proper data to process");
                }
                else
                {
                    string SP = "";
                    if (requestData[0].Type.Trim() == "SelectHoliday")
                    {
                        requestData[0].Type = "Select";
                        SP = "SP_Holiday";
                    }
                    else if (requestData[0].Type.Trim() == "SelectSyllabus")
                    {
                        requestData[0].Type = "SelectByStudentApp";
                        SP = "SP_ClassSubjectMapWithSyllabus";
                    }

                    else if (requestData[0].Type.Trim() == "ViewRoutine")
                    {
                        requestData[0].Type = "SelectByStudent";
                        SP = "SP_RoutineView";
                    }
                    else if (requestData[0].Type.Trim() == "GetExam")
                    {
                        requestData[0].Type = "SelectStudentApp";
                        SP = "SP_ClassExamMapping";
                    }

                    else if (requestData[0].Type.Trim() == "GetExamSchedule")
                    {
                        requestData[0].Type = "SelectStudentApp";
                        SP = "SP_ExamTimetable";
                    }
                    else if (requestData[0].Type.Trim() == "ViewResult")
                    {
                        requestData[0].Type = "StudentView";
                        SP = "SP_StudentMarksStudent";
                    }
                    else if (requestData[0].Type.Trim() == "PrintResult")
                    {
                        requestData[0].Type = "PrintResultStudent";
                        SP = "SP_StudentMarksTeacher";
                    }
                    else if (requestData[0].Type.Trim() == "BookManger")
                    {
                        requestData[0].Type = "Select";
                        SP = "SP_Book";
                    }
                    else if (requestData[0].Type.Trim() == "BookRetrun")
                    {
                        requestData[0].Type = "SelectStudentApp";
                        SP = "SP_IssueBook";
                    }
                    
                    SqlParameter[] prmm = new SqlParameter[]
                                   {
                                       new SqlParameter("Type", requestData[0].Type.Trim()),
                                       new SqlParameter("UserID", requestData[0].UserID??null),
                                       new SqlParameter("UserType", requestData[0].UserType??null),
                                       new SqlParameter("ExamID", requestData[0].ExamID??null),
                                       new SqlParameter("ClassID", requestData[0].ClassID??null)
                                   };
                    if (requestData[0].Type.Trim() == "PrintResultStudent")
                    {
                        DataSet objresult = SDhelp.ExecuteDataSet(Convert.ToInt32(requestData[0].AppID.Trim()), SP, prmm, CommandType.StoredProcedure);


                        if (objresult.Tables[0].Rows.Count > 0)
                        {

                            obj.success = 1;
                            obj.message = Convert.ToString("Success");
                            obj.dataSet = objresult;
                        }
                        else
                        {
                            obj.success = 0;
                            obj.message = Convert.ToString("Unable to process, try agian!");
                        }
                    }
                    else
                    {
                        DataTable objresult = SDhelp.ExecuteDataTable(Convert.ToInt32(requestData[0].AppID.Trim()), SP, prmm, CommandType.StoredProcedure);


                        if (objresult.Rows.Count > 0)
                        {

                            obj.success = 1;
                            obj.message = Convert.ToString("Success");
                            obj.data = objresult;
                        }
                        else
                        {
                            obj.success = 0;
                            obj.message = Convert.ToString("Unable to process, try agian!");
                        }
                    }

                }

                string jsonString = objLib.ObjToJsonWithJsonNet(obj);
                StringContent sc = new StringContent(jsonString);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
            }
            catch (Exception ex)
            {

            }
            return resp;
        }
        #endregion
    }
}