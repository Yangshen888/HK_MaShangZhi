using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.SchoolLife
{
    public class SchoolLifeViewModel
    {
        public Guid? SchoollifeUuid { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string Accessory { get; set; }
        public int? IsDelete { get; set; }
        public string State { get; set; }
        public Guid? SchoolUuid { get; set; }
        public DateTime? AddTime { get; set; }
        public string AddPeople { get; set; }
        public string Tag { get; set; }
        public string Digest { get; set; }
    }
}
