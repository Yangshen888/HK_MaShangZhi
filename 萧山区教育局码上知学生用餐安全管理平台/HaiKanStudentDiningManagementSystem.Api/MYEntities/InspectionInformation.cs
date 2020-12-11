using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class InspectionInformation
    {
        public ulong Id { get; set; }
        public uint OrganizationId { get; set; }
        public ulong OwnerInspectionId { get; set; }
        public ulong InspectionId { get; set; }
        public ulong InspectionInformationId { get; set; }
        public uint? UserId { get; set; }
        public int? CreateUserId { get; set; }
        public string UserName { get; set; }
        public string CardNumber { get; set; }
        public string Duty { get; set; }
        public string CheckStatus { get; set; }
        public string Attendance { get; set; }
        public string AttendanceTime { get; set; }
        public string ProsessInfo { get; set; }
        public string ImgUrl { get; set; }
        public string Note { get; set; }
        public string AiFaceImg { get; set; }
        public string TemperatureImg { get; set; }
        public string HandImg { get; set; }
        public string Temperature { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string PeopleStatus { get; set; }
        public string PeopleStatusName { get; set; }
    }
}
