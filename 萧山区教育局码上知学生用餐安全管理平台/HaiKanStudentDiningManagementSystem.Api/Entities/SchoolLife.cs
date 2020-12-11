using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class SchoolLife
    {
        public int Id { get; set; }
        public Guid SchoollifeUuid { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string AddTime { get; set; }
        public string State { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string Accessory { get; set; }
        public string Tag { get; set; }
        public string Digest { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
