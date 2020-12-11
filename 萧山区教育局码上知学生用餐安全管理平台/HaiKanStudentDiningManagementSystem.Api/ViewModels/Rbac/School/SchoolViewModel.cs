using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.Rbac.School
{
    public class SchoolViewModel
    {
        public Guid? SchoolUuid { get; set; }
        public string SchoolName { get; set; }
        public string AddTime { get; set; }
        public int? IsDelete { get; set; }
        public string AddPeople { get; set; }
        public string PurchasingStandard { get; set; }
        public string Link { get; set; }
        public string SchoolImg { get; set; }
        public bool Isptjob { get; set; }
        public bool Isrecharge { get; set; }
        public bool Isreservation { get; set; }
        public string Yard { get; set; }
        public string Secretkey { get; set; }
        public bool IsshowReport { get; set; }
        public bool IsCuiPrices { get; set; }
        public string SchoolType { get; set; }
        public bool IsYard { get; set; }
        public string QRcode { get; set; }
        public string Accessory { get; set; }
        public string Menus { get; set; }
    }
}
