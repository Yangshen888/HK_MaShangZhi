using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.Ingredient
{
    public class PurchaseRecordViewModel
    {
        public Guid? PurchaseUuid { get; set; }
        public Guid? IngredientUuid { get; set; }
        public string FoodName { get; set; }
        public string Type { get; set; }
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
    }
}
