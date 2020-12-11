using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.WorkStudy
{
    public class WxPostJobsRequestPayload:RequestPayload
    {
        public string SchoolUuid { get; set; }
        public string appealPeople { get; set; }
    }
}
