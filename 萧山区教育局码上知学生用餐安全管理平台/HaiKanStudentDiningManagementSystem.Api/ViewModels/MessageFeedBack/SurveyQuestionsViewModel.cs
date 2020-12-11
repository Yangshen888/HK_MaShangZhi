using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack
{
    public class SurveyQuestionsViewModel
    {
        public Guid? SurveyQuestionsUuid { get; set; }
        public int? IsMuti { get; set; }
        public int? QuestionType { get; set; }
        public string QuestionTitle { get; set; }
        public Guid? SurveyUuid { get; set; }
    }
}
