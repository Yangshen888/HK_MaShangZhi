namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ElectronicBill")]
    public partial class ElectronicBill
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid ElectronicUUID { get; set; }

        [StringLength(255)]
        public string Supplier { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string PurchaseTime { get; set; }

        [StringLength(255)]
        public string CuisineName { get; set; }

        [StringLength(255)]
        public string Specification { get; set; }

        [StringLength(255)]
        public string Quantity { get; set; }

        [StringLength(255)]
        public string ProducedTime { get; set; }

        [StringLength(255)]
        public string ExpirationTime { get; set; }

        [StringLength(255)]
        public string RT { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        public virtual School School { get; set; }
    }
}
