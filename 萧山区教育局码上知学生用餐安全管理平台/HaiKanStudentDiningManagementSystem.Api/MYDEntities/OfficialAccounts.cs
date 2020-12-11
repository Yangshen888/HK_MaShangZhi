using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYDEntities
{
    public partial class OfficialAccounts
    {
        public ulong Id { get; set; }
        public string MpId { get; set; }
        public string Name { get; set; }
        public string Biz { get; set; }
        public uint CategoryId { get; set; }
        public string SchoolName { get; set; }
        public DateTime? LastFetchedAt { get; set; }
    }
}
