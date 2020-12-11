using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Lights
    {
        public ulong Id { get; set; }
        public uint LightId { get; set; }
        public int? OrganizationId { get; set; }
        public string Name { get; set; }
        public sbyte Type { get; set; }
        public bool? Del { get; set; }
        public uint CreateUserId { get; set; }
        public uint UpdateUserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
