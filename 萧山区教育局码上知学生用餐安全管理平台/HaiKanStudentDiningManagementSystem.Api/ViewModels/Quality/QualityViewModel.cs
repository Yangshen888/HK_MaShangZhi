using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.Quality
{
    public class QualityViewModel
    {
        public Guid? QualityUuid { get; set; }
        public string FlieName { get; set; }
        public string JianJie { get; set; }
        public string Accessory { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public DateTime? AddTime { get; set; }
        public string AddPeople { get; set; }
        public string EffectiveTime { get; set; }
    }
}
