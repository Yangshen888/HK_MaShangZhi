using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.canteenManagement
{
    public class manageMessageRequestpayload : RequestPayload
    {
        public string kw { get; set; }
        public string kw1 { get; set; }
        public string schoolguid { get; set; }

    }
}
