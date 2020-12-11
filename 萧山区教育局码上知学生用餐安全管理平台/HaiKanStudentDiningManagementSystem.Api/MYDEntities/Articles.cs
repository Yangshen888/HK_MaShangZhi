using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYDEntities
{
    public partial class Articles
    {
        public ulong Id { get; set; }
        public uint OrgId { get; set; }
        public uint SiteId { get; set; }
        public uint CategoryId { get; set; }
        public uint UserId { get; set; }
        public uint OfficialAccountId { get; set; }
        public string Subject { get; set; }
        public string Cover { get; set; }
        public string OriginalUrl { get; set; }
        public bool Hidden { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}
