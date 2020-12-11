namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Survey")]
    public partial class Survey
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid SurveyUUID { get; set; }

        [StringLength(255)]
        public string Headline { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        public Guid? SchoolUUID { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? ProceedState { get; set; }

        public int? ProductState { get; set; }

        public int? Number { get; set; }

        public DateTime? AddTime { get; set; }

        public int? IsDelete { get; set; }

        public virtual School School { get; set; }
    }
}
