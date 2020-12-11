using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack
{
    public class SurveyViewModel
    {
        public Guid? SurveyUuid { get; set; }
        public string Headline { get; set; }
        public string Type { get; set; }
        public Guid? SchoolUuid { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? ProceedState { get; set; }
        public int? ProductState { get; set; }
        public int? Number { get; set; }
    }
}
