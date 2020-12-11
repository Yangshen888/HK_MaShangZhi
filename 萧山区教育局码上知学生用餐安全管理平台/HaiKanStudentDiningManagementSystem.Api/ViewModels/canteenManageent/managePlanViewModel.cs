using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.canteenManageent
{
    public class managePlanViewModel
    {
        public Guid ManageUUID { get; set; }
        public string Headline { get; set; }
        public string Content { get; set; }
        public string AddTime { get; set; }
        public Guid? SchoolUUID { get; set; }
        public string AddPeople { get; set; }

        public int? IsDelete { get; set; }
    }
}
