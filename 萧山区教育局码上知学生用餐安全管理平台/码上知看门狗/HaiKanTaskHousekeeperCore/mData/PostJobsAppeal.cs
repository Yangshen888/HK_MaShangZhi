namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PostJobsAppeal")]
    public partial class PostJobsAppeal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid PostJobsAppealUUID { get; set; }

        public Guid? PostUUID { get; set; }

        [StringLength(50)]
        public string StuName { get; set; }

        [StringLength(2)]
        public string Sex { get; set; }

        [StringLength(50)]
        public string Grade { get; set; }

        [StringLength(50)]
        public string Class { get; set; }

        public int? PoorState { get; set; }

        [StringLength(50)]
        public string GuardianName { get; set; }

        [StringLength(50)]
        public string GuardianPhone { get; set; }

        [StringLength(50)]
        public string AppealPeople { get; set; }

        public DateTime? AddTime { get; set; }

        public int? State { get; set; }

        public virtual Postjobs Postjobs { get; set; }
    }
}
