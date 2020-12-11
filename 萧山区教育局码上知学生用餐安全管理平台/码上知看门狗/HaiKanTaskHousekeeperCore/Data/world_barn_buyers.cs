namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.world_barn_buyers")]
    public partial class world_barn_buyers
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        [Column(TypeName = "uint")]
        public long world_barn_user_id { get; set; }

        [Required]
        [StringLength(255)]
        public string buyer_name { get; set; }

        [Required]
        [StringLength(255)]
        public string buyer_order_shortname { get; set; }

        [Required]
        [StringLength(255)]
        public string buyer_shortname { get; set; }

        public int delivery_time { get; set; }

        public int entry_id { get; set; }
    }
}
