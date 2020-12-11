using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack
{
    public class WxSurveyAnswerViewModel
    {
        public Guid? SurveyAnswerUuid { get; set; }
        public Guid? SurveyUuid { get; set; }
        public string AnswerStr { get; set; }
        public string AddPeople { get; set; }
        public Guid? SchoolUuid { get; set; }
    }
}
