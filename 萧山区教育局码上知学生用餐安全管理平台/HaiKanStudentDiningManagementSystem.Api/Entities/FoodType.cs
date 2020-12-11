using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class FoodType
    {
        public FoodType()
        {
            Ingredient = new HashSet<Ingredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Guid TypeUuid { get; set; }

        public virtual ICollection<Ingredient> Ingredient { get; set; }
    }
}
