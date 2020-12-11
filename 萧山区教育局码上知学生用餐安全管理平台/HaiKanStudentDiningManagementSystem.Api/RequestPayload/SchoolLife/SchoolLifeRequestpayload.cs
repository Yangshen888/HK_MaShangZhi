using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.SchoolLife
{
    public class SchoolLifeRequestpayload : RequestPayload
    {
        public string kw { get; set; }
        public string kw1 { get; set; }
        public string schoolguid { get; set; }
    }
}
