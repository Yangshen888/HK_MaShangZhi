using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Users
    {
        public ulong Id { get; set; }
        public uint OrganizationId { get; set; }
        public uint UserId { get; set; }
        public string Name { get; set; }
        public int? SysUserId { get; set; }
        public int? GroupId { get; set; }
        public int? Nbunber { get; set; }
        public int? ParentId { get; set; }
        public string SuperviseOrganization { get; set; }
        public int? RegionId { get; set; }
        public string Code { get; set; }
        public string Phone { get; set; }
        public string Idcard { get; set; }
        public string IdcardImg { get; set; }
        public string Birthday { get; set; }
        public sbyte Sex { get; set; }
        public int? DutyId { get; set; }
        public string DutyName { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? Seniority { get; set; }
        public DateTime? HealthBeginDate { get; set; }
        public DateTime? HealthEndDate { get; set; }
        public string HealthImg { get; set; }
        public sbyte? HealthStatus { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? ExaminationAt { get; set; }
        public DateTime? TrainAt { get; set; }
        public string OrganizationType { get; set; }
        public string Note { get; set; }
        public int Rank { get; set; }
        public sbyte Status { get; set; }
        public sbyte IsFinal { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreateUserId { get; set; }
        public int? TeamGroupId { get; set; }
        public string TeamGroupName { get; set; }
        public string HealthNum { get; set; }
        public int? HealthTypeId { get; set; }
        public string HealthFrom { get; set; }
        public sbyte? WarnStatus { get; set; }
        public DateTime? WarnAt { get; set; }
        public string ChargeArea { get; set; }
        public int? OverdueNum { get; set; }
        public string Icons { get; set; }
        public string LoginName { get; set; }
        public sbyte Activation { get; set; }
        public sbyte HealthOverdue { get; set; }
        public int? EducationId { get; set; }
        public sbyte? Work { get; set; }
        public string Mzt { get; set; }
        public string Mdz { get; set; }
        public DateTime? HealthCodeAt { get; set; }
        public int? ElecHealthCertificateCount { get; set; }
        public string HashValue { get; set; }
        public bool Sync { get; set; }
        public DateTime? SyncAt { get; set; }
        public string OcrResult { get; set; }
    }
}
