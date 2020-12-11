namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MealFlow")]
    public partial class MealFlow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid MealFlowUUID { get; set; }

        public Guid? CuisineUUID { get; set; }

        [StringLength(255)]
        public string MealType { get; set; }

        public string Buying { get; set; }

        public string Detection { get; set; }

        public string WashVege { get; set; }

        public string Chopper { get; set; }

        public string Cook { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        [StringLength(100)]
        public string BuyingTime { get; set; }

        [StringLength(100)]
        public string DetectionTime { get; set; }

        [StringLength(100)]
        public string ChopperTime { get; set; }

        [StringLength(100)]
        public string WashVegeTime { get; set; }

        [StringLength(100)]
        public string CookTime { get; set; }

        public virtual Cuisine Cuisine { get; set; }

        public virtual School School { get; set; }
    }
}
