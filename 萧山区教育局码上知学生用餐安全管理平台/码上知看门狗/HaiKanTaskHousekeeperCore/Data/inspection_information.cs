namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.inspection_information")]
    public partial class inspection_information
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "uint")]
        public long organization_id { get; set; }

        [Column(TypeName = "uint")]
        public long inspection_id { get; set; }

        [Column(TypeName = "uint")]
        public long user_id { get; set; }

        [StringLength(255)]
        public string user_name { get; set; }

        [StringLength(255)]
        public string card_number { get; set; }

        [StringLength(255)]
        public string duty { get; set; }

        [StringLength(255)]
        public string check_status { get; set; }

        [StringLength(255)]
        public string attendance { get; set; }

        [StringLength(255)]
        public string attendance_time { get; set; }

        [StringLength(255)]
        public string prosess_info { get; set; }

        [StringLength(255)]
        public string img_url { get; set; }

        [StringLength(255)]
        public string note { get; set; }

        [StringLength(255)]
        public string ai_face_img { get; set; }

        [StringLength(255)]
        public string temperature_img { get; set; }

        [StringLength(255)]
        public string hand_img { get; set; }

        [StringLength(255)]
        public string temperature { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? updated_at { get; set; }
    }
}
