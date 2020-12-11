using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class WorldBarnOrderItems
    {
        public ulong Id { get; set; }
        public string OrderId { get; set; }
        public string GoodsName { get; set; }
        public uint GoodsId { get; set; }
        public uint OrderType { get; set; }
        public decimal DemandedQuantity { get; set; }
        public decimal RealQuantity { get; set; }
        public decimal RealPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public string Unit { get; set; }
        public string Note { get; set; }
        public string Model { get; set; }
        public string SupplierName { get; set; }
        public uint? SupplierId { get; set; }
        public string CompareStatus { get; set; }
        public string LastOrderPrice { get; set; }
        public DateTime? LastOrderPriceDate { get; set; }
        public DateTime? ConfirmGoodsDate { get; set; }
    }
}
