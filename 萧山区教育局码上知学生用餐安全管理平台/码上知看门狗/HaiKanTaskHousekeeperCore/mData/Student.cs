namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Student")]
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            Order = new HashSet<Order>();
        }

        [Key]
        public Guid StudentUUID { get; set; }

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
        public string OederName { get; set; }

        public decimal AmountPayable { get; set; }

        public decimal? OederMoney { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }

        public virtual School School { get; set; }
    }
}
