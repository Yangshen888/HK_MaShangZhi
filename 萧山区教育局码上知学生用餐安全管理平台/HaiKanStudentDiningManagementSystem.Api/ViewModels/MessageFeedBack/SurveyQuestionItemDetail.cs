using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack
{
    public class SurveyQuestionItemDetail
    {
        public string Optionts { get; set; }
        public string QuestionStr { get; set; }
        public bool checkbox { get; set; }
        public bool disabled { get; set; }
        public int num { get; set; }
    }
}
