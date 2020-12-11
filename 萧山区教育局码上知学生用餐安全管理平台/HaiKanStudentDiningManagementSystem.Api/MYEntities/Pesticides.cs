using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Pesticides
    {
        public ulong Id { get; set; }
        public int Status { get; set; }
        public ulong PesticideId { get; set; }
        public uint OrganizationId { get; set; }
        public uint CreateUserId { get; set; }
        public uint UserOrganizationId { get; set; }
        public string UserName { get; set; }
        public string Inspector { get; set; }
        public string TestPaper { get; set; }
        public string VegetablesName { get; set; }
        public string Vegetables { get; set; }
        public sbyte InspectionOrder { get; set; }
        public string InspectionOrders { get; set; }
        public sbyte InspectionResult { get; set; }
        public string InspectionResults { get; set; }
        public sbyte HandleMeasure { get; set; }
        public string HandleMeasures { get; set; }
        public string Note { get; set; }
        public DateTime? CheckedAt { get; set; }
        public byte Sync { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? SyncAt { get; set; }
    }
}
