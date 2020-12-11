using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class SurverUser
    {
        public int Id { get; set; }
        public Guid SurverUserUuid { get; set; }
        public string Age { get; set; }
        public string Major { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string StudentNum { get; set; }
        public string Phone { get; set; }
        public string Remark { get; set; }
        public string AddTime { get; set; }
        public Guid? SurverUuid { get; set; }
        public int? IsDelete { get; set; }
    }
}
