using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class WorldBarnOrders
    {
        public string OrderId { get; set; }
        public string BuyerCode { get; set; }
        public uint BuyerId { get; set; }
        public string BuyerIds { get; set; }
        public string BuyerInfo { get; set; }
        public string BuyerName { get; set; }
        public string BuyerNames { get; set; }
        public string ChineseTotalAmount { get; set; }
        public uint CompanyId { get; set; }
        public string ContactPerson { get; set; }
        public DateTime? CreateDate { get; set; }
        public uint CurCustomerId { get; set; }
        public string CurCustomerName { get; set; }
        public sbyte? CurOrderStatus { get; set; }
        public DateTime? CustomerDeliveryDate { get; set; }
        public uint CustomerDeliveryTime { get; set; }
        public string CustomerDeliveryTimeValue { get; set; }
        public uint? DayOrderNum { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public uint EntryId { get; set; }
        public string GoodsClassList { get; set; }
        public int? GoodsNum { get; set; }
        public bool IsEvaluation { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public uint LastModifyUserId { get; set; }
        public string LastModifyUsername { get; set; }
        public uint LineId { get; set; }
        public string LineName { get; set; }
        public string Mobile { get; set; }
        public uint ModifyBelongType { get; set; }
        public decimal OldRealAmount { get; set; }
        public string OrderCancelReason { get; set; }
        public string OrderDetail { get; set; }
        public DateTime? OrderPrintDate { get; set; }
        public sbyte? PaymentStatus { get; set; }
        public string PrintCode { get; set; }
        public int PrintId { get; set; }
        public string QrCode { get; set; }
        public decimal RealAmount { get; set; }
        public string RelOrderId { get; set; }
        public string RelOrderType { get; set; }
        public string Remark { get; set; }
        public string ServiceLicenseNo { get; set; }
        public int Status { get; set; }
        public int Source { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
