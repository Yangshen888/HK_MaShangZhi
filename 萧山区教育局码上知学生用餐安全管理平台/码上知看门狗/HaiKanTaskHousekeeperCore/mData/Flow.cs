namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Flow")]
    public partial class Flow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid FlowUUID { get; set; }

        [StringLength(255)]
        public string FlowName { get; set; }

        [StringLength(255)]
        public string FlowTime { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        [StringLength(100)]
        public string AddPeople { get; set; }

        public string Accessory { get; set; }

        public virtual School School { get; set; }
    }
}
