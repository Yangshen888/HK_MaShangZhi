using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.PurchasesInfo
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
    public class PurchasesInfoRequestPayload : RequestPayload
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    {
        public string Kw { get; set; }
        public string Kw1 { get; set; }
        public string Kw2 { get; set; }
        public string Kw3 { get; set; }
        public List<string> Kw4 { get; set; }
    }
}
