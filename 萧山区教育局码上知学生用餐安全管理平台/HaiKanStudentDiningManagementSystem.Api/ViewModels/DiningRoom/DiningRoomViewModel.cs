using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.DiningRoom
{
    public class DiningRoomViewModel
    {
        public Guid? VideoUuid { get; set; }
        public string Name { get; set; }
        public string AddTime { get; set; }
        public string Accessory { get; set; }
        public string Type { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
    }
}
