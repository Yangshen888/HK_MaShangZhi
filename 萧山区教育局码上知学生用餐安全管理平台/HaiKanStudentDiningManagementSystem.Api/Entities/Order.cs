using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Order
    {
        public Guid OrderUuid { get; set; }
        public string OrderNum { get; set; }
        public string OederName { get; set; }
        public decimal? OederMoney { get; set; }
        public decimal? AmountPayable { get; set; }
        public int Id { get; set; }
        public Guid? StudentUuid { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string Phone { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public DateTime? OederTime { get; set; }

        public virtual School SchoolUu { get; set; }
        public virtual Student StudentUu { get; set; }
        public virtual SystemUser SystemUserUu { get; set; }
    }
}
