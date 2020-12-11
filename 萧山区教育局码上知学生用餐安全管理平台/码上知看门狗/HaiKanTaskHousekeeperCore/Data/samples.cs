namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.samples")]
    public partial class samples
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "uint")]
        public long organization_id { get; set; }

        [StringLength(255)]
        public string food_ids { get; set; }

        [StringLength(255)]
        public string food_name { get; set; }

        [StringLength(255)]
        public string note { get; set; }

        [StringLength(255)]
        public string img { get; set; }

        [Column(TypeName = "uint")]
        public long weight { get; set; }

        [Column(TypeName = "uint")]
        public long hours { get; set; }

        public sbyte del { get; set; }

        public int meal_time { get; set; }

        [StringLength(255)]
        public string meal_time_name { get; set; }

        public int eliminate_id { get; set; }

        [StringLength(255)]
        public string eliminate_name { get; set; }

        public int auditor_id { get; set; }

        [StringLength(255)]
        public string auditor_name { get; set; }

        [StringLength(255)]
        public string sample_name { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? sampled_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? matured_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? eliminated_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? updated_at { get; set; }
    }
}
