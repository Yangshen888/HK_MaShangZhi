using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Purchases
    {
        public ulong Id { get; set; }
        public ulong PurchaseId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Register { get; set; }
        public int? RegisterUserId { get; set; }
        public string RegisterDate { get; set; }
        public string PurchaseUser { get; set; }
        public string PurchaseDate { get; set; }
        public string Type { get; set; }
        public string Types { get; set; }
        public string Supplier { get; set; }
        public string Status { get; set; }
        public string TicketImgs { get; set; }
        public string Note { get; set; }
        public byte Sync { get; set; }
        public DateTime? SyncAt { get; set; }
    }
}
