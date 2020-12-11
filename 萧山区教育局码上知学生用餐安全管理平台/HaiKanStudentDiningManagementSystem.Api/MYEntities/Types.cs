using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Types
    {
        public ulong Id { get; set; }
        public uint TypeId { get; set; }
        public string KeyValue { get; set; }
        public string KeyName { get; set; }
        public string Description { get; set; }
        public sbyte Status { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
