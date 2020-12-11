namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SurveyQuestionsItems
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid SurveyQuestionsItemsUUID { get; set; }

        [StringLength(255)]
        public string Optionts { get; set; }

        [StringLength(255)]
        public string QuestionStr { get; set; }

        [StringLength(255)]
        public string AddPerson { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AddTime { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SurveyQuestionsUUID { get; set; }

        public Guid? SchoolUUID { get; set; }
    }
}
