using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.Student
{
    public class StudentRequestPayload : RequestPayload
    {
        public string kw1 { get; set; }
        public string kw2 { get; set; }
    }
}
