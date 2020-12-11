using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Departments
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public int OrganizationId { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
