using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Postjobs
    {
        public Postjobs()
        {
            PostJobsAppeal = new HashSet<PostJobsAppeal>();
        }

        public int Id { get; set; }
        public Guid PostUuid { get; set; }
        public string Unit { get; set; }
        public string UnitName { get; set; }
        public string Require { get; set; }
        public string Site { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public DateTime? AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? Num { get; set; }
        public int? ReleaseState { get; set; }
        public int? FullState { get; set; }

        public virtual School SchoolUu { get; set; }
        public virtual ICollection<PostJobsAppeal> PostJobsAppeal { get; set; }
    }
}
