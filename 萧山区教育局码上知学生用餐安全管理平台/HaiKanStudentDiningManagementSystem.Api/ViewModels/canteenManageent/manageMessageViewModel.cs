using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.canteenManageent
{
    public class manageMessageViewModel
    {
        public Guid TrainUUID { get; set; }
        public string Theme { get; set; }
        public string TrainTime { get; set; }
        public string Content { get; set; }
        public string TrainName { get; set; }
        public string Speaker { get; set; }
        public int? UserType { get; set; }
        public Guid? SchoolUUID { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }

        public int? IsDelete { get; set; }
        public string OldCard { get; set; }
        public string SystemRoleUuid { get; set; }
    }
}
