using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.Class
{
    public class ClassRequestpayload : RequestPayload
    {
        public string Kw { get; set; }
        public Guid? Sc { get; set; }
    }
}
