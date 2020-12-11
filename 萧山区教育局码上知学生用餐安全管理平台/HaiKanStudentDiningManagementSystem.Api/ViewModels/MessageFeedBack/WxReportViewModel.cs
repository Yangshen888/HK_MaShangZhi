using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack
{
    public class WxReportViewModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public Guid? SchoolUuid { get; set; }
    }
}
