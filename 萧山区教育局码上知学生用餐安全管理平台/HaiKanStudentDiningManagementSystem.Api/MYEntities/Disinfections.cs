using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Disinfections
    {
        public ulong Id { get; set; }
        public uint OrganizationId { get; set; }
        public ulong DisinfectionId { get; set; }
        public bool IsClean { get; set; }
        public string Method { get; set; }
        public string Disinfection { get; set; }
        public uint Type { get; set; }
        public string TypeName { get; set; }
        public string Img { get; set; }
        public string ImgUrls { get; set; }
        public uint? CreateUserId { get; set; }
        public uint? DisinfectionUserId { get; set; }
        public string DisinfectionUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public byte Sync { get; set; }
        public DateTime? SyncAt { get; set; }
    }
}
