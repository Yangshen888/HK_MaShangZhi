using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.PurchasesInfo
{
    public class DisinfectionViewModel
    {
        public ulong Id { get; set; }
        public ulong dId { get; set; }
        public uint OrganizationId { get; set; }
        public ulong DisinfectionId { get; set; }
        public bool IsClean { get; set; }
        public string Method { get; set; }
        public string Disinfection { get; set; }
        public uint DxType { get; set; }
        public string DxTypeName { get; set; }
        public string Img { get; set; }
        public string ImgUrls { get; set; }
        public uint? CreateUserId { get; set; }
        public uint? DisinfectionUserId { get; set; }
        public string DisinfectionUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Sync { get; set; }
        public DateTime? SyncAt { get; set; }
        public ulong OwnerDisinfectionId { get; set; }
        public ulong DisinfectionRecordId { get; set; }
        public uint MethodId { get; set; }
        public uint Number { get; set; }
        public uint RoomId { get; set; }
        public string RoomName { get; set; }
        public uint ToolId { get; set; }
        public string ToolName { get; set; }
        public uint XdccType { get; set; }
        public string XdccTypeName { get; set; }
    }
}
