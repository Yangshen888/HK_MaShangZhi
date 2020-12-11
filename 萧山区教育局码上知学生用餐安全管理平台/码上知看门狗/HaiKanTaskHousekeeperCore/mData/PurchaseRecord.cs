namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PurchaseRecord")]
    public partial class PurchaseRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid PurchaseUUID { get; set; }

        public Guid? IngredientUUID { get; set; }

        [StringLength(255)]
        public string Supplier { get; set; }

        [StringLength(255)]
        public string PurchaseDate { get; set; }

        [StringLength(255)]
        public string PurchaseNum { get; set; }

        public double? HeatEnergy { get; set; }

        public double? Protein { get; set; }

        public double? Fat { get; set; }

        public double? Saccharides { get; set; }

        public double? VA { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        [StringLength(255)]
        public string State { get; set; }

        [Required]
        public string SystemUserUUID { get; set; }

        public string Accessory { get; set; }

        public double? Price { get; set; }

        [StringLength(100)]
        public string Unit { get; set; }

        public virtual Ingredient Ingredient { get; set; }

        public virtual School School { get; set; }
    }
}
