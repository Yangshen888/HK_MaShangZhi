using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class DisinfectionRecords
    {
        public ulong Id { get; set; }
        public ulong DisinfectionId { get; set; }
        public ulong OwnerDisinfectionId { get; set; }
        public ulong DisinfectionRecordId { get; set; }
        public int? CreateUserId { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string Method { get; set; }
        public uint MethodId { get; set; }
        public uint Number { get; set; }
        public uint RoomId { get; set; }
        public string RoomName { get; set; }
        public uint ToolId { get; set; }
        public string ToolName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
