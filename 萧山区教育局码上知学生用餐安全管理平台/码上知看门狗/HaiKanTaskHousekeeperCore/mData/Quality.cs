namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Quality")]
    public partial class Quality
    {
        [Key]
        public Guid QualityUUID { get; set; }

        [StringLength(255)]
        public string FlieName { get; set; }

        public string JianJie { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        public string Accessory { get; set; }

        [StringLength(100)]
        public string AddTime { get; set; }

        [StringLength(100)]
        public string EffectiveTime { get; set; }
    }
}
