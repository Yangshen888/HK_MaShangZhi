using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.PurchasesInfo
{
    public class MorningCheckInfoRequestPayload:RequestPayload
    {
        public ulong Id { get; set; }
        public uint Inspectionid { get; set; }
    }
}
