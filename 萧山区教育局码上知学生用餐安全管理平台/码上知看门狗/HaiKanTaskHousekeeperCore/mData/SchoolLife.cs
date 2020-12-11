namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SchoolLife")]
    public partial class SchoolLife
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid SchoollifeUUID { get; set; }

        [StringLength(255)]
        public string Headline { get; set; }

        public string Content { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(55)]
        public string State { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        public string Accessory { get; set; }

        [StringLength(100)]
        public string Tag { get; set; }

        [StringLength(255)]
        public string Digest { get; set; }

        public virtual School School { get; set; }
    }
}
