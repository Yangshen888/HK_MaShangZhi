using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.Rbac.School
{
    public class SchoolRequestPayload:RequestPayload
    {
        public CommonEnum.IsDeleted IsDeleted { get; set; }
    }
}
