using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class ElectronicBill
    {
        public int Id { get; set; }
        public Guid ElectronicUuid { get; set; }
        public string Supplier { get; set; }
        public string Phone { get; set; }
        public string PurchaseTime { get; set; }
        public string CuisineName { get; set; }
        public string Specification { get; set; }
        public string Quantity { get; set; }
        public string ProducedTime { get; set; }
        public string ExpirationTime { get; set; }
        public string Rt { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
