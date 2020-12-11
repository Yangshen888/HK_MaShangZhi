using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class SchoolNews
    {
        public long? Id { get; set; }
        public string Headline { get; set; }
        public Guid Uuid { get; set; }
        public string AddTime { get; set; }
        public string Accessory { get; set; }
        public string Tag { get; set; }
        public int Type { get; set; }
        public string Digest { get; set; }
        public Guid? SchoolUuid { get; set; }
    }
}
