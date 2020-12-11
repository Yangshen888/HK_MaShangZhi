using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.WorkStudy
{
    public class WxPostjobsAppealViewModel
    {
        public Guid? PostUuid { get; set; }
        public string StuName { get; set; }
        public string Sex { get; set; }
        public string Grade { get; set; }
        public string Class { get; set; }
        public int? PoorState { get; set; }
        public string GuardianName { get; set; }
        public string GuardianPhone { get; set; }
        public string AppealPeople { get; set; }
        public int? State { get; set; }
    }
}
