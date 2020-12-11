namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LiveShot")]
    public partial class LiveShot
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid LiveShotUUID { get; set; }

        public Guid? CuisineUUID { get; set; }

        public string Accessory { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        [StringLength(50)]
        public string Datetime { get; set; }

        [StringLength(50)]
        public string Datetype { get; set; }

        public string Accessoryvido { get; set; }

        public virtual Cuisine Cuisine { get; set; }

        public virtual School School { get; set; }
    }
}
