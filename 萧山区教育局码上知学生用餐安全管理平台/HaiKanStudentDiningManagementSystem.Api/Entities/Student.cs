using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Student
    {
        public Student()
        {
            Order = new HashSet<Order>();
        }

        public Guid StudentUuid { get; set; }
        public string StudentNum { get; set; }
        public string StudentName { get; set; }
        public string IdcardNum { get; set; }
        public int? Sex { get; set; }
        public Guid? SchoolUuid { get; set; }
        public int? IsDeleted { get; set; }
        public int Id { get; set; }
        public string ParentsPhone { get; set; }
        public string ClassName { get; set; }
        public string OederName { get; set; }
        public decimal AmountPayable { get; set; }
        public decimal? OederMoney { get; set; }

        public virtual School SchoolUu { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
