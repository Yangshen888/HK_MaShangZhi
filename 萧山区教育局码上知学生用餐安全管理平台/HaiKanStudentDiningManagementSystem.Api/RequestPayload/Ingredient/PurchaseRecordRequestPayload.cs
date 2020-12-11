using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.Ingredient
{
    public class PurchaseRecordRequestPayload:RequestPayload
    {
        public string[] Kw2 { get; set; }
        public string Kw3 { get; set; }
    }
}
