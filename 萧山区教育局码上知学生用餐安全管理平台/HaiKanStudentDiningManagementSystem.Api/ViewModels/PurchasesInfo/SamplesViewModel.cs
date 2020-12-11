using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.PurchasesInfo
{
    public class SamplesViewModel
    {
        public ulong Id { get; set; }
        public ulong SampleId { get; set; }
        public uint OrganizationId { get; set; }
        public string FoodIds { get; set; }
        public string FoodName { get; set; }
        public string Note { get; set; }
        public string Img { get; set; }
        public uint Weight { get; set; }
        public uint Hours { get; set; }
        public sbyte Del { get; set; }
        public uint? CreateUserId { get; set; }
        public int MealTime { get; set; }
        public string MealTimeName { get; set; }
        public int EliminateId { get; set; }
        public string EliminateName { get; set; }
        public int AuditorId { get; set; }
        public string AuditorName { get; set; }
        public string SampleName { get; set; }
        public DateTime? SampledAt { get; set; }
        public int? SampleUserId { get; set; }
        public DateTime? MaturedAt { get; set; }
        public DateTime? EliminatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public byte Sync { get; set; }
        public DateTime? SyncAt { get; set; }
    }
}
