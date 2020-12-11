namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.world_barn_order_items")]
    public partial class world_barn_order_items
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Required]
        [StringLength(191)]
        public string order_id { get; set; }

        [Required]
        [StringLength(191)]
        public string goods_name { get; set; }

        [Column(TypeName = "uint")]
        public long goods_id { get; set; }

        [Column(TypeName = "uint")]
        public long order_type { get; set; }
        [Column(TypeName = "decimal")]
        public decimal demanded_quantity { get; set; }

        [Column(TypeName = "decimal")]
        public decimal real_quantity { get; set; }

        [Column(TypeName = "decimal")]
        public decimal real_price { get; set; }

        [Column(TypeName = "decimal")]
        public decimal unit_price { get; set; }

        [Column(TypeName = "decimal")]
        public decimal subtotal { get; set; }

        [Required]
        [StringLength(191)]
        public string unit { get; set; }

        [StringLength(191)]
        public string note { get; set; }

        [StringLength(191)]
        public string model { get; set; }

        [StringLength(191)]
        public string supplier_name { get; set; }

        [Column(TypeName = "uint")]
        public long? supplier_id { get; set; }

        [StringLength(191)]
        public string compare_status { get; set; }

        [StringLength(191)]
        public string last_order_price { get; set; }

        [Column(TypeName = "date")]
        public DateTime? last_order_price_date { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? confirm_goods_date { get; set; }
    }
}
