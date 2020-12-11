using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class WorldBarnBuyers
    {
        public ulong Id { get; set; }
        public uint WorldBarnUserId { get; set; }
        public string BuyerName { get; set; }
        public string BuyerOrderShortname { get; set; }
        public string BuyerShortname { get; set; }
        public int DeliveryTime { get; set; }
        public int EntryId { get; set; }
    }
}
