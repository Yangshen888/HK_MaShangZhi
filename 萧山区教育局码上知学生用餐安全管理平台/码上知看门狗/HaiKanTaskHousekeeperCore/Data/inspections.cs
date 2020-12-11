namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.inspections")]
    public partial class inspections
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "uint")]
        public long organization_id { get; set; }

        [StringLength(255)]
        public string organization_name { get; set; }

        [StringLength(255)]
        public string should_count { get; set; }

        [StringLength(255)]
        public string actual_count { get; set; }

        [StringLength(255)]
        public string unqualified_count { get; set; }

        [Column(TypeName = "uint")]
        public long created_user { get; set; }

        [StringLength(255)]
        public string inspector { get; set; }

        [Column(TypeName = "date")]
        public DateTime? created_date { get; set; }

        public bool is_supply { get; set; }

        public sbyte type { get; set; }

        public bool is_different { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? updated_at { get; set; }
    }
}
