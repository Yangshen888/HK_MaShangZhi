namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ingredient")]
    public partial class Ingredient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ingredient()
        {
            PurchaseRecord = new HashSet<PurchaseRecord>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid IngredientUUID { get; set; }

        [StringLength(255)]
        public string FoodName { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        public double? HeatEnergy { get; set; }

        public double? Protein { get; set; }

        public double? Fat { get; set; }

        public double? Saccharides { get; set; }

        public double? VA { get; set; }

        public string Accessory { get; set; }

        public Guid? TypeUUID { get; set; }

        public virtual FoodType FoodType { get; set; }

        public virtual School School { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseRecord> PurchaseRecord { get; set; }
    }
}
