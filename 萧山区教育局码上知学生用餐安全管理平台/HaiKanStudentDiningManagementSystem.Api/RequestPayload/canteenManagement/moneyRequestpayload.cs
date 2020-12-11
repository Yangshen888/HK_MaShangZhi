using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.canteenManagement
{
    public class moneyRequestpayload : RequestPayload
    {
        public string kw { get; set; }
        public string kw1 { get; set; }
       //接受参数33333
        public string schoolguid { get; set; }
    }
}
