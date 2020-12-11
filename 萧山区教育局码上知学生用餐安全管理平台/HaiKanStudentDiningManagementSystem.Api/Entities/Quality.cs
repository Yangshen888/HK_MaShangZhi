using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Quality
    {
        public Guid QualityUuid { get; set; }
        public string FlieName { get; set; }
        public string JianJie { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string Accessory { get; set; }
        public string AddTime { get; set; }
        public string EffectiveTime { get; set; }
    }
}
