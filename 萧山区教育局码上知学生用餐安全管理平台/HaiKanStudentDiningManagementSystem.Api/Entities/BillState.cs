using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class BillState
    {
        public Guid BillUuid { get; set; }
        public long Id { get; set; }
        public string BillNum { get; set; }
        public string Appid { get; set; }
        public string MchId { get; set; }
        public int State { get; set; }
        public decimal Money { get; set; }
        public string Key { get; set; }
        public Guid? SbillUuid { get; set; }
    }
}
