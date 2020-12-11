using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class WorldBarnGoodsCategories
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
    }
}
