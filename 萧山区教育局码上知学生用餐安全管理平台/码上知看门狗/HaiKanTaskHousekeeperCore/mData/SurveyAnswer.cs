namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SurveyAnswer")]
    public partial class SurveyAnswer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid SurveyAnswerUUID { get; set; }

        public Guid? SurveyUUID { get; set; }

        public string AnswerStr { get; set; }

        public int? IsDelete { get; set; }

        public DateTime? AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public Guid? SchoolUUID { get; set; }
    }
}
