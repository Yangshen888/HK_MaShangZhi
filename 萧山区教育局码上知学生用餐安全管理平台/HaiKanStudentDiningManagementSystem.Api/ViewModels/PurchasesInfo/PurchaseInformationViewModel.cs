using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.PurchasesInfo
{
    public class PurchaseInformationViewModel
    {
        public ulong Pid { get; set; }
        public ulong PPurchaseId { get; set; }
        public ulong Id { get; set; }
        public sbyte? Status { get; set; }
        public ulong OwnerPurchaseId { get; set; }
        public uint OrganizationId { get; set; }
        public ulong PurchaseId { get; set; }
        public ulong PurchaseInformationId { get; set; }
        public string FoodName { get; set; }
        public string UnitName { get; set; }
        public string ModelName { get; set; }
        public string Number { get; set; }
        public string Supplier { get; set; }
        public uint SupplierId { get; set; }
        public int? CreateUserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
