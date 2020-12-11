namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SurveyQuestions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid SurveyQuestionsUUID { get; set; }

        public int? IsMuti { get; set; }

        public int? QuestionType { get; set; }

        public int? IsDelete { get; set; }

        public DateTime? AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        [StringLength(255)]
        public string QuestionTitle { get; set; }

        public Guid? SchoolUUID { get; set; }

        public Guid? SurveyUUID { get; set; }
    }
}
