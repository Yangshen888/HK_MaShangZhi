using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class ArticleManage
    {
        public int Id { get; set; }
        public Guid ArticleUuid { get; set; }
        public Guid SchoolUuid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string AddTime { get; set; }
        public int IsDelete { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
