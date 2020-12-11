using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Partners
    {
        public ulong Id { get; set; }
        public uint OrganizationId { get; set; }
        public uint PartnerId { get; set; }
        public string FullName { get; set; }
        public string LicenseNo { get; set; }
        public string LicensePeriod { get; set; }
        public string LicenseImg { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string TypeName { get; set; }
        public uint TypeId { get; set; }
        public int? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? AreaId { get; set; }
        public string AreaName { get; set; }
        public int? StreetId { get; set; }
        public string StreetName { get; set; }
        public string DetailAddress { get; set; }
        public string Idcard { get; set; }
        public string MajorGoods { get; set; }
        public string MajorGoodsIds { get; set; }
        public string ProductionLicence { get; set; }
        public string ProvideIdCodeImg { get; set; }
        public string ProvideIdCode { get; set; }
        public string Note { get; set; }
        public string PermitNumberNo { get; set; }
        public string Tel { get; set; }
        public int? ProvideId { get; set; }
        public string ProvideName { get; set; }
        public string ProvidePhone { get; set; }
        public string EnterpriseTypeName { get; set; }
    }
}
