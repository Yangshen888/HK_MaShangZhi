using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class WorldBarnGoods
    {
        public ulong Id { get; set; }
        public string GoodsName { get; set; }
        public string GoodsAlias { get; set; }
        public string GoodsCode { get; set; }
        public uint BigClassId { get; set; }
        public uint SmallClassId { get; set; }
        public string GoodsDesc { get; set; }
        public string UnitCode { get; set; }
        public string Spec { get; set; }
        public uint RsId { get; set; }
        public string RsMemo { get; set; }
    }
}
