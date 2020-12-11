namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [Key]
        public Guid OrderUUID { get; set; }

        [StringLength(255)]
        public string OrderNum { get; set; }

        [StringLength(100)]
        public string OederName { get; set; }

        public decimal? OederMoney { get; set; }

        public decimal? AmountPayable { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Guid? StudentUUID { get; set; }

        public Guid? SchoolUUID { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        public Guid? SystemUserUUID { get; set; }

        public DateTime? OederTime { get; set; }

        public virtual School School { get; set; }

        public virtual Student Student { get; set; }

        public virtual SystemUser SystemUser { get; set; }
    }
}
