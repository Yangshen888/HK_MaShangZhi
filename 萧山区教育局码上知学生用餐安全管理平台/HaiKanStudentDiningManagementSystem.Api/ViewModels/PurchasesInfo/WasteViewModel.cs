using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.PurchasesInfo
{
    public class WasteViewModel
    {
        public ulong Id { get; set; }
        public ulong WasteId { get; set; }
        public uint OrganizationId { get; set; }
        public int Status { get; set; }
        public string Imgs { get; set; }
        public string FullName { get; set; }
        public DateTime? HandleDate { get; set; }
        public decimal SwillNumber { get; set; }
        public decimal WasteoilNumber { get; set; }
        public decimal OtherWasteNumber { get; set; }
        public ulong? HandlerId { get; set; }
        public string HandlerName { get; set; }
        public string NewHandlerName { get; set; }
        public uint ReceiverCompanyId { get; set; }
        public string Receiver { get; set; }
        public string ReceiverCompanyName { get; set; }
        public string ReceiverIdentityCard { get; set; }
        public string Note { get; set; }
        public string UpdateUser { get; set; }
        public uint CreateUserId { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool? IsSupply { get; set; }
        public byte Sync { get; set; }
        public DateTime? SyncAt { get; set; }
    }
}
