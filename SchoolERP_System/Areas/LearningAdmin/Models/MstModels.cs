using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolERP_System.Areas.LearningAdmin.Models
{
    public class MstModels
    {
    }
    public class Specialization
    {
        public string SC_ID { get; set; }
        public string SC_Name { get; set; }
    }

    public class Subject
    {
        public string SM_Id { get; set; }
        public string SM_SubjectCode { get; set; }
        public string SM_SubjectName { get; set; }
    }
    public class Unit
    {
        public string UM_Id { get; set; }
        public string UM_UnitName { get; set; }
    }
    public class Topic
    {
        public string TM_Id { get; set; }
        public string TM_Name { get; set; }
    }
    public class DL
    {
        public string DL_ID { get; set; }
        public string DL_Name { get; set; }
    }
    public class Video
    {
        public string V_ID { get; set; }
        public string V_Name { get; set; }
        public string V_Description { get; set; }
        public string V_Path { get; set; }
    }
    public class SS
    {
        public string SS_ID { get; set; }
        public string Spec_ID { get; set; }
        public string Sub_ID { get; set; }
        public string SC_Name { get; set; }
        public string SM_SubjectName { get; set; }
    }
    public class SU
    {
        public string SU_ID { get; set; }
        public string Unit_ID { get; set; }
        public string Sub_ID { get; set; }
        public string UM_UnitName { get; set; }
        public string SM_SubjectName { get; set; }
    }
    public class UP
    {
        public string UP_ID { get; set; }
        public string Unit_ID { get; set; }
        public string Topic_ID { get; set; }
        public string UM_UnitName { get; set; }
        public string TM_Name { get; set; }
    }
    public class TV
    {
        public string TV_ID { get; set; }
        public string Video_ID { get; set; }
        public string Topic_ID { get; set; }
        public string V_Name { get; set; }
        public string TM_Name { get; set; }
    }
    public class VQ
    {
        public string QM_ID { get; set; }
        public string SC_Name { get; set; }
        public string SM_SubjectName { get; set; }
        public string UM_UnitName { get; set; }
        public string TM_Name { get; set; }
        public string DL_Name { get; set; }
        public string QM_Type { get; set; }
        public string QM_Question { get; set; }
    }

    public class VQ_EDIT
    {
        public string QM_ID { get; set; }
        public string QM_SC_ID { get; set; }
        public string QM_SubjectID { get; set; }
        public string QM_UnitID { get; set; }
        public string QM_TopicID { get; set; }
        public string QM_Level_ID { get; set; }
        public string QM_Type { get; set; }
        public string QM_Question { get; set; }
        public string QM_Paragaraph { get; set; }
        public string QM_Solution { get; set; }
        public string QM_SlNo1 { get; set; }
        public string QM_SlNo2 { get; set; }
        public string QM_SlNo3 { get; set; }
        public string QM_SlNo4 { get; set; }
        public string QM_Option1 { get; set; }
        public string QM_Option2 { get; set; }
        public string QM_Option3 { get; set; }
        public string QM_Option4 { get; set; }
    }
    public class LT
    {
        public string LT_ID { get; set; }
        public string LT_TestName { get; set; }
        public string LT_Date { get; set; }
        public string LT_FromTime { get; set; }
        public string LT_ToTime { get; set; }
        public string LT_SC_ID { get; set; }
        public string LT_SubjectID { get; set; }
        public string LT_Description { get; set; }
        public string SC_Name { get; set; }
        public string SM_SubjectName { get; set; }
    }
}