namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.world_barn_goods")]
    public partial class world_barn_goods
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(191)]
        public string goods_name { get; set; }

        [StringLength(191)]
        public string goods_alias { get; set; }

        [StringLength(191)]
        public string goods_code { get; set; }

        [Column(TypeName = "uint")]
        public long big_class_id { get; set; }

        [Column(TypeName = "uint")]
        public long small_class_id { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string goods_desc { get; set; }

        [StringLength(191)]
        public string unit_code { get; set; }

        [StringLength(191)]
        public string spec { get; set; }

        [Column(TypeName = "uint")]
        public long rs_id { get; set; }

        [StringLength(191)]
        public string rs_memo { get; set; }
    }
}
