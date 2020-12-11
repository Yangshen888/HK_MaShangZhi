namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Postjobs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Postjobs()
        {
            PostJobsAppeal = new HashSet<PostJobsAppeal>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid PostUUID { get; set; }

        [StringLength(255)]
        public string Unit { get; set; }

        [StringLength(255)]
        public string UnitName { get; set; }

        [StringLength(255)]
        public string Require { get; set; }

        [StringLength(255)]
        public string Site { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        public DateTime? AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? Num { get; set; }

        public int? ReleaseState { get; set; }

        public int? FullState { get; set; }

        public virtual School School { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostJobsAppeal> PostJobsAppeal { get; set; }
    }
}
