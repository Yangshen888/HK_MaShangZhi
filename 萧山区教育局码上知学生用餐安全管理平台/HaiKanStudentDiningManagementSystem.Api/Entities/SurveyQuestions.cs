using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class SurveyQuestions
    {
        public int Id { get; set; }
        public Guid SurveyQuestionsUuid { get; set; }
        public int? IsMuti { get; set; }
        public int? QuestionType { get; set; }
        public int? IsDelete { get; set; }
        public DateTime? AddTime { get; set; }
        public string AddPeople { get; set; }
        public string QuestionTitle { get; set; }
        public Guid? SchoolUuid { get; set; }
        public Guid? SurveyUuid { get; set; }
    }
}
