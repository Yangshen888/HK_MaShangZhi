using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.WorkStudy
{
    public class PostJobsRequestPayload:RequestPayload
    {
        /// <summary>
        /// 是否已被删除
        /// </summary>
        public CommonEnum.IsDeleted IsDeleted { get; set; }
        public int ReleaseState { get; set; }
        public int FullState { get; set; }
    }
}
