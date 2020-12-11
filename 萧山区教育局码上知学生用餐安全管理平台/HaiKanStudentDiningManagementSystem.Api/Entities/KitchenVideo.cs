using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class KitchenVideo
    {
        public int Id { get; set; }
        public Guid VideoUuid { get; set; }
        public string Name { get; set; }
        public string AddTime { get; set; }
        public string Accessory { get; set; }
        public string Type { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
