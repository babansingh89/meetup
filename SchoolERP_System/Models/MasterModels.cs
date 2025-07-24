using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolERP_System.Models
{
    public class ClassMstModels
    {
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassStrength { get; set; }
    }
    public class Section
    {
        public string SectionID { get; set; }
        public string SectionName { get; set; }
    }
    public class CS
    {
        public string ClsSecID { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string SectionID { get; set; }
        public string SectionName { get; set; }
    }
    public class CSubj
    {
        public string ClsSecID { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string DocPath { get; set; }
        public string ClsSubID { get; set; }
    }

    public class Session
    {
        public string SessionID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string IsCurrent { get; set; }
    }
    public class Streamcl
    {
        public string StreamID { get; set; }
        public string StreamName { get; set; }
    }
    public class Route
    {
        public string RouteID { get; set; }
        public string RouteName { get; set; }
    }
    public class CSt
    {
        public string ClsStrID { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string StreamID { get; set; }
        public string StreamName { get; set; }
    }
    public class Holiday
    {
        public string HolidayID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Reason { get; set; }
    }
    public class RouteWiseFees
    {
        public string RouteFeeID { get; set; }
        public string RouteID { get; set; }
        public string RouteName { get; set; }
        public string Route_Fee { get; set; }
        public string Fee_Date { get; set; }
    }
    public class FeeHead
    {
        public string FeeHeadID { get; set; }
        public string FeeHeadName { get; set; }
        public string Total_of_Installment { get; set; }
        public string At_Admission { get; set; }
        public string Installment_TOA { get; set; }
        public string Is_Bus_Fees { get; set; }
        public string Is_Admission_Fees { get; set; }
        public string Is_Session_Fees { get; set; }
        public string Is_Others_Fees { get; set; }
        public string Others_Fees_Month { get; set; }
        public string Is_Bengali_Fees { get; set; }
        public string IsActive { get; set; }
        public string FeesType { get; set; }
    }
    public class ClassWiseFeeHead
    {
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string CategoryID { get; set; }
        public string StreamID { get; set; }
        public string StreamName { get; set; }
        public string Category { get; set; }
        public string FeeHeadID { get; set; }
        public string FeeHeadName { get; set; }
        public string Total_No_Ins { get; set; }
        public string Total_Fee_Amt { get; set; }
        public string Ins_No { get; set; }
        public string Ins_Amt { get; set; }
        public string Due_Date { get; set; }
        public string Late_Fee { get; set; }
        public string isactive { get; set; }
        public string Status { get; set; }
    }
    public class ClassWiseFeeHeadDetails
    {
        public string ClassID { get; set; }
        public string CategoryID { get; set; }
        public string FeeHeadID { get; set; }
        public string Total_No_Ins { get; set; }
        public string Total_Fee_Amt { get; set; }
        public string Ins_No { get; set; }
        public string Ins_Amt { get; set; }
        public string Due_Date { get; set; }
        public string Late_Fee { get; set; }
        public string Active { get; set; }
    }
    public class ClassWiseFeeHeadDtl
    {
        public string ClassID { get; set; }
        public string CategoryID { get; set; }
        public string FeeHeadID { get; set; }
        public string Ins_Amt { get; set; }
        public string Late_Fee { get; set; }
        public string Active { get; set; }
    }
    public class Enquiry
    {
        public string EnquiryID { get; set; }
        public string SessionID { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string E_Type { get; set; }
        public string E_Date { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string FatherName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string NextFollowDate { get; set; }
        public string NextFollowUpNote { get; set; }
        public string E_Mapping { get; set; }
        public string FollowUpDate { get; set; }
        public string Messege { get; set; }
        public string Remarks { get; set; }
    }

    public class EnquiryDetails
    {
        public string FollowUpDate { get; set; }
        public string Messege { get; set; }
        public string Remarks { get; set; }
    }
    public class SR
    {
        public string SR_ID { get; set; }
        public string SR_RegNo { get; set; }
        public string SR_Date { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_DOB { get; set; }
        public string SR_PhNo { get; set; }
        public string ClassName { get; set; }
        public string UserID { get; set; }
        public string SNo { get; set; }
        public string SR_CurrentRollNo { get; set; }
        public string SR_FatherName { get; set; }
        public string SR_MotherName { get; set; }
        public string SR_FatherOccupation { get; set; }
        public string SR_MotherOccupation { get; set; }
        public string SR_Shift { get; set; }
        public string SR_SMS { get; set; }
        public string SR_Email { get; set; }
        public string SR_AlternetContactNo { get; set; }
        public string SR_Gender { get; set; }
        public string SR_PunchingCode { get; set; }
        public string SR_LocalGurdianName { get; set; }
        public string SR_LocalGurdianMobile { get; set; }
        public string SR_Addrees { get; set; }
        public string SR_ClassID { get; set; }
        public string SR_CategoryID { get; set; }
        public string SR_StreamID { get; set; }
        public string SR_SectionID { get; set; }
        public string SR_Pic { get; set; }
        public string SR_AddressPic { get; set; }
        public string SR_CirtificatePic { get; set; }
        public string SR_SessionID { get; set; }
        public string IsDiscount { get; set; }
        public string IsAvailBus { get; set; }
        public string RouteID { get; set; }
        public string IsAdmitted { get; set; }
        public string SR_EN_ID { get; set; }
        public string SR_Caste { get; set; }
        public string SectionName { get; set; }
        public string UserPassword { get; set; }
        public string AppID { get; set; }
        public string OptedMonth { get; set; }
        public string OptedHostelMonth { get; set; }
        public string BusPayMonth { get; set; }
        public string HostelPayMonth { get; set; }
        public string MediumType { get; set; }
        
    }

    public class ST
    {
        public string STID { get; set; }
        public string STName { get; set; }
        public string STDesc { get; set; }
    }
    public class SelectStudent
    {
        public string SR_ID { get; set; }
        public string SR_RegNo { get; set; }
        public string SR_CurrentRollNo { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_PhNo { get; set; }
        public string SectionName { get; set; }
        public string Due { get; set; }
        public string IsPresent { get; set; }
    }
    public class MonthList
    {
        public string ID { get; set; }
        public string MonthNameDesc { get; set; }
    }
    public class AttendanceSave
    {
        // public string Mobile { get; set; }
        public string StudentID { get; set; }
        public string IsPresent { get; set; }
    }
    public class SMSStudent
    {
        public string EMPID { get; set; }
        public string StudentID { get; set; }
        public string Mobile { get; set; }
    }
    public class SMSStudentIndividual
    {
        public string EMPID { get; set; }
        public string StudentID { get; set; }
        public string Mobile { get; set; }
        public string strText { get; set; }
    }
    public class SMSStudentAllClass
    {
        public string SR_ID { get; set; }
        public string SR_PhNo { get; set; }
        public string SR_StudentName { get; set; }
    }
    public class Employee
    {
        public string EM_EmpId { get; set; }
        public string EM_EmpCode { get; set; }
        public string EM_EmpName { get; set; }
        public string EM_EmpDOJ { get; set; }
        public string EM_EmpDesignationId { get; set; }
        public string EM_EmpFathers { get; set; }
        public string EM_EmpAddress { get; set; }
        public string EM_EmpContactNo { get; set; }
        public string EM_TotalSal { get; set; }
        public string EM_Degree { get; set; }
        public string EM_NIDNo { get; set; }
        public string EM_DA { get; set; }
        public string EM_Basic { get; set; }
        public string EM_HRA { get; set; }
        public string EM_Conveyance { get; set; }
        public string EM_MedicalAllowance { get; set; }
        public string EM_TelephoneAllowance { get; set; }
        public string EM_SpecialAllowance { get; set; }
        public string EM_Reimbursement { get; set; }
        public string EM_Leave { get; set; }
        public string EM_ProvidentFund { get; set; }
        public string EM_Ptax { get; set; }
        public string EM_TDS { get; set; }
        public string EM_DepartmentID { get; set; }
        public string EM_Bank { get; set; }
        public string EM_AccountNo { get; set; }
        public string EM_IFSC { get; set; }
        public string EM_CompanyID { get; set; }
        public string EM_Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string EM_ProfilePic { get; set; }
        public string EM_PunchCode { get; set; }
        public string Sno { get; set; }
        public string EmpDesignationName { get; set; }
        public string DesName { get; set; }

        public string EM_DOB { get; set; }
        public string EM_Gender { get; set; }
        public string EM_UserType { get; set; }
        public string EM_PanNo { get; set; }
        public string EM_AdharNo { get; set; }


    }
    public class Desgination
    {
        public string DesID { get; set; }
        public string DesName { get; set; }
        public string DesCode { get; set; }
    }

    public class Shift
    {
        public string ShiftID { get; set; }
        public string ClassID { get; set; }
        public string ShiftFrom { get; set; }
        public string ShiftTo { get; set; }
        public string ClassName { get; set; }
    }
    public class RollSettings
    {
        public string SR_ID { get; set; }
        public string SR_RegNo { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_CurrentRollNo { get; set; }
    }
    public class Promotion
    {
        public string SR_ID { get; set; }
        public string SR_RegNo { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_CurrentRollNo { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SR_PhNo { get; set; }
    }
    public class Exam
    {
        public string ExamID { get; set; }
        public string ExamName { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
    }
    public class OQ
    {
        public string OPID { get; set; }
        public string TicketNo { get; set; }
        public string EM_EmpName { get; set; }
        public string TeacherID { get; set; }
        public string StudentID { get; set; }
        public string SR_StudentName { get; set; }
        public string Subject { get; set; }
        public string OPStatus { get; set; }
        public string CreatedDate { get; set; }
        public string PostData { get; set; }
        public string UserType { get; set; }
        public string LastReply { get; set; }
        public string ClassName { get; set; }
    }
    public class printAdmitCls
    {
        public string SR_CurrentRollNo { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_RegNo { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string ExamName { get; set; }
    }
    public class printIDCls
    {
        public string SR_CurrentRollNo { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_RegNo { get; set; }
        public string ClassName { get; set; }
        public string SR_DOB { get; set; }
        public string SR_PhNo { get; set; }
        public string SR_Pic { get; set; }
        public string SR_Addrees { get; set; }
    }

    public class FeeDetails
    {
        public string SR_ID { get; set; }
        public string SR_RegNo { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_CurrentRollNo { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SR_PhNo { get; set; }
        public string StreamName { get; set; }
        public string Session_ActiveMonth { get; set; }
        public string PayMonth { get; set; }

    }
    public class FeesDetails
    {
        public string Ins_No { get; set; }
        public string Ins_Amt { get; set; }
        public string Due_Date { get; set; }
        public string Late_Fee { get; set; }
        public string Total_Fee_Amt { get; set; }
        public string Total_No_Ins { get; set; }
        public string FeeHeadName { get; set; }
        public string FeeHeadID { get; set; }
        public string SFCT_PaymentAmount { get; set; }
        public string SFCT_DiscountAmount { get; set; }
        public string SFCT_DueAmount { get; set; }
        public string MediumType { get; set; }
        public string IS_PaymentDone { get; set; }
        
    }
    public class FeeCollection
    {
        public string SFCT_FeeHeadID { get; set; }
        public string SFCT_InstallmentNo { get; set; }
        public string SFCT_LateFee { get; set; }
        public string SFCT_PaymentAmount { get; set; }
        public string SFCT_DiscountAmount { get; set; }
        public string SFCT_DueAmount { get; set; }


    }
    public class Pay_Mode_Details
    {
        public string Pay_Mode { get; set; }
        public string Bank_Name { get; set; }
        public string Branch_Name { get; set; }
        public string D_C_No { get; set; }
        public string Pay_Date { get; set; }
        public string Pay_Amount { get; set; }
    }
    public class FeeCollectionDetails
    {
        public string SFC_Id { get; set; }
        public string SFC_PaymentAmount { get; set; }
        public string SFC_Date { get; set; }
        public string SFC_ReceiptNo { get; set; }
        public string SR_StudentName { get; set; }
        public string ClassName { get; set; }
        public string SR_RegNo { get; set; }
        public string PayMonth { get; set; }
        public string PayMonthID { get; set; }
        
        public string SR_Gender { get; set; }
        public string SectionName { get; set; }
        public string SR_CurrentRollNo { get; set; }
        public string SR_ID{ get; set; }
        public string SR_PhNo { get; set; }
        public string SFC_PaymentAmountOther { get; set; }
    }
    public class FeeCollectionDetailsPrint
    {
        public string SR_CurrentRollNo { get; set; }
        public string SFC_PaymentAmount { get; set; }
        public string SFC_Date { get; set; }
        public string SFC_ReceiptNo { get; set; }
        public string SR_StudentName { get; set; }
        public string ClassName { get; set; }
        public string SR_RegNo { get; set; }
        public string SectionName { get; set; }
    }
    public class FeeCollectionPrint
    {
        public string SFCT_InstallmentNo { get; set; }
        public string SFCT_LateFee { get; set; }
        public string SFCT_PaymentAmount { get; set; }
        public string SFCT_DiscountAmount { get; set; }
        public string SFCT_DueAmount { get; set; }
        public string FeeHeadName { get; set; }
        public string MonthYear { get; set; }


    }
    public class SubjectMstModels
    {
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
    }
    public class Studentmarks
    {
        public string SR_ID { get; set; }
        public string SR_RegNo { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_CurrentRollNo { get; set; }
        public string SectionName { get; set; }
        public string Theory { get; set; }
        public string Practical { get; set; }
        public string Oral { get; set; }
        public string Projects { get; set; }
        public string Internal { get; set; }
        //public string MaxMarks { get; set; }
        //public string PassMarks { get; set; }
    }
    public class ViewStudentmarks
    {
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string ExamID { get; set; }
        public string ExamName { get; set; }
        public string SubjectName { get; set; }
    }

    public class Expensescls
    {
        public string ExpensesID { get; set; }
        public string ExpensesName { get; set; }
        public string ExpEntID { get; set; }
        public string Desc { get; set; }
        public string Amount { get; set; }
        public string EntryDate { get; set; }
        public string PayMode { get; set; }
        public string BM_Name { get; set; }
        public string DraftNo { get; set; }
        public string DraftDate { get; set; }
        public string BranchName { get; set; }
    }
    public class EmployeeFile
    {
        public string EMT_Name { get; set; }
        public string EMT_File { get; set; }
    }
    public class LoanTransaction
    {
        public string LaonTranID { get; set; }
        public string LoanID { get; set; }
        public string EmpID { get; set; }
        public string InsNo { get; set; }
        public string Amount { get; set; }
        public string InsDate { get; set; }
    }
    public class LoanMaster
    {
        public string LoanID { get; set; }
        public string LoanDate { get; set; }
        public string LoanType { get; set; }
        public string EmpID { get; set; }
        public string Amount { get; set; }
        public string Intrest { get; set; }
        public string AdvMonth { get; set; }
        public string AdvYear { get; set; }
        public string NoOfInstall { get; set; }
        public string LoanStatus { get; set; }
        public string EM_EmpName { get; set; }
    }
    public class SalaryProcess
    {
        public string EM_ID { get; set; }
        public string TotalSal { get; set; }
        public string LM_Amount { get; set; }
        public string InstNo { get; set; }
        public string InstAmnt { get; set; }
        public string Advance { get; set; }
        public string SpclAllnce { get; set; }
        public string Inecentive { get; set; }
        public string OthDeduction { get; set; }
        public string NetAmnt { get; set; }

        public string MonthID { get; set; }
        public string YearID { get; set; }
        public string EM_EmpName { get; set; }
    }
    public class EmpAttendacne
    {
        public string EM_EmpId { get; set; }
        public string EM_EmpCode { get; set; }
        public string EM_EmpName { get; set; }
        public string EM_EmpContactNo { get; set; }
        public string IsPresent { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Attendance_Type { get; set; }
        
    }
    public class SaveEmpAttendacne
    {
        public string EmpID { get; set; }
        public string IsPresent { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string AttendanceDate { get; set; }
    }
    public class SchoolModel
    {
        public string AppID { get; set; }
        public string SchName { get; set; }
        public string SchAddress { get; set; }
        public string Contact { get; set; }
        public string EmailID { get; set; }
        public string LicenceFrom { get; set; }
        public string LicenceTo { get; set; }
        public string UserID { get; set; }
        public string UserPassword { get; set; }
        public string IsPresentSMS { get; set; }
        public string IsAbsentSMS { get; set; }
        public string SchoolLogo { get; set; }
    }
    public class SchoolSupport
    {
        public string SupportID { get; set; }
        public string AppID { get; set; }
        public string TicketNo { get; set; }
        public string IssueTitle { get; set; }
        public string IssueDesc { get; set; }
        public string Status { get; set; }
        public string ResolveDesc { get; set; }
        public string ResolveDate { get; set; }
        public string UserID { get; set; }
        public string CreatedDate { get; set; }
    }
    public class Dashboard
    {
        public string StudentCount { get; set; }
        public string PresentCount { get; set; }
        public string AbsentCount { get; set; }
        public string DueCount { get; set; }
    }
    public class DashboardAttendance
    {
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string Total { get; set; }
        public string Present { get; set; }
        public string Absent { get; set; }

    }
    public class printEmpAttendance
    {
        public string EMPID { get; set; }
        public string EM_EmpCode { get; set; }
        public string EM_EmpName { get; set; }
        public string EM_EmpDOJ { get; set; }
        public string EM_EmpContactNo { get; set; }
        public string Pass { get; set; }
        public string Fail { get; set; }
        public string Per { get; set; }
        public string WD { get; set; }
    }
    public class printStRegistration
    {
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SR_RegNo { get; set; }
        public string SR_CurrentRollNo { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_FatherName { get; set; }
        public string SR_PhNo { get; set; }
        public string SR_DOB { get; set; }
        public string SR_Gender { get; set; }
    }
    public class printStPer
    {
        public string SR_ID { get; set; }
        public string SR_RegNo { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_DOB { get; set; }
        public string SR_PhNo { get; set; }
        public string WD { get; set; }
        public string Pass { get; set; }
        public string Fail { get; set; }
        public string Per { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SR_CurrentRollNo { get; set; }
        public string AppID { get; set; }
    }
    public class printStMonth
    {
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SFC_ReceiptNo { get; set; }
        public string SFC_Date { get; set; }
        public string SR_RegNo { get; set; }
        public string SR_StudentName { get; set; }
        public string SFC_TotalAmount { get; set; }
        public string SFC_DiscountAmount { get; set; }
        public string SFC_PaymentAmount { get; set; }
        public string SFC_DueAmount { get; set; }
    }
    public class printStDue
    {
        public string SR_RegNo { get; set; }
        public string SR_CurrentRollNo { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_PhNo { get; set; }
        public string Due { get; set; }
        public string className { get; set; }
        public string SectionName { get; set; }
    }
    public class ClassRotineModel
    {
        public string RID { get; set; }
        public string Days { get; set; }
        public string SubjectName { get; set; }
        public string EM_EmpName { get; set; }
        public string StHr { get; set; }
        public string EnHr { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }

    }
    public class ClassRotineModelEdit
    {
        public string Id { get; set; }
        public string ClassID { get; set; }
        public string SectionID { get; set; }
        public string SubjectID { get; set; }
        public string TeacherID { get; set; }
        public string Days { get; set; }
        public string StHr { get; set; }
        public string StMn { get; set; }
        public string EnHr { get; set; }
        public string EnMn { get; set; }
    }
    public class Notice
    {
        public string NID { get; set; }
        public string ViewTo { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string NTitle { get; set; }
        public string NDate { get; set; }
        public string NDescription { get; set; }
    }
    public class ExamMaster
    {
        public string StudentID { get; set; }
        public string SR_RegNo { get; set; }
        public string SR_StudentName { get; set; }
        public string ClassName { get; set; }
        public string SR_CurrentRollNo { get; set; }
        public string SectionName { get; set; }
        public string ExamName { get; set; }
    }
    public class ExamDetails
    {
        public string StudentID { get; set; }
        public string SubjectName { get; set; }
        public string MaxMarks { get; set; }
        public string PassMarks { get; set; }
        public string Theory { get; set; }
        public string Practical { get; set; }
        public string Oral { get; set; }
        public string Projects { get; set; }
        public string Internal { get; set; }
    }
    public class Book
    {
        public string BookID { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string ISBN { get; set; }
        public string Stock { get; set; }
        public string StockOut { get; set; }
    }
    public class IssueBook
    {
        public string IssueID { get; set; }
        public string IssueDate { get; set; }
        public string BookStatus { get; set; }
        public string RetrunDate { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_RegNo { get; set; }
        public string ClassName { get; set; }
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public string ClassID { get; set; }
        public string SectionID { get; set; }
        public string BookID { get; set; }
        public string SR_ID { get; set; }
    }
    public class EmpAssignModel
    {
        public string EmpID { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SectionList { get; set; }
    }
    public class ParenrsStudent
    {
        public string SR_ID { get; set; }
        public string SR_ClassID { get; set; }
        public string SR_SectionID { get; set; }
        public string SR_StudentName { get; set; }
        public string SR_SessionID { get; set; }
        public string SR_RegNo { get; set; }
        public string SR_CurrentRollNo { get; set; }
        public string SR_DefaultLogin { get; set; }
        public string UserID { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
    }
    public class ParentsModel
    {
        public string AppID { get; set; }
        public string UserType { get; set; }
        public string P_EmpId { get; set; }
        public string P_EmpName { get; set; }
        public string P_EmpContactNo { get; set; }
        public string P_DOB { get; set; }
        public string P_Gender { get; set; }
        public string P_EmpFathers { get; set; }
        public string P_EmpAddress { get; set; }
        public string UserPassword { get; set; }
        public string P_PanNo { get; set; }
        public string P_AdharNo { get; set; }
        public string P_Email { get; set; }
        public string P_ProfilePic { get; set; }
    }
    public class ViewPostModel
    {
        public string PostID { get; set; }
        public string EM_EmpName { get; set; }
        public string ClassName { get; set; }
        public string EM_ProfilePic { get; set; }
        public string PostMode { get; set; }
        public string PostData { get; set; }
        public string UserType { get; set; }
        public string SectionName { get; set; }
        public string CreatedDate { get; set; }
    }
    public class ViewPostINDModel
    {
        public string PostINDID { get; set; }
        public string EM_EmpName { get; set; }
        public string EM_ProfilePic { get; set; }
        public string PostData { get; set; }
        public string UserType { get; set; }
        public string CreatedDate { get; set; }

    }
    public class EditPostModel
    {
        public string PostID { get; set; }
        public string ClassID { get; set; }
        public string PostMode { get; set; }
        public string PostData { get; set; }
        public string SectionID { get; set; }
    }
    public class ExamTimetable
    {
        public string ETID { get; set; }
        public string ExamDate { get; set; }
        public string ExamDay { get; set; }
        public string SubjectName { get; set; }
        public string ExamName { get; set; }
        public string ClassName { get; set; }
        public string StHr { get; set; }
        public string EnHr { get; set; }
        public string ExamID { get; set; }
        public string SubjectID { get; set; }
        public string ClassID { get; set; }
        public string StMn { get; set; }
        public string EnMn { get; set; }
    }
    public class StudentCalenderModel
    {

        public string start { get; set; }
        public string end { get; set; }
        public string summary { get; set; }
        public string mask { get; set; }
    }
    public class StudentReportModel
    {
        public string _ID { get; set; }
        public string _StudentID { get; set; }
        public string _StudentCode { get; set; }
        public string _StudentName { get; set; }
        public string _PDays { get; set; }
        public string _1 { get; set; }
        public string _2 { get; set; }
        public string _3 { get; set; }
        public string _4 { get; set; }
        public string _5 { get; set; }
        public string _6 { get; set; }
        public string _7 { get; set; }
        public string _8 { get; set; }
        public string _9 { get; set; }
        public string _10 { get; set; }
        public string _11 { get; set; }
        public string _12 { get; set; }
        public string _13 { get; set; }
        public string _14 { get; set; }
        public string _15 { get; set; }
        public string _16 { get; set; }
        public string _17 { get; set; }
        public string _18 { get; set; }
        public string _19 { get; set; }
        public string _20 { get; set; }
        public string _21 { get; set; }
        public string _22 { get; set; }
        public string _23 { get; set; }
        public string _24 { get; set; }
        public string _25 { get; set; }
        public string _26 { get; set; }
        public string _27 { get; set; }
        public string _28 { get; set; }
        public string _29 { get; set; }
        public string _30 { get; set; }
        public string _31 { get; set; }
        public string _DaysInMonth { get; set; }
        public string _SunDayCount { get; set; }
        public string _HolidayCount { get; set; }
        public string _TotalWorkingDays { get; set; }
    }

    public class FaceIdentification
    {
        public string deviceID { get; set; }
        public string deviceSerialno { get; set; }
        public string employeeID { get; set; }
        public string date { get; set; }
        public string modeofPunch { get; set; }
        public string modeofAttn { get; set; }
        public string time { get; set; }
        public string ip { get; set; }
    }
}