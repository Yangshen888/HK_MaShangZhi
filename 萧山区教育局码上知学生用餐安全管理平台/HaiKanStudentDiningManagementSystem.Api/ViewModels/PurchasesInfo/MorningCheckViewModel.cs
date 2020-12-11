using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.PurchasesInfo
{
    public class MorningCheckViewModel
    {
        public ulong? Id { get; set; }
        public uint OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int ShouldCount { get; set; }
        public int ActualCount { get; set; }
        public int UnqualifiedCount { get; set; }
        public uint CreatedUser { get; set; }
        public string Inspector { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsSupply { get; set; }
        public sbyte Type { get; set; }
        public bool? IsDifferent { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TeamGroupId { get; set; }
        public string TeamGroupName { get; set; }
    }
}
