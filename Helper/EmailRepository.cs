using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using IOCLWeb.Models;

namespace IOCLWeb.Helper
{
    public class EmailRepository
    {
        string MailOnOff = Convert.ToString(ConfigurationManager.AppSettings["IOCLMailOnOff"]);
        string IOCLsupportMail = Convert.ToString(ConfigurationManager.AppSettings["IOCLsupportMail"]);

        public void IOCLMailContent(string Mail_Name, string MailToUserID = "", string MailFrom = "")
        {
            try
            {
                #region Inspection_Note_Approval
                if (Mail_Name == "Inspection_Note_Approval")
                {
                    SqlParameter[] prm1 = new SqlParameter[]{
                        new SqlParameter("@Type", "Inspection_Note_Approval"),
                        new SqlParameter("@UserID", MailToUserID)
                    };
                    DataSet ds_Inspection_Note = new SQLHelper().ExecuteDataSet("SP_IOCL_Mail_Details", prm1, CommandType.StoredProcedure);
                    DataTable dtt_User = ds_Inspection_Note.Tables[0];
                    DataTable dtt_MailContent = ds_Inspection_Note.Tables[1];

                    string Mail_From_EmpName = Convert.ToString(((SessionModel)HttpContext.Current.Session["LoggedinUser"]).UserName);
                    string Mail_To_EmpEmail = Convert.ToString(dtt_User.Rows[0]["Mail_To_EmpEmail"]);
                    string Mail_Subject = Convert.ToString(dtt_MailContent.Rows[0]["Mail_Subject"]);
                    string Mail_Body = Convert.ToString(dtt_MailContent.Rows[0]["Mail_BodyHtml"]);

                    MailModels cnm = new MailModels();
                    cnm.Mail_From_EmpName = Mail_From_EmpName;
                    cnm.Mail_From_EmpEmail = IOCLsupportMail;
                    cnm.Mail_To_EmpEmail = Mail_To_EmpEmail;
                    cnm.Mail_Subject = Mail_Subject;
                    cnm.Mail_BodyHtml = Mail_Body.Replace("@@Initiator##", cnm.Mail_From_EmpName);
                    SendMail(cnm);
                }
                #endregion

                #region Reserved_Price_Approval
                if (Mail_Name == "Reserved_Price_Approval")
                {
                    SqlParameter[] prm1 = new SqlParameter[]{
                        new SqlParameter("@Type", "Reserved_Price_Approval"),
                        new SqlParameter("@UserID", MailToUserID)
                    };
                    DataSet ds_Inspection_Note = new SQLHelper().ExecuteDataSet("SP_IOCL_Mail_Details", prm1, CommandType.StoredProcedure);
                    DataTable dtt_User = ds_Inspection_Note.Tables[0];
                    DataTable dtt_MailContent = ds_Inspection_Note.Tables[1];

                    string Mail_From_EmpName = Convert.ToString(((SessionModel)HttpContext.Current.Session["LoggedinUser"]).UserName);
                    string Mail_To_EmpEmail = Convert.ToString(dtt_User.Rows[0]["Mail_To_EmpEmail"]);
                    string Mail_Subject = Convert.ToString(dtt_MailContent.Rows[0]["Mail_Subject"]);
                    string Mail_Body = Convert.ToString(dtt_MailContent.Rows[0]["Mail_BodyHtml"]);

                    MailModels cnm = new MailModels();
                    cnm.Mail_From_EmpName = Mail_From_EmpName;
                    cnm.Mail_From_EmpEmail = IOCLsupportMail;
                    cnm.Mail_To_EmpEmail = Mail_To_EmpEmail;
                    cnm.Mail_Subject = Mail_Subject;
                    cnm.Mail_BodyHtml = Mail_Body.Replace("@@Initiator##", cnm.Mail_From_EmpName);
                    SendMail(cnm);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public void SendMail(MailModels cnm)
        {
            string MailStatus = "";
            try
            {
                MailMessage objMM = new MailMessage();
                SmtpClient smtpmail = new SmtpClient();
                MailAddress FromMailAddress = new MailAddress(cnm.Mail_From_EmpEmail);

                var mbcc = cnm.Mail_To_EmpEmail.Split(',');
                for (int j = 0; j <= mbcc.Length - 1; j++)
                {
                    if (mbcc[j] != "")
                    {
                        MailAddress toMailAddress = new MailAddress(mbcc[j]);
                        objMM.To.Add(toMailAddress);
                    }
                }
                if (!string.IsNullOrWhiteSpace(cnm.Mail_CC_EmpEmail))
                {
                    objMM.CC.Add(cnm.Mail_CC_EmpEmail);
                }
                objMM.From = FromMailAddress;
                objMM.Priority = MailPriority.Normal;
                objMM.Subject = cnm.Mail_Subject;
                objMM.IsBodyHtml = true;
                objMM.Body = cnm.Mail_BodyHtml;
                smtpmail.Host = "172.16.1.152";
                smtpmail.UseDefaultCredentials = false;
                smtpmail.Port = 25;
                if (MailOnOff == "1")
                {
                    smtpmail.Send(objMM);
                    MailStatus = "MailSuccess";
                }
                else
                {
                    MailStatus = "MailOff";
                }
            }
            catch (Exception ex)
            {
                MailStatus = ex.Message.ToString();
            }
            //SaveCreditNoteMailTrail(cnm.CNID, cnm.MailFromEmpEmail, cnm.MailToEmpEmail, cnm.MailCCEmpEmail, cnm.MailSubject, cnm.MailBody, MailStatus);
        }

        //public void SaveCreditNoteMailTrail(string CNID, string MailFrom, string MailTo, string MailCC, string MailSubject, string MailBody, string MailStatus)
        //{
        //    SqlParameter[] prm1 = new SqlParameter[] {
        //        new SqlParameter("Type", "saveMailDetails"),
        //        new SqlParameter("CNID", CNID),
        //        new SqlParameter("MailFrom", MailFrom),
        //        new SqlParameter("MailTo", MailTo),
        //        new SqlParameter("MailCC", MailCC),
        //        new SqlParameter("MailSub", MailSubject),
        //        new SqlParameter("MailBody", MailBody),
        //        new SqlParameter("MailStatus", MailStatus),
        //        new SqlParameter("MailType", "")
        //    };
        //    string Output = Convert.ToString(new SQLHelper().ExecuteScalar("", prm1, CommandType.StoredProcedure));
        //}
    }

    public class MailModels
    {
        public string Mail_From_EmpName { get; set; }
        public string Mail_From_EmpEmail { get; set; }
        public string Mail_To_EmpName { get; set; }
        public string Mail_To_EmpEmail { get; set; }
        public string Mail_CC_EmpEmail { get; set; }
        public string Mail_Subject { get; set; }
        public string Mail_BodyHtml { get; set; }
    }
}