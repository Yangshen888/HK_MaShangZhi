namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Train")]
    public partial class Train
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid TrainUUID { get; set; }

        [StringLength(255)]
        public string Theme { get; set; }

        [StringLength(255)]
        public string TrainTime { get; set; }

        [StringLength(255)]
        public string TrainName { get; set; }

        [StringLength(255)]
        public string Speaker { get; set; }

        public string Content { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        [StringLength(255)]
        public string IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public string TrainLession { get; set; }

        [StringLength(255)]
        public string Mnum { get; set; }

        [StringLength(255)]
        public string Anum { get; set; }

        public virtual School School { get; set; }
    }
}
