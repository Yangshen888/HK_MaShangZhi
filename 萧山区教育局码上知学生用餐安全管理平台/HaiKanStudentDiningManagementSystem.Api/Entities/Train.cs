using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Train
    {
        public int Id { get; set; }
        public Guid TrainUuid { get; set; }
        public string Theme { get; set; }
        public string TrainTime { get; set; }
        public string TrainName { get; set; }
        public string Speaker { get; set; }
        public string Content { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public string IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string Address { get; set; }
        public string TrainLession { get; set; }
        public string Mnum { get; set; }
        public string Anum { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
