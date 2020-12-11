using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class Cuisine
    {
        public Cuisine()
        {
            LiveShot = new HashSet<LiveShot>();
            MealFlow = new HashSet<MealFlow>();
        }

        public int Id { get; set; }
        public Guid CuisineUuid { get; set; }
        public string CuisineName { get; set; }
        public string Price { get; set; }
        public string Burdening { get; set; }
        public string Ingredient { get; set; }
        public string Abstract { get; set; }
        public string CuisineType { get; set; }
        public string Accessory { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public int? LikeNum { get; set; }
        public string UpdateTime { get; set; }

        public virtual School SchoolUu { get; set; }
        public virtual ICollection<LiveShot> LiveShot { get; set; }
        public virtual ICollection<MealFlow> MealFlow { get; set; }
    }
}
