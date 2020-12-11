using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.Process
{
    public class ProcessViewModel
    {
        public Guid? MealFlowUuid { get; set; }
        public Guid? CuisineUuid { get; set; }
        public string MealType { get; set; }
        public string Buying { get; set; }
        public string Detection { get; set; }
        public string WashVege { get; set; }
        public string Chopper { get; set; }
        public string Cook { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }
        public string BuyingTime { get; set; }
        public string DetectionTime { get; set; }
        public string ChopperTime { get; set; }
        public string WashVegeTime { get; set; }
        public string CookTime { get; set; }
    }
}
