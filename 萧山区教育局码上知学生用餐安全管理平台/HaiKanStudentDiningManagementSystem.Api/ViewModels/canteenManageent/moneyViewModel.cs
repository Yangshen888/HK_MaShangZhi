using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.canteenManageent
{
    public class moneyViewModel
    {
        public Guid ElectronicUUID { get; set; }
        public string Supplier { get; set; }
        public string Phone { get; set; }
        public string PurchaseTime { get; set; }
        public string CuisineName { get; set; }
        public string Specification { get; set; }
        public string Quantity { get; set; }

        public string ExpirationTime { get; set; }
        public string ProducedTime { get; set; }
        public string RT { get; set; }
        public int? IsDelete { get; set; }
        public string AddTime { get; set; }
        public Guid? SchoolUuid { get; set; }

    }
}
