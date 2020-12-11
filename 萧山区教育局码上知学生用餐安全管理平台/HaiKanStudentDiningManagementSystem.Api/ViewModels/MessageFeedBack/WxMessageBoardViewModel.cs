using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack
{
    public class WxMessageBoardViewModel
    {
        public Guid? MessageUuid { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string AddPeople { get; set; }
    }
}
