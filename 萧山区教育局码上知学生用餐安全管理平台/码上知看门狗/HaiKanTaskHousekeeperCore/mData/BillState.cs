namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BillState")]
    public partial class BillState
    {
        [Key]
        public Guid BillUUID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required]
        [StringLength(255)]
        public string BillNum { get; set; }

        [StringLength(100)]
        public string Appid { get; set; }

        [Required]
        [StringLength(100)]
        public string Mch_id { get; set; }

        public int State { get; set; }

        public decimal Money { get; set; }

        [Required]
        [StringLength(255)]
        public string Key { get; set; }

        public Guid? SBillUUID { get; set; }
    }
}
