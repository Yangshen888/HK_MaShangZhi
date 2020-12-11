using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Report
    {
        public int Id { get; set; }
        public Guid ReportUuid { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public DateTime? ReportDate { get; set; }
        public string Result { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string ReplyName { get; set; }
        public DateTime? ReplyTime { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public int? State { get; set; }

        public virtual School SchoolUu { get; set; }
        public virtual SystemUser SystemUserUu { get; set; }
    }
}
