using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack
{
    public class MessageBoardViewModel
    {
        public Guid? MessageUuid { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public DateTime? MessageDate { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string Response { get; set; }
        public string ResponseType { get; set; }
        public DateTime? ResponseDate { get; set; }
        public int? State { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string AddPeople { get; set; }
    }
}
