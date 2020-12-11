using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Vegetables
    {
        public ulong Id { get; set; }
        public uint OrganizationId { get; set; }
        public uint VegetableId { get; set; }
        public uint TypeId { get; set; }
        public string Name { get; set; }
        public string Explain { get; set; }
        public bool? Del { get; set; }
        public uint CreateUserId { get; set; }
        public uint UpdateUserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
