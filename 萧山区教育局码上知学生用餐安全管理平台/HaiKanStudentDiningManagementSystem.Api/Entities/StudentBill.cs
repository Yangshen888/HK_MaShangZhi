using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class StudentBill
    {
        public Guid StudentBillUuid { get; set; }
        public string StudentNum { get; set; }
        public string StudentName { get; set; }
        public string IdcardNum { get; set; }
        public int? Sex { get; set; }
        public Guid? SchoolUuid { get; set; }
        public int? IsDeleted { get; set; }
        public int Id { get; set; }
        public string ParentsPhone { get; set; }
        public string ClassName { get; set; }
        public string OrderName { get; set; }
        public decimal? AmountPayable { get; set; }
        public decimal? OrderMoney { get; set; }
        public string SerialNumber { get; set; }
        public string OrderNum { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string OederTime { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
