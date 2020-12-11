using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Goods
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public uint OrganizationId { get; set; }
        public ulong GoodsId { get; set; }
        public decimal Price { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string Explains { get; set; }
        public int? MealTime { get; set; }
        public string MealTimeName { get; set; }
        public string Img { get; set; }
    }
}
