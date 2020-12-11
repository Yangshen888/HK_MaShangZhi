using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack
{
    public class SurveyQuestionDetail
    {
        public int? IsMuti { get; set; }
        public int? QuestionType { get; set; }
        public string QuestionTitle { get; set; }
        public List<SurveyQuestionItemDetail> SurveyQuestionItemDetail { get; set; }
        public string SubText { get; set; }
    }
}
