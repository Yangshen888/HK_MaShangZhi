using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            PurchaseRecord = new HashSet<PurchaseRecord>();
        }

        public int Id { get; set; }
        public Guid IngredientUuid { get; set; }
        public string FoodName { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public double? HeatEnergy { get; set; }
        public double? Protein { get; set; }
        public double? Fat { get; set; }
        public double? Saccharides { get; set; }
        public double? Va { get; set; }
        public string Accessory { get; set; }
        public Guid? TypeUuid { get; set; }

        public virtual School SchoolUu { get; set; }
        public virtual FoodType TypeUu { get; set; }
        public virtual ICollection<PurchaseRecord> PurchaseRecord { get; set; }
    }
}
