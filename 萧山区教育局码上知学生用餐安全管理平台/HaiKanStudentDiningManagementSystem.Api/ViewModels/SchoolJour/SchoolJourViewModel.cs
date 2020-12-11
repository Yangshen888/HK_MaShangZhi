using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.SchoolJour
{
    public class SchoolJourViewModel
    {
        internal readonly object DateTime;

        public Guid? SchoolJourUuid { get; set; }
            public string Headline { get; set; }
            public string Content { get; set; }
            public int? IsDelete { get; set; }
            public Guid? SchoolUuid { get; set; }
            public DateTime? Addtime { get; set; }
            public string AddPeople { get; set; }
        public string Accessory { get; set; }
        public string Tag { get; set; }
        public string Digest { get; set; }
    }
}
