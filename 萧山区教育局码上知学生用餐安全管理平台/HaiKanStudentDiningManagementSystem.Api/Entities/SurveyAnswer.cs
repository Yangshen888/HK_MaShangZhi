using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class SurveyAnswer
    {
        public int Id { get; set; }
        public Guid SurveyAnswerUuid { get; set; }
        public Guid? SurveyUuid { get; set; }
        public string AnswerStr { get; set; }
        public int? IsDelete { get; set; }
        public DateTime? AddTime { get; set; }
        public string AddPeople { get; set; }
        public Guid? SchoolUuid { get; set; }
    }
}
