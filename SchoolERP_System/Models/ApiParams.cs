using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolERP_System.Models
{
    public class ApiParams
    {
        public string AppID { get; set; }
        public string UserID { get; set; }
        public string UserPassword { get; set; }
        public string UserType { get; set; }
        public string Type { get; set; }
    }
    public class UpdateProfileParams
    {
        public string AppID { get; set; }
        public string UserID { get; set; }
        public string UserPassword { get; set; }
        public string UserType { get; set; }
        public string Type { get; set; }
        public byte[] ProfileLogo { get; set; }
        public string EM_EmpId { get; set; }
        public string EM_EmpName { get; set; }
        public string EM_DOB { get; set; }
        public string EM_Gender { get; set; }
        public string EM_EmpFathers { get; set; }
        public string EM_EmpAddress { get; set; }
        public string EM_Email { get; set; }
        public string EM_PanNo { get; set; }
        public string EM_AdharNo { get; set; }
        public string EM_Password { get; set; }
        public string SR_ID { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_AlternetContactNo { get; set; }
        public string SR_Email { get; set; }
        public string SR_DOB { get; set; }
        public string SR_Gender { get; set; }
        public string SR_Addrees { get; set; }
        public string P_EmpId { get; set; }
        public string P_EmpName { get; set; }
        public string P_DOB { get; set; }
        public string P_Gender { get; set; }
        public string P_EmpFathers { get; set; }
        public string P_EmpAddress { get; set; }
        public string P_Email { get; set; }
        public string P_PanNo { get; set; }
        public string P_AdharNo { get; set; }
        public string P_Password { get; set; }
    }
    public class ApiProfile
    {
        public string AppID { get; set; }
        public string SchName { get; set; }
        public string SchAddress { get; set; }
        public string Contact { get; set; }
        public string EmailID { get; set; }
        public string UserPassword { get; set; }
        public byte[] SchLogo { get; set; }
    }
    public class ApiPost
    {
        public byte[] FileData { get; set; }
        public string Filetype { get; set; }
        public string Type { get; set; }
        public string PostID { get; set; }
        public string ClassID { get; set; }
        public string SectionID { get; set; }
        public string PostMode { get; set; }
        public string PostData { get; set; }
        public string UserType { get; set; }
        public string UserID { get; set; }
        public string AppID { get; set; }
        public string StudentID { get; set; }

    }
    public class ApiSupport
    {
        public string Type { get; set; }
        public string AppID { get; set; }
        public string SupportID { get; set; }
        public string IssueTitle { get; set; }
        public string IssueDesc { get; set; }
        public string IssueBy { get; set; }
        public string UserID { get; set; }

    }
    public class ApiEnquiry
    {
        public string Type { get; set; }
        public string AppID { get; set; }
        public string EnquiryID { get; set; }
        public string ClassID { get; set; }
        public string E_Date { get; set; }
        public string E_Type { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string FatherName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string NextFollowUpNote { get; set; }
        public string NextFollowDate { get; set; }
        public List<EnquiryDetailsAPI> enqData;
    }

    public class EnquiryDetailsAPI
    {
        public string FollowUpDate { get; set; }
        public string Messege { get; set; }
        public string Remarks { get; set; }
    }

    public class ApiPreAdmitted
    {
        public string Type { get; set; }
        public string AppID { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_Gender { get; set; }
        public string SR_DOB { get; set; }
        public string SR_PhNo { get; set; }
        public string SR_ClassID { get; set; }
        public string SR_SectionID { get; set; }
        public string SR_CategoryID { get; set; }
        public string SR_PunchingCode { get; set; }
        public string SR_ID { get; set; }
        public byte[] SR_Pic { get; set; }
    }
    public class ApiCommon
    {
        public string Type { get; set; }
        public string AppID { get; set; }
        public string ID { get; set; }
    }

    public class ApiAttendacne
    {
        public string Type { get; set; }
        public string ClassID { get; set; }
        public string SectionID { get; set; }
        public string MonthID { get; set; }
        public string UserType { get; set; }
        public string UserID { get; set; }
        public string AppID { get; set; }
        public List<AttendanceSave> Student { get; set; }
        public string MonthText { get; set; }
        public string ExamID { get; set; }
    }
    public class ResponseAttendanceObj
    {
        public int success { get; set; }
        public string message { get; set; }
        public List<ResponseAttendance> Attendance;
    }
    public class ResponseAttendance
    {
        public string ID { get; set; }
        public string StudentID { get; set; }
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string PDays { get; set; }
        public string DaysInMonth { get; set; }
        public string SunDayCount { get; set; }
        public string HolidayCount { get; set; }
        public string TotalWorkingDays { get; set; }
        public List<Responsevalue> AttendanceValue { get; set; }
    }
    public class Responsevalue
    {
        public string Day { get; set; }
        public string Status { get; set; }
    }
}