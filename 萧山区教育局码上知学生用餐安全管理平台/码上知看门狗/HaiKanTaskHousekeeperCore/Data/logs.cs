namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.logs")]
    public partial class logs
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(191)]
        public string object_type { get; set; }

        [Column(TypeName = "ubigint")]
        public decimal object_id { get; set; }

        [Column(TypeName = "uint")]
        public long organization_id { get; set; }

        [StringLength(191)]
        public string name { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? created_at { get; set; }
    }
}
