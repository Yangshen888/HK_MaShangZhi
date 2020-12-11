using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.WorkStudy
{
    public class WxPostjobsViewModel
    {
        public Guid? PostUuid { get; set; }
        public string  Unit { get; set; }
        public string UnitName{ get; set; }
        public string Site{ get; set; }
        public string Require{ get; set; }
        public Guid? SchoolUuid { get; set; }
        public DateTime? AddTime{ get; set; }
        public int? Num{ get; set; }
        public int? PassNum { get; set; }
    }
}
