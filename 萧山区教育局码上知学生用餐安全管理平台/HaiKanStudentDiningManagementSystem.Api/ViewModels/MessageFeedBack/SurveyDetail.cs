using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack
{
    public class SurveyDetail
    {
        public Guid SurveyUuid { get; set; }
        public string Headline { get; set; }
        public string Type { get; set; }
        public List<SurveyQuestionDetail> SurveyQuestionDetail { get; set; }
    }
}
