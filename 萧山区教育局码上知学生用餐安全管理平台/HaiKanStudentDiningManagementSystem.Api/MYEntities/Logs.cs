using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Logs
    {
        public ulong Id { get; set; }
        public string ObjectType { get; set; }
        public ulong ObjectId { get; set; }
        public ulong OrganizationId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
