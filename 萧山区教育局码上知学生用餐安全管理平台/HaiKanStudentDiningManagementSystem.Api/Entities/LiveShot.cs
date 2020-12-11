using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class LiveShot
    {
        public int Id { get; set; }
        public Guid LiveShotUuid { get; set; }
        public Guid? CuisineUuid { get; set; }
        public string Accessory { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string Datetime { get; set; }
        public string Datetype { get; set; }
        public string Accessoryvido { get; set; }

        public virtual Cuisine CuisineUu { get; set; }
        public virtual School SchoolUu { get; set; }
    }
}
