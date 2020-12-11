using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Class
    {
        public Guid ClassUuid { get; set; }
        public string ClassName { get; set; }
        public int Id { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
