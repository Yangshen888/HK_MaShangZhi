using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class MessageBoard
    {
        public int Id { get; set; }
        public Guid MessageUuid { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public DateTime? MessageDate { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string Response { get; set; }
        public string ResponseType { get; set; }
        public DateTime? ResponseDate { get; set; }
        public int? State { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string AddPeople { get; set; }

        public virtual School SchoolUu { get; set; }
        public virtual SystemUser SystemUserUu { get; set; }
    }
}
