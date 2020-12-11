namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.monitors")]
    public partial class monitors
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [StringLength(255)]
        public string full_name { get; set; }

        [Column(TypeName = "uint")]
        public long organization_id { get; set; }

        [StringLength(255)]
        public string title { get; set; }

        [StringLength(255)]
        public string monitor_name { get; set; }

        [Column(TypeName = "uint")]
        public long monitor_branch_id { get; set; }

        [Column(TypeName = "uint")]
        public long monitor_node_id { get; set; }

        [StringLength(255)]
        public string monitor_type { get; set; }

        [StringLength(255)]
        public string hls_url { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? updated_at { get; set; }
    }
}
