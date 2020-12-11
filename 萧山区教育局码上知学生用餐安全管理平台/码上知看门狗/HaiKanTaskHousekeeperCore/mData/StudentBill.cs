namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentBill")]
    public partial class StudentBill
    {
        [Key]
        public Guid StudentBillUUID { get; set; }

        [StringLength(50)]
        public string StudentNum { get; set; }

        [StringLength(50)]
        public string StudentName { get; set; }

        [StringLength(50)]
        public string IDCardNum { get; set; }

        public int? Sex { get; set; }

        public Guid? SchoolUUID { get; set; }

        public int? IsDeleted { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(100)]
        public string ParentsPhone { get; set; }

        [StringLength(100)]
        public string ClassName { get; set; }

        [StringLength(100)]
        public string OrderName { get; set; }

        public decimal? AmountPayable { get; set; }

        public decimal? OrderMoney { get; set; }

        [StringLength(100)]
        public string SerialNumber { get; set; }

        [StringLength(100)]
        public string OrderNum { get; set; }

        public Guid? SystemUserUUID { get; set; }

        public string OederTime { get; set; }

        public virtual School School { get; set; }
    }
}
