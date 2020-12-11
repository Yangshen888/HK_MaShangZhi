using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Duties
    {
        public ulong Id { get; set; }
        public int? DutyId { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public sbyte Status { get; set; }
        public bool Sync { get; set; }
        public DateTime? SyncAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
