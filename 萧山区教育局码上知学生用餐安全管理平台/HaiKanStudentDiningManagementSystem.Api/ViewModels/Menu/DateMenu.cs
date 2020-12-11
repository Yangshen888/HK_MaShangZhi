using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.Menu
{
    public class DateMenu
    {
        public string CuisineName { get; set; }
        public string CuisineType { get; set; }
        public string Ingredient { get; set; }
        public string Abstract { get; set; }
        public string Burdening { get; set; }
        public int? LikeNum { get; set; }
        public string Price { get; set; }
        public Guid CuisineUuid { get; set; }
        public string Accessory { get; set; }
        public string Date { get; set; }
    }
}
