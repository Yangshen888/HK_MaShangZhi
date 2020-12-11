using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class PostJobsAppeal
    {
        public int Id { get; set; }
        public Guid PostJobsAppealUuid { get; set; }
        public Guid? PostUuid { get; set; }
        public string StuName { get; set; }
        public string Sex { get; set; }
        public string Grade { get; set; }
        public string Class { get; set; }
        public int? PoorState { get; set; }
        public string GuardianName { get; set; }
        public string GuardianPhone { get; set; }
        public string AppealPeople { get; set; }
        public DateTime? AddTime { get; set; }
        public int? State { get; set; }

        public virtual Postjobs PostUu { get; set; }
    }
}
