namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.disinfections")]
    public partial class disinfections
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "uint")]
        public long organization_id { get; set; }

        [Column(TypeName = "uint")]
        public long organization_name { get; set; }

        public bool is_clean { get; set; }

        [StringLength(255)]
        public string method { get; set; }

        [StringLength(255)]
        public string disinfection { get; set; }

        [Column(TypeName = "uint")]
        public long type { get; set; }

        [StringLength(255)]
        public string type_name { get; set; }

        [StringLength(255)]
        public string img { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string img_urls { get; set; }

        [StringLength(255)]
        public string disinfection_user_name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? created_date { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? updated_at { get; set; }
    }
}
