namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.screenshots")]
    public partial class screenshots
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [StringLength(191)]
        public string path { get; set; }

        [Column(TypeName = "uint")]
        public long? organization_id { get; set; }

        [Column(TypeName = "uint")]
        public long monitor_id { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? created_at { get; set; }
    }
}
