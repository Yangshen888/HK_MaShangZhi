using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class SurveyFeedbackAnswer
    {
        public int Id { get; set; }
        public string AnswerStr { get; set; }
        public int? UserId { get; set; }
        public int? IsDelete { get; set; }
        public DateTime? AddTime { get; set; }
        public string AddPeople { get; set; }
        public string TrueName { get; set; }
        public int? Age { get; set; }
        public string Sex { get; set; }
        public int? GradeId { get; set; }
        public int? MajiorId { get; set; }
        public string Tel { get; set; }
        public string StudentNum { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAdress { get; set; }
        public string CompanyTrueName { get; set; }
        public string CompanyTel { get; set; }
        public Guid? SchoolUuid { get; set; }
        public Guid SurveyFeedbackUuid { get; set; }
    }
}
