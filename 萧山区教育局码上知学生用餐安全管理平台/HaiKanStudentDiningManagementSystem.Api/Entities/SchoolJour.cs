using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class SchoolJour
    {
        public int Id { get; set; }
        public Guid SchoolJourUuid { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string AddPeople { get; set; }
        public string Addtime { get; set; }
        public string Start { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string Accessory { get; set; }
        public string Tag { get; set; }
        public string Digest { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
