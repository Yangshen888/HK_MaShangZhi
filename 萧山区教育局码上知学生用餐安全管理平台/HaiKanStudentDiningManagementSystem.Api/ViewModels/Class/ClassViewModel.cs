using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.Class
{
    public class ClassViewModel
    {
        public Guid? ClassUuid { get; set; }
        public string ClassName { get; set; }
        public int Id { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
    }
}
