using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack
{
    public class SurveyQuestionsItemsViewModel
    {
        public Guid? SurveyQuestionsItemsUuid { get; set; }
        public string Optionts { get; set; }
        public string QuestionStr { get; set; }
        public Guid? SurveyQuestionsUuid { get; set; }
    }
}
