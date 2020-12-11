namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SurverUser")]
    public partial class SurverUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid SurverUserUUID { get; set; }

        [StringLength(255)]
        public string Age { get; set; }

        [StringLength(255)]
        public string Major { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Year { get; set; }

        [StringLength(255)]
        public string StudentNum { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Remark { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        public Guid? SurverUUID { get; set; }

        public int? IsDelete { get; set; }
    }
}
