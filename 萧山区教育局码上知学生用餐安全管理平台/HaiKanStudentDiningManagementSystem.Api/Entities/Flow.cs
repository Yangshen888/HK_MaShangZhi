using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Flow
    {
        public int Id { get; set; }
        public Guid FlowUuid { get; set; }
        public string FlowName { get; set; }
        public string FlowTime { get; set; }
        public string AddTime { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string AddPeople { get; set; }
        public string Accessory { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
