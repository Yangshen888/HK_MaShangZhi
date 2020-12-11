using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.WorkStudy
{
    public class WxPostJobsAppealRequestPayload:RequestPayload
    {
        public string AppealPeople { get; set; }
        public Guid? SchoolUuid { get; set; }
    }
}
