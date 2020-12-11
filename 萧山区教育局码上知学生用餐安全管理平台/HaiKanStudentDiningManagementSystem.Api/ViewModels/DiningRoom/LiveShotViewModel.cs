using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.DiningRoom
{
    public class LiveShotViewModel
    {
        public Guid? LiveShotUuid { get; set; }
        public Guid? CuisineUuid { get; set; }
        public string Accessory { get; set; }
        public string Accessoryvido { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string Datetime { get; set; }
        public string Datetype { get; set; }
    }
}
