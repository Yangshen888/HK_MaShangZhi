using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Synthesizes
    {
        public ulong Id { get; set; }
        public uint OrganizationId { get; set; }
        public ulong SynthesizeId { get; set; }
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int ContinueTime { get; set; }
        public string Department { get; set; }
        public string DepartmentName { get; set; }
        public string Reperson { get; set; }
        public string RepersonId { get; set; }
        public sbyte Status { get; set; }
        public string Introduce { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public uint? CreateUserId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdateUserId { get; set; }
        public string Imgs { get; set; }
        public string Result { get; set; }
        public int? Number { get; set; }
        public byte Sync { get; set; }
        public DateTime? SyncAt { get; set; }
    }
}
