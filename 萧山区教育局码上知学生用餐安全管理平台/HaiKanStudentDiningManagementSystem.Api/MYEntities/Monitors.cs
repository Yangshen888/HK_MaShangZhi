using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Monitors
    {
        public ulong Id { get; set; }
        public string FullName { get; set; }
        public uint OrganizationId { get; set; }
        public string Title { get; set; }
        public string MonitorName { get; set; }
        public uint MonitorBranchId { get; set; }
        public uint MonitorNodeId { get; set; }
        public string MonitorType { get; set; }
        public string HlsUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string SchoolName { get; set; }
        public string Procedure { get; set; }
    }
}
