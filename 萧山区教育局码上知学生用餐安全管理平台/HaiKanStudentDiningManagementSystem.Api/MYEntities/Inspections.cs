using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Inspections
    {
        public ulong Id { get; set; }
        public ulong InspectionId { get; set; }
        public uint OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string ShouldCount { get; set; }
        public string ActualCount { get; set; }
        public string UnqualifiedCount { get; set; }
        public uint CreatedUser { get; set; }
        public string Inspector { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsSupply { get; set; }
        public sbyte Type { get; set; }
        public bool? IsDifferent { get; set; }
        public byte Sync { get; set; }
        public DateTime? SyncAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? TeamGroupId { get; set; }
        public string TeamGroupName { get; set; }
    }
}
