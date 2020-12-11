using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class PurchaseInformation
    {
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
