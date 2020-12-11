using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class PurchaseRecord
    {
        public int Id { get; set; }
        public Guid PurchaseUuid { get; set; }
        public Guid? IngredientUuid { get; set; }
        public string Supplier { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchaseNum { get; set; }
        public double? HeatEnergy { get; set; }
        public double? Protein { get; set; }
        public double? Fat { get; set; }
        public double? Saccharides { get; set; }
        public double? Va { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string State { get; set; }
        public string SystemUserUuid { get; set; }
        public string Accessory { get; set; }
        public double? Price { get; set; }
        public string Unit { get; set; }

        public virtual Ingredient IngredientUu { get; set; }
        public virtual School SchoolUu { get; set; }
    }
}
