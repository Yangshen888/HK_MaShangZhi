using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Survey
    {
        public int Id { get; set; }
        public Guid SurveyUuid { get; set; }
        public string Headline { get; set; }
        public string Type { get; set; }
        public Guid? SchoolUuid { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? ProceedState { get; set; }
        public int? ProductState { get; set; }
        public int? Number { get; set; }
        public DateTime? AddTime { get; set; }
        public int? IsDelete { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
