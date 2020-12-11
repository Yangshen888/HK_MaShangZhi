namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.purchases")]
    public partial class purchases
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        public int organization_id { get; set; }

        [StringLength(255)]
        public string organization_name { get; set; }

        [StringLength(255)]
        public string register { get; set; }

        [StringLength(255)]
        public string register_date { get; set; }

        [StringLength(255)]
        public string purchase_user { get; set; }

        [StringLength(255)]
        public string purchase_date { get; set; }

        [StringLength(255)]
        public string type { get; set; }

        [StringLength(255)]
        public string supplier { get; set; }

        [StringLength(255)]
        public string status { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string ticket_imgs { get; set; }

        [StringLength(255)]
        public string note { get; set; }
    }
}
