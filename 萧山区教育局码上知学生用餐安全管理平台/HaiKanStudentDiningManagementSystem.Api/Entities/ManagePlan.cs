using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class ManagePlan
    {
        public int Id { get; set; }
        public Guid ManageUuid { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
