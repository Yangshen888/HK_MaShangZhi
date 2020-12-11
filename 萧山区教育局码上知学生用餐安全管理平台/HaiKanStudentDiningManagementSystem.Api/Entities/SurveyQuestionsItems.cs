using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class SurveyQuestionsItems
    {
        public int Id { get; set; }
        public Guid SurveyQuestionsItemsUuid { get; set; }
        public string Optionts { get; set; }
        public string QuestionStr { get; set; }
        public string AddPerson { get; set; }
        public DateTime? AddTime { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SurveyQuestionsUuid { get; set; }
        public Guid? SchoolUuid { get; set; }
    }
}
