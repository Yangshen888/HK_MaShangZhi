namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MSZ : DbContext
    {
        public MSZ()
            : base("name=MSZ")
        {
        }

        public virtual DbSet<BillState> BillState { get; set; }
        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<Cuisine> Cuisine { get; set; }
        public virtual DbSet<ElectronicBill> ElectronicBill { get; set; }
        public virtual DbSet<Flow> Flow { get; set; }
        public virtual DbSet<FoodType> FoodType { get; set; }
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<KitchenVideo> KitchenVideo { get; set; }
        public virtual DbSet<LiveShot> LiveShot { get; set; }
        public virtual DbSet<ManagePlan> ManagePlan { get; set; }
        public virtual DbSet<MealFlow> MealFlow { get; set; }
        public virtual DbSet<MessageBoard> MessageBoard { get; set; }
        public virtual DbSet<NWeekMenu> NWeekMenu { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Postjobs> Postjobs { get; set; }
        public virtual DbSet<PostJobsAppeal> PostJobsAppeal { get; set; }
        public virtual DbSet<PurchaseRecord> PurchaseRecord { get; set; }
        public virtual DbSet<Quality> Quality { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<SchoolJour> SchoolJour { get; set; }
        public virtual DbSet<SchoolLife> SchoolLife { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentBill> StudentBill { get; set; }
        public virtual DbSet<SurverUser> SurverUser { get; set; }
        public virtual DbSet<Survey> Survey { get; set; }
        public virtual DbSet<SurveyAnswer> SurveyAnswer { get; set; }
        public virtual DbSet<SurveyQuestions> SurveyQuestions { get; set; }
        public virtual DbSet<SurveyQuestionsItems> SurveyQuestionsItems { get; set; }
        public virtual DbSet<SystemIcon> SystemIcon { get; set; }
        public virtual DbSet<SystemLog> SystemLog { get; set; }
        public virtual DbSet<SystemMenu> SystemMenu { get; set; }
        public virtual DbSet<SystemPermission> SystemPermission { get; set; }
        public virtual DbSet<SystemRole> SystemRole { get; set; }
        public virtual DbSet<SystemRolePermissionMapping> SystemRolePermissionMapping { get; set; }
        public virtual DbSet<SystemUser> SystemUser { get; set; }
        public virtual DbSet<SystemUserRoleMapping> SystemUserRoleMapping { get; set; }
        public virtual DbSet<Train> Train { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillState>()
                .Property(e => e.Appid)
                .IsUnicode(false);

            modelBuilder.Entity<BillState>()
                .Property(e => e.Mch_id)
                .IsUnicode(false);

            modelBuilder.Entity<BillState>()
                .Property(e => e.Money)
                .HasPrecision(14, 2);

            modelBuilder.Entity<Class>()
                .Property(e => e.ClassName)
                .IsUnicode(false);

            modelBuilder.Entity<MealFlow>()
                .Property(e => e.BuyingTime)
                .IsUnicode(false);

            modelBuilder.Entity<MealFlow>()
                .Property(e => e.DetectionTime)
                .IsUnicode(false);

            modelBuilder.Entity<MealFlow>()
                .Property(e => e.ChopperTime)
                .IsUnicode(false);

            modelBuilder.Entity<MealFlow>()
                .Property(e => e.WashVegeTime)
                .IsUnicode(false);

            modelBuilder.Entity<MealFlow>()
                .Property(e => e.CookTime)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Quality>()
                .Property(e => e.FlieName)
                .IsUnicode(false);

            modelBuilder.Entity<Quality>()
                .Property(e => e.EffectiveTime)
                .IsUnicode(false);

            modelBuilder.Entity<School>()
                .Property(e => e.SchoolImg)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.StudentName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.IDCardNum)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.ClassName)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.OederName)
                .IsUnicode(false);

            modelBuilder.Entity<StudentBill>()
                .Property(e => e.StudentName)
                .IsUnicode(false);

            modelBuilder.Entity<StudentBill>()
                .Property(e => e.IDCardNum)
                .IsUnicode(false);

            modelBuilder.Entity<StudentBill>()
                .Property(e => e.ClassName)
                .IsUnicode(false);

            modelBuilder.Entity<StudentBill>()
                .Property(e => e.OrderName)
                .IsUnicode(false);

            modelBuilder.Entity<StudentBill>()
                .Property(e => e.SerialNumber)
                .IsUnicode(false);

            modelBuilder.Entity<StudentBill>()
                .Property(e => e.OrderNum)
                .IsUnicode(false);

            modelBuilder.Entity<SurveyAnswer>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<SurveyQuestions>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<SurveyQuestions>()
                .Property(e => e.QuestionTitle)
                .IsUnicode(false);

            modelBuilder.Entity<SurveyQuestionsItems>()
                .Property(e => e.Optionts)
                .IsUnicode(false);

            modelBuilder.Entity<SurveyQuestionsItems>()
                .Property(e => e.QuestionStr)
                .IsUnicode(false);

            modelBuilder.Entity<SurveyQuestionsItems>()
                .Property(e => e.AddPerson)
                .IsUnicode(false);

            modelBuilder.Entity<SurveyQuestionsItems>()
                .Property(e => e.AddTime)
                .HasPrecision(1);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.OperationContent)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.IPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.AddPeople)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.ManageDepartment)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.ZaiGang)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Sex)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.InTime)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Types)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.SystemRoleUUid)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Wechat)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Topic)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Train)
                .IsUnicode(false);

            modelBuilder.Entity<SystemUser>()
                .Property(e => e.Main)
                .IsUnicode(false);
        }
    }
}
