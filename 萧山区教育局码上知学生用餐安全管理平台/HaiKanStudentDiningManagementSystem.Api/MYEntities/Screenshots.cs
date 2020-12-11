using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Screenshots
    {
        public ulong Id { get; set; }
        public string Path { get; set; }
        public uint MonitorId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
