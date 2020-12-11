using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class School
    {
        public School()
        {
            ArticleManage = new HashSet<ArticleManage>();
            Class = new HashSet<Class>();
            Cuisine = new HashSet<Cuisine>();
            ElectronicBill = new HashSet<ElectronicBill>();
            Flow = new HashSet<Flow>();
            Ingredient = new HashSet<Ingredient>();
            KitchenVideo = new HashSet<KitchenVideo>();
            LiveShot = new HashSet<LiveShot>();
            ManagePlan = new HashSet<ManagePlan>();
            MealFlow = new HashSet<MealFlow>();
            MessageBoard = new HashSet<MessageBoard>();
            NweekMenu = new HashSet<NweekMenu>();
            Order = new HashSet<Order>();
            Postjobs = new HashSet<Postjobs>();
            PurchaseRecord = new HashSet<PurchaseRecord>();
            Report = new HashSet<Report>();
            SchoolJour = new HashSet<SchoolJour>();
            SchoolLife = new HashSet<SchoolLife>();
            Student = new HashSet<Student>();
            StudentBill = new HashSet<StudentBill>();
            Survey = new HashSet<Survey>();
            SystemRole = new HashSet<SystemRole>();
            SystemUser = new HashSet<SystemUser>();
            Train = new HashSet<Train>();
        }

        public int Id { get; set; }
        public Guid SchoolUuid { get; set; }
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
        public bool? IsshowReport { get; set; }
        public bool? IsCuiPrices { get; set; }
        public string SchoolType { get; set; }
        public bool IsYard { get; set; }
        public string Qrcode { get; set; }
        public string Accessory { get; set; }
        public string Menus { get; set; }

        public virtual ICollection<ArticleManage> ArticleManage { get; set; }
        public virtual ICollection<Class> Class { get; set; }
        public virtual ICollection<Cuisine> Cuisine { get; set; }
        public virtual ICollection<ElectronicBill> ElectronicBill { get; set; }
        public virtual ICollection<Flow> Flow { get; set; }
        public virtual ICollection<Ingredient> Ingredient { get; set; }
        public virtual ICollection<KitchenVideo> KitchenVideo { get; set; }
        public virtual ICollection<LiveShot> LiveShot { get; set; }
        public virtual ICollection<ManagePlan> ManagePlan { get; set; }
        public virtual ICollection<MealFlow> MealFlow { get; set; }
        public virtual ICollection<MessageBoard> MessageBoard { get; set; }
        public virtual ICollection<NweekMenu> NweekMenu { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<Postjobs> Postjobs { get; set; }
        public virtual ICollection<PurchaseRecord> PurchaseRecord { get; set; }
        public virtual ICollection<Report> Report { get; set; }
        public virtual ICollection<SchoolJour> SchoolJour { get; set; }
        public virtual ICollection<SchoolLife> SchoolLife { get; set; }
        public virtual ICollection<Student> Student { get; set; }
        public virtual ICollection<StudentBill> StudentBill { get; set; }
        public virtual ICollection<Survey> Survey { get; set; }
        public virtual ICollection<SystemRole> SystemRole { get; set; }
        public virtual ICollection<SystemUser> SystemUser { get; set; }
        public virtual ICollection<Train> Train { get; set; }
    }
}
