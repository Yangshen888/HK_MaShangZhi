using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.MessageFeedBack
{
    public class WxMessageBackRequestPayload:RequestPayload
    {
        public Guid? schoolUuid { get; set; }
        public string people { get; set; }
    }
}
