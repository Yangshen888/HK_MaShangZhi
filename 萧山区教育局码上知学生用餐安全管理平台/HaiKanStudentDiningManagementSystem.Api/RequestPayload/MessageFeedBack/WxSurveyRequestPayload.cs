using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.MessageFeedBack
{
    public class WxSurveyRequestPayload:RequestPayload
    {
        public Guid? SchoolUuid { get; set; }
        public string People { get; set; }
    }
}
