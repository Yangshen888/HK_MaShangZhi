using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Wxmenu
    {
        public Guid WxmenuUuid { get; set; }
        public string MenuName { get; set; }
        public int Id { get; set; }
        public int? IsDelete { get; set; }
    }
}
