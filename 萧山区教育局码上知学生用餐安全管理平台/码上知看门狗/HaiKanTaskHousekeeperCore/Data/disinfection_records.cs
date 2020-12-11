namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.disinfection_records")]
    public partial class disinfection_records
    {
        [Column(TypeName = "ubigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        public int disinfection_id { get; set; }

        public int type { get; set; }

        [Required]
        [StringLength(255)]
        public string type_name { get; set; }

        [StringLength(255)]
        public string method { get; set; }

        [Column(TypeName = "uint")]
        public long method_id { get; set; }

        [Column(TypeName = "uint")]
        public long number { get; set; }

        [Column(TypeName = "uint")]
        public long room_id { get; set; }

        [StringLength(255)]
        public string room_name { get; set; }

        [Column(TypeName = "uint")]
        public long tool_id { get; set; }

        [StringLength(255)]
        public string tool_name { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? created_at { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? updated_at { get; set; }
    }
}
