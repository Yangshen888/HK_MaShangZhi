namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("iot_tool_msa.world_barn_orders")]
    public partial class world_barn_orders
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(191)]
        public string order_id { get; set; }

        [StringLength(191)]
        public string buyer_code { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long buyer_id { get; set; }

        [StringLength(191)]
        public string buyer_ids { get; set; }

        [StringLength(191)]
        public string buyer_info { get; set; }

        [StringLength(191)]
        public string buyer_name { get; set; }

        [StringLength(191)]
        public string buyer_names { get; set; }

        [StringLength(191)]
        public string chinese_total_amount { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long company_id { get; set; }

        [StringLength(191)]
        public string contact_person { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? create_date { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long cur_customer_id { get; set; }

        [StringLength(191)]
        public string cur_customer_name { get; set; }

        [Key]
        [Column(Order = 4)]
        public sbyte cur_order_status { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? customer_delivery_date { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long customer_delivery_time { get; set; }

        [StringLength(191)]
        public string customer_delivery_time_value { get; set; }

        [Column(TypeName = "uint")]
        public long? day_order_num { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? delivery_date { get; set; }

        [Key]
        [Column(Order = 6, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long entry_id { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string goods_class_list { get; set; }

        public int? goods_num { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool is_evaluation { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? last_modify_date { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long last_modify_user_id { get; set; }

        [StringLength(191)]
        public string last_modify_username { get; set; }

        [Key]
        [Column(Order = 9, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long line_id { get; set; }

        [StringLength(191)]
        public string line_name { get; set; }

        [StringLength(191)]
        public string mobile { get; set; }

        [Key]
        [Column(Order = 10, TypeName = "uint")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long modify_belong_type { get; set; }

        [Key]
        [Column(Order = 11)]
        public decimal old_real_amount { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(191)]
        public string order_cancel_reason { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string order_detail { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? order_print_date { get; set; }

        [Key]
        [Column(Order = 13)]
        public sbyte payment_status { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(191)]
        public string print_code { get; set; }

        [Key]
        [Column(Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int print_id { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(191)]
        public string qr_code { get; set; }

        [Key]
        [Column(Order = 17)]
        [StringLength(191)]
        public string rel_order_id { get; set; }

        [StringLength(191)]
        public string rel_order_type { get; set; }

        [Key]
        [Column(Order = 18)]
        [StringLength(191)]
        public string remark { get; set; }

        [StringLength(191)]
        public string service_license_no { get; set; }

        [Key]
        [Column(Order = 19)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int status { get; set; }

        [Key]
        [Column(Order = 20)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int source { get; set; }

        [Key]
        [Column(Order = 21)]
        public decimal total_amount { get; set; }
    }
}
