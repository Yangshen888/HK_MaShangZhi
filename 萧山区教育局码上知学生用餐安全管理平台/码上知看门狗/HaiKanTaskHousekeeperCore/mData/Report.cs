namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Report")]
    public partial class Report
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid ReportUUID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Content { get; set; }

        public DateTime? ReportDate { get; set; }

        [StringLength(255)]
        public string Result { get; set; }

        public Guid? SystemUserUUID { get; set; }

        [StringLength(255)]
        public string ReplyName { get; set; }

        public DateTime? ReplyTime { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        public int? State { get; set; }

        public virtual School School { get; set; }

        public virtual SystemUser SystemUser { get; set; }
    }
}
