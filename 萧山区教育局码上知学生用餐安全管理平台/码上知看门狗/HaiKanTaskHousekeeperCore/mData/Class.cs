namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Class")]
    public partial class Class
    {
        [Key]
        public Guid ClassUUID { get; set; }

        [StringLength(100)]
        public string ClassName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        public virtual School School { get; set; }
    }
}
