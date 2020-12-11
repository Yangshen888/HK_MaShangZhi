namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.pesticides")]
    public partial class pesticides
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        public int status { get; set; }

        [Column(TypeName = "uint")]
        public long organization_id { get; set; }

        [Column(TypeName = "uint")]
        public long create_user_id { get; set; }

        [Column(TypeName = "uint")]
        public long user_organization_id { get; set; }

        [Required]
        [StringLength(255)]
        public string user_name { get; set; }

        [Required]
        [StringLength(255)]
        public string inspector { get; set; }

        [StringLength(255)]
        public string test_paper { get; set; }

        [StringLength(255)]
        public string vegetables_name { get; set; }

        [StringLength(255)]
        public string vegetables { get; set; }

        public sbyte inspection_order { get; set; }

        [StringLength(255)]
        public string inspection_orders { get; set; }

        public sbyte inspection_result { get; set; }

        [StringLength(255)]
        public string inspection_results { get; set; }

        public sbyte handle_measure { get; set; }

        [StringLength(255)]
        public string handle_measures { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string note { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? checked_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? updated_at { get; set; }
    }
}
