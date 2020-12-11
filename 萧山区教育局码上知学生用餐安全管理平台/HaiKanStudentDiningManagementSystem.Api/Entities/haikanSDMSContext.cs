using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class haikanSDMSContext : DbContext
    {
        public haikanSDMSContext()
        {
        }

        public haikanSDMSContext(DbContextOptions<haikanSDMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArticleManage> ArticleManage { get; set; }
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
        public virtual DbSet<NweekMenu> NweekMenu { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<PostJobsAppeal> PostJobsAppeal { get; set; }
        public virtual DbSet<Postjobs> Postjobs { get; set; }
        public virtual DbSet<PurchaseRecord> PurchaseRecord { get; set; }
        public virtual DbSet<Quality> Quality { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<SchoolJour> SchoolJour { get; set; }
        public virtual DbSet<SchoolLife> SchoolLife { get; set; }
        public virtual DbSet<SchoolNews> SchoolNews { get; set; }
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
        public virtual DbSet<ViewMenu> ViewMenu { get; set; }
        public virtual DbSet<ViewSystemPermissionWithMenu> ViewSystemPermissionWithMenu { get; set; }
        public virtual DbSet<ViewSystemPermissionWithMenu2> ViewSystemPermissionWithMenu2 { get; set; }
        public virtual DbSet<Wxmenu> Wxmenu { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlServer("Data Source=192.168.0.214.;Initial Catalog=mashangzhi;Persist Security Info=True;User ID=mashangzhi;Password=haikan051030");
                optionsBuilder.UseSqlServer("Data Source=172_17_0_4\\JIULONGCLOUD;Initial Catalog=mashangzhi;Persist Security Info=True;User ID=mashangzhi;Password=haikan051030");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleManage>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .IsClustered(false);

                entity.HasComment("文章管理");

                entity.HasIndex(e => e.ArticleUuid)
                    .HasName("IX_ArticleManage")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddTime).HasMaxLength(255);

                entity.Property(e => e.ArticleUuid).HasColumnName("ArticleUUID");

                entity.Property(e => e.Content).HasComment("内容");

                entity.Property(e => e.SchoolUuid).HasColumnName("SchoolUUID");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasComment("标题");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.ArticleManage)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticleManage_School");
            });

            modelBuilder.Entity<BillState>(entity =>
            {
                entity.HasKey(e => e.BillUuid)
                    .IsClustered(false);

                entity.HasComment("订单状态");

                entity.HasIndex(e => e.Id)
                    .HasName("BillState_ID_IDX")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.BillUuid)
                    .HasColumnName("BillUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Appid)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BillNum)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.MchId)
                    .IsRequired()
                    .HasColumnName("Mch_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Money).HasColumnType("decimal(14, 2)");

                entity.Property(e => e.SbillUuid).HasColumnName("SBillUUID");

                entity.Property(e => e.State).HasComment("0查询中  1支付成功");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.ClassUuid)
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("IX_Class")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.ClassUuid)
                    .HasColumnName("ClassUUID")
                    .HasComment("班级uuid")
                    .ValueGeneratedNever();

                entity.Property(e => e.ClassName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("班级名称");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否删除   0否   1是");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校id");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.Class)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Class_School");
            });

            modelBuilder.Entity<Cuisine>(entity =>
            {
                entity.HasKey(e => e.CuisineUuid)
                    .IsClustered(false);

                entity.HasComment("菜品表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_Cuisine")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.CuisineUuid)
                    .HasColumnName("CuisineUUID")
                    .HasComment("菜品UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Abstract).HasComment("简介");

                entity.Property(e => e.Accessory).HasComment("附件");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Burdening).HasComment("配料");

                entity.Property(e => e.CuisineName)
                    .HasMaxLength(255)
                    .HasComment("菜品名称");

                entity.Property(e => e.CuisineType)
                    .HasMaxLength(255)
                    .HasComment("菜品类型");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Ingredient).HasComment("主料");

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.LikeNum).HasComment("点赞数");

                entity.Property(e => e.Price)
                    .HasMaxLength(255)
                    .HasComment("价格");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.UpdateTime).HasMaxLength(255);

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.Cuisine)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Cuisine_School");
            });

            modelBuilder.Entity<ElectronicBill>(entity =>
            {
                entity.HasKey(e => e.ElectronicUuid)
                    .IsClustered(false);

                entity.HasComment("电子台账");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_ElectronicBill")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.ElectronicUuid)
                    .HasColumnName("ElectronicUUID")
                    .HasComment("电子台账UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.CuisineName)
                    .HasMaxLength(255)
                    .HasComment("食品名称");

                entity.Property(e => e.ExpirationTime)
                    .HasMaxLength(255)
                    .HasComment("保质期");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .HasComment("供货商联系方式");

                entity.Property(e => e.ProducedTime)
                    .HasMaxLength(255)
                    .HasComment("生产日期");

                entity.Property(e => e.PurchaseTime)
                    .HasMaxLength(255)
                    .HasComment("进货时间");

                entity.Property(e => e.Quantity)
                    .HasMaxLength(255)
                    .HasComment("数量");

                entity.Property(e => e.Rt)
                    .HasColumnName("RT")
                    .HasMaxLength(255)
                    .HasComment("保存方式");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.Specification)
                    .HasMaxLength(255)
                    .HasComment("规格");

                entity.Property(e => e.Supplier)
                    .HasMaxLength(255)
                    .HasComment("供货商");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.ElectronicBill)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ElectronicBill_School");
            });

            modelBuilder.Entity<Flow>(entity =>
            {
                entity.HasKey(e => e.FlowUuid);

                entity.HasComment("成菜流程(新)");

                entity.Property(e => e.FlowUuid)
                    .HasColumnName("FlowUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Accessory).HasComment("附件");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(100)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.FlowName)
                    .HasMaxLength(255)
                    .HasComment("流程名称");

                entity.Property(e => e.FlowTime)
                    .HasMaxLength(255)
                    .HasComment("流程时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校uuid");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.Flow)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Flow_FK");
            });

            modelBuilder.Entity<FoodType>(entity =>
            {
                entity.HasKey(e => e.TypeUuid)
                    .IsClustered(false);

                entity.HasComment("食材类别");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_FoodType")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.TypeUuid)
                    .HasColumnName("TypeUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.HasKey(e => e.IngredientUuid)
                    .IsClustered(false);

                entity.HasComment("食材库");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_Ingredient")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.IngredientUuid)
                    .HasColumnName("IngredientUUID")
                    .HasComment("食材库UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Accessory).HasComment("图片");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Fat)
                    .HasDefaultValueSql("((0))")
                    .HasComment("脂肪");

                entity.Property(e => e.FoodName)
                    .HasMaxLength(255)
                    .HasComment("食材名称");

                entity.Property(e => e.HeatEnergy)
                    .HasDefaultValueSql("((0))")
                    .HasComment("热能");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.Protein)
                    .HasDefaultValueSql("((0))")
                    .HasComment("蛋白质");

                entity.Property(e => e.Saccharides)
                    .HasDefaultValueSql("((0))")
                    .HasComment("糖类");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.TypeUuid)
                    .HasColumnName("TypeUUID")
                    .HasComment("食材类型");

                entity.Property(e => e.Va)
                    .HasColumnName("VA")
                    .HasDefaultValueSql("((0))")
                    .HasComment("VA");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.Ingredient)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Ingredient_School");

                entity.HasOne(d => d.TypeUu)
                    .WithMany(p => p.Ingredient)
                    .HasForeignKey(d => d.TypeUuid)
                    .HasConstraintName("FK_Ingredient_FoodType");
            });

            modelBuilder.Entity<KitchenVideo>(entity =>
            {
                entity.HasKey(e => e.VideoUuid)
                    .HasName("PK_KitchenVideo_1")
                    .IsClustered(false);

                entity.HasComment("厨房视频");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_KitchenVideo")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.VideoUuid)
                    .HasColumnName("VideoUUID")
                    .HasComment("视频UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Accessory).HasComment("附件");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasComment("视频名称");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasComment("类型");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.KitchenVideo)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_KitchenVideo_School");
            });

            modelBuilder.Entity<LiveShot>(entity =>
            {
                entity.HasKey(e => e.LiveShotUuid)
                    .IsClustered(false);

                entity.HasComment("每餐实拍");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_LiveShot")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.LiveShotUuid)
                    .HasColumnName("LiveShotUUID")
                    .HasComment("实拍UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Accessory).HasComment("附件");

                entity.Property(e => e.Accessoryvido).HasComment("视频附件");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.CuisineUuid)
                    .HasColumnName("CuisineUUID")
                    .HasComment("食品UUID");

                entity.Property(e => e.Datetime)
                    .HasMaxLength(50)
                    .HasComment("图片对应的日期");

                entity.Property(e => e.Datetype)
                    .HasMaxLength(50)
                    .HasComment("对应的时段(早上,中午,晚上)");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.HasOne(d => d.CuisineUu)
                    .WithMany(p => p.LiveShot)
                    .HasForeignKey(d => d.CuisineUuid)
                    .HasConstraintName("FK_LiveShot_Cuisine");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.LiveShot)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_LiveShot_School");
            });

            modelBuilder.Entity<ManagePlan>(entity =>
            {
                entity.HasKey(e => e.ManageUuid)
                    .IsClustered(false);

                entity.HasComment("管理方案");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_ManagePlan")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.ManageUuid)
                    .HasColumnName("ManageUUID")
                    .HasComment("管理方案UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Content).HasComment("内容");

                entity.Property(e => e.Headline)
                    .HasMaxLength(255)
                    .HasComment("标题");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.ManagePlan)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ManagePlan_School");
            });

            modelBuilder.Entity<MealFlow>(entity =>
            {
                entity.HasKey(e => e.MealFlowUuid)
                    .IsClustered(false);

                entity.HasComment("成菜流程");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_MealFlow")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.MealFlowUuid)
                    .HasColumnName("MealFlowUUID")
                    .HasComment("成菜流程UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Buying).HasComment("买入人员");

                entity.Property(e => e.BuyingTime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("买入时间");

                entity.Property(e => e.Chopper).HasComment("切配人员");

                entity.Property(e => e.ChopperTime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("切配时间");

                entity.Property(e => e.Cook).HasComment("成菜人员");

                entity.Property(e => e.CookTime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("成菜时间");

                entity.Property(e => e.CuisineUuid)
                    .HasColumnName("CuisineUUID")
                    .HasComment("菜品UUID");

                entity.Property(e => e.Detection).HasComment("检测人员");

                entity.Property(e => e.DetectionTime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("检查时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.MealType)
                    .HasMaxLength(255)
                    .HasComment("用餐类别");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.WashVege).HasComment("洗菜人员");

                entity.Property(e => e.WashVegeTime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("洗菜时间");

                entity.HasOne(d => d.CuisineUu)
                    .WithMany(p => p.MealFlow)
                    .HasForeignKey(d => d.CuisineUuid)
                    .HasConstraintName("FK_MealFlow_Cuisine");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.MealFlow)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MealFlow_School");
            });

            modelBuilder.Entity<MessageBoard>(entity =>
            {
                entity.HasKey(e => e.MessageUuid)
                    .IsClustered(false);

                entity.HasComment("留言板");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_MessageBoard")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.MessageUuid)
                    .HasColumnName("MessageUUID")
                    .HasComment("留言UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("留言用户");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasComment("留言内容");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除 0.未 1.已删除");

                entity.Property(e => e.MessageDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("留言时间");

                entity.Property(e => e.Response)
                    .HasMaxLength(255)
                    .HasComment("回复内容");

                entity.Property(e => e.ResponseDate)
                    .HasColumnType("datetime")
                    .HasComment("回复时间");

                entity.Property(e => e.ResponseType)
                    .HasMaxLength(255)
                    .HasComment("回复类型");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.State).HasComment("状态0.未回复 1.已回复");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("用户UUID");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasComment("留言类型");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.MessageBoard)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_MessageBoard_School");

                entity.HasOne(d => d.SystemUserUu)
                    .WithMany(p => p.MessageBoard)
                    .HasForeignKey(d => d.SystemUserUuid)
                    .HasConstraintName("FK_MessageBoard_SystemUser");
            });

            modelBuilder.Entity<NweekMenu>(entity =>
            {
                entity.HasKey(e => e.NweekMenuUuid)
                    .IsClustered(false);

                entity.ToTable("NWeekMenu");

                entity.HasComment("下周菜单");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_NWeekMenu")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.NweekMenuUuid)
                    .HasColumnName("NWeekMenuUUID")
                    .HasComment("下周菜单UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Datetimes)
                    .HasMaxLength(255)
                    .HasComment("时间范围");

                entity.Property(e => e.FriMon).HasComment("周五早上");

                entity.Property(e => e.FriNight).HasComment("周五晚上");

                entity.Property(e => e.FriNoon).HasComment("周五中午");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.MonMon).HasComment("周一早上");

                entity.Property(e => e.MonNight).HasComment("周一晚上");

                entity.Property(e => e.MonNoon).HasComment("周一中午");

                entity.Property(e => e.SatMon).HasComment("周六早上");

                entity.Property(e => e.SatNight).HasComment("周六晚上");

                entity.Property(e => e.SatNoon).HasComment("周六中午");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.SunMon).HasComment("周日早上");

                entity.Property(e => e.SunNight).HasComment("周日晚上");

                entity.Property(e => e.SunNoon).HasComment("周日中午");

                entity.Property(e => e.ThursMon).HasComment("周四早上");

                entity.Property(e => e.ThursNight).HasComment("周四晚上");

                entity.Property(e => e.ThursNoon).HasComment("周四中午");

                entity.Property(e => e.TuesMon).HasComment("周二早上");

                entity.Property(e => e.TuesNight).HasComment("周二晚上");

                entity.Property(e => e.TuesNoon).HasComment("周二中午");

                entity.Property(e => e.WedMon).HasComment("周三早上");

                entity.Property(e => e.WedNight).HasComment("周三晚上");

                entity.Property(e => e.WedNoon).HasComment("周三中午");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.NweekMenu)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_NWeekMenu_School");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderUuid)
                    .IsClustered(false);

                entity.HasComment("订单表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_Order")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.OrderUuid)
                    .HasColumnName("OrderUUID")
                    .HasComment("订单uuid")
                    .ValueGeneratedNever();

                entity.Property(e => e.AmountPayable)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))")
                    .HasComment("应付金额");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.OederMoney)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))")
                    .HasComment("支付金额");

                entity.Property(e => e.OederName)
                    .HasMaxLength(100)
                    .HasComment("订单名称");

                entity.Property(e => e.OederTime)
                    .HasColumnType("datetime")
                    .HasComment("订单时间");

                entity.Property(e => e.OrderNum)
                    .HasMaxLength(255)
                    .HasComment("订单编号");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("电话");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校id");

                entity.Property(e => e.StudentUuid)
                    .HasColumnName("StudentUUID")
                    .HasComment("学生uuid");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("支付人uuid");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Order_School");

                entity.HasOne(d => d.StudentUu)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.StudentUuid)
                    .HasConstraintName("FK_Order_Student");

                entity.HasOne(d => d.SystemUserUu)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.SystemUserUuid)
                    .HasConstraintName("FK_Order_SystemUser");
            });

            modelBuilder.Entity<PostJobsAppeal>(entity =>
            {
                entity.HasKey(e => e.PostJobsAppealUuid)
                    .IsClustered(false);

                entity.HasComment("勤工俭学申请表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_PostJobsAppeal")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.PostJobsAppealUuid)
                    .HasColumnName("PostJobsAppealUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("添加时间");

                entity.Property(e => e.AppealPeople)
                    .HasMaxLength(50)
                    .HasComment("添加人");

                entity.Property(e => e.Class)
                    .HasMaxLength(50)
                    .HasComment("班级");

                entity.Property(e => e.Grade)
                    .HasMaxLength(50)
                    .HasComment("年段");

                entity.Property(e => e.GuardianName)
                    .HasMaxLength(50)
                    .HasComment("监护人姓名");

                entity.Property(e => e.GuardianPhone)
                    .HasMaxLength(50)
                    .HasComment("监护人手机");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PoorState).HasComment("是否贫困生0.是1.否");

                entity.Property(e => e.PostUuid)
                    .HasColumnName("PostUUID")
                    .HasComment("岗位uuid");

                entity.Property(e => e.Sex)
                    .HasMaxLength(2)
                    .HasComment("性别");

                entity.Property(e => e.State).HasComment("审核状态0.待审核1.通过2.未通过");

                entity.Property(e => e.StuName)
                    .HasMaxLength(50)
                    .HasComment("学生姓名");

                entity.HasOne(d => d.PostUu)
                    .WithMany(p => p.PostJobsAppeal)
                    .HasForeignKey(d => d.PostUuid)
                    .HasConstraintName("FK__PostJobsA__PostU__29221CFB");
            });

            modelBuilder.Entity<Postjobs>(entity =>
            {
                entity.HasKey(e => e.PostUuid)
                    .IsClustered(false);

                entity.HasComment("勤工俭学岗位表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_Postjobs")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.PostUuid)
                    .HasColumnName("PostUUID")
                    .HasComment("勤工俭学UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("添加时间");

                entity.Property(e => e.FullState).HasComment("招收是否已满 0 未满 1 已满");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除 0.未删除1.已删除");

                entity.Property(e => e.Num).HasComment("招收人数");

                entity.Property(e => e.ReleaseState).HasComment("发布状态 0 未发布 1 已发布");

                entity.Property(e => e.Require)
                    .HasMaxLength(255)
                    .HasComment("用人要求");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.Site)
                    .HasMaxLength(255)
                    .HasComment("工作地点");

                entity.Property(e => e.Unit)
                    .HasMaxLength(255)
                    .HasComment("单位");

                entity.Property(e => e.UnitName)
                    .HasMaxLength(255)
                    .HasComment("岗位名称");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.Postjobs)
                    .HasForeignKey(d => d.SchoolUuid)
                    .HasConstraintName("FK__Postjobs__School__1F98B2C1");
            });

            modelBuilder.Entity<PurchaseRecord>(entity =>
            {
                entity.HasKey(e => e.PurchaseUuid)
                    .IsClustered(false);

                entity.HasComment("采购记录");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_PurchaseRecord")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.PurchaseUuid)
                    .HasColumnName("PurchaseUUID")
                    .HasComment("采购记录UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Accessory).HasComment("附件");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Fat)
                    .HasDefaultValueSql("((0))")
                    .HasComment("脂肪");

                entity.Property(e => e.HeatEnergy)
                    .HasDefaultValueSql("((0))")
                    .HasComment("热能");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IngredientUuid)
                    .HasColumnName("IngredientUUID")
                    .HasComment("食材UUID");

                entity.Property(e => e.IsDelete)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否删除");

                entity.Property(e => e.Price)
                    .HasDefaultValueSql("((0))")
                    .HasComment("采购价格");

                entity.Property(e => e.Protein)
                    .HasDefaultValueSql("((0))")
                    .HasComment("蛋白质");

                entity.Property(e => e.PurchaseDate)
                    .HasMaxLength(255)
                    .HasComment("采购日期");

                entity.Property(e => e.PurchaseNum)
                    .HasMaxLength(255)
                    .HasComment("采购数量");

                entity.Property(e => e.Saccharides)
                    .HasDefaultValueSql("((0))")
                    .HasComment("糖类");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.State)
                    .HasMaxLength(255)
                    .HasComment("状态");

                entity.Property(e => e.Supplier)
                    .HasMaxLength(255)
                    .HasComment("供应商");

                entity.Property(e => e.SystemUserUuid)
                    .IsRequired()
                    .HasColumnName("SystemUserUUID")
                    .HasComment("采购人（用户UUID）");

                entity.Property(e => e.Unit)
                    .HasMaxLength(100)
                    .HasComment("单位");

                entity.Property(e => e.Va)
                    .HasColumnName("VA")
                    .HasDefaultValueSql("((0))")
                    .HasComment("VA");

                entity.HasOne(d => d.IngredientUu)
                    .WithMany(p => p.PurchaseRecord)
                    .HasForeignKey(d => d.IngredientUuid)
                    .HasConstraintName("FK_PurchaseRecord_Ingredient");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.PurchaseRecord)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_PurchaseRecord_School");
            });

            modelBuilder.Entity<Quality>(entity =>
            {
                entity.HasKey(e => e.QualityUuid)
                    .HasName("PK__Quality__0D4FBD278C5616DD");

                entity.HasComment("质量文件");

                entity.Property(e => e.QualityUuid)
                    .HasColumnName("QualityUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Accessory).HasComment("附件");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(100)
                    .HasComment("添加时间");

                entity.Property(e => e.EffectiveTime)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("生效时间");

                entity.Property(e => e.FlieName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("文件名称");

                entity.Property(e => e.JianJie).HasComment("简介");

                entity.Property(e => e.SchoolUuid).HasColumnName("SchoolUUID");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.ReportUuid)
                    .IsClustered(false);

                entity.HasComment("举报邮箱");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_Report")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.ReportUuid)
                    .HasColumnName("ReportUUID")
                    .HasComment("举报信箱UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .HasComment("举报内容");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasComment("举报人");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .HasComment("电话");

                entity.Property(e => e.ReplyName)
                    .HasMaxLength(255)
                    .HasComment("处理人");

                entity.Property(e => e.ReplyTime)
                    .HasColumnType("datetime")
                    .HasComment("处理时间");

                entity.Property(e => e.ReportDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("举报时间");

                entity.Property(e => e.Result)
                    .HasMaxLength(255)
                    .HasComment("处理结果");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.State).HasComment("状态0待处理 1.已处理");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("用户UUID");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Report_School");

                entity.HasOne(d => d.SystemUserUu)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.SystemUserUuid)
                    .HasConstraintName("FK_Report_SystemUser");
            });

            modelBuilder.Entity<School>(entity =>
            {
                entity.HasKey(e => e.SchoolUuid)
                    .IsClustered(false);

                entity.HasComment("学校表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_School")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Accessory).HasComment("二维码附件");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsCuiPrices)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否显示菜价");

                entity.Property(e => e.IsDelete).HasComment("是否删除 0未删 1已删");

                entity.Property(e => e.IsYard).HasComment("0:商户码  1:二维码");

                entity.Property(e => e.Isptjob)
                    .HasColumnName("ISPTJob")
                    .HasComment("是否显示勤工俭学");

                entity.Property(e => e.Isrecharge)
                    .HasColumnName("ISRecharge")
                    .HasComment("是否显示饭卡充值");

                entity.Property(e => e.Isreservation)
                    .HasColumnName("ISReservation")
                    .HasComment("是否显示在线订餐");

                entity.Property(e => e.IsshowReport)
                    .IsRequired()
                    .HasColumnName("ISShowReport")
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否启用举报信箱");

                entity.Property(e => e.Link)
                    .HasMaxLength(255)
                    .HasComment("链接");

                entity.Property(e => e.Menus)
                    .HasMaxLength(200)
                    .HasComment("微信菜单");

                entity.Property(e => e.PurchasingStandard).HasComment("采购标准");

                entity.Property(e => e.Qrcode)
                    .HasColumnName("QRcode")
                    .HasComment("二维码图片");

                entity.Property(e => e.SchoolImg)
                    .IsUnicode(false)
                    .HasComment("学校图片");

                entity.Property(e => e.SchoolName)
                    .HasMaxLength(255)
                    .HasComment("学校名称");

                entity.Property(e => e.SchoolType)
                    .HasMaxLength(100)
                    .HasComment("学校类型");

                entity.Property(e => e.Secretkey)
                    .HasMaxLength(255)
                    .HasComment("密钥");

                entity.Property(e => e.Yard)
                    .HasMaxLength(255)
                    .HasComment("商户码");
            });

            modelBuilder.Entity<SchoolJour>(entity =>
            {
                entity.HasKey(e => e.SchoolJourUuid)
                    .IsClustered(false);

                entity.HasComment("校园新闻");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SchoolJour")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SchoolJourUuid)
                    .HasColumnName("SchoolJourUUID")
                    .HasComment("校园新闻UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Accessory).HasComment("附件");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.Addtime)
                    .HasMaxLength(255)
                    .HasComment("添加日期");

                entity.Property(e => e.Content).HasComment("内容");

                entity.Property(e => e.Digest)
                    .HasMaxLength(255)
                    .HasComment("摘要");

                entity.Property(e => e.Headline)
                    .HasMaxLength(255)
                    .HasComment("标题");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除 0未删 1已删");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.Start)
                    .HasMaxLength(255)
                    .HasComment("状态");

                entity.Property(e => e.Tag)
                    .HasMaxLength(100)
                    .HasComment("标签");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.SchoolJour)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SchoolJour_School");
            });

            modelBuilder.Entity<SchoolLife>(entity =>
            {
                entity.HasKey(e => e.SchoollifeUuid)
                    .IsClustered(false);

                entity.HasComment("校园生活");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SchoolLife")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SchoollifeUuid)
                    .HasColumnName("SchoollifeUUID")
                    .HasComment("校园生活UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Accessory).HasComment("附件");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Content).HasComment("内容");

                entity.Property(e => e.Digest)
                    .HasMaxLength(255)
                    .HasComment("摘要");

                entity.Property(e => e.Headline)
                    .HasMaxLength(255)
                    .HasComment("标题");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除 0未删 1已删");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.State)
                    .HasMaxLength(55)
                    .HasComment("状态");

                entity.Property(e => e.Tag)
                    .HasMaxLength(100)
                    .HasComment("标签");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.SchoolLife)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SchoolLife_School");
            });

            modelBuilder.Entity<SchoolNews>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("SchoolNews");

                entity.Property(e => e.AddTime).HasMaxLength(11);

                entity.Property(e => e.Digest).HasMaxLength(255);

                entity.Property(e => e.Headline).HasMaxLength(255);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SchoolUuid).HasColumnName("SchoolUUID");

                entity.Property(e => e.Tag).HasMaxLength(100);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentUuid)
                    .IsClustered(false);

                entity.HasComment("学生表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_Student")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.StudentUuid)
                    .HasColumnName("StudentUUID")
                    .HasComment("学生id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AmountPayable)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("应付金额");

                entity.Property(e => e.ClassName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("班级名称");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdcardNum)
                    .HasColumnName("IDCardNum")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("身份证号");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否删除  0否  1是");

                entity.Property(e => e.OederMoney)
                    .HasColumnType("decimal(18, 2)")
                    .HasComment("已付金额");

                entity.Property(e => e.OederName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("付费项目");

                entity.Property(e => e.ParentsPhone)
                    .HasMaxLength(100)
                    .HasComment("付费人电话");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校");

                entity.Property(e => e.Sex).HasComment("性别");

                entity.Property(e => e.StudentName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("姓名");

                entity.Property(e => e.StudentNum)
                    .HasMaxLength(50)
                    .HasComment("学号");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Student_School");
            });

            modelBuilder.Entity<StudentBill>(entity =>
            {
                entity.HasKey(e => e.StudentBillUuid)
                    .IsClustered(false);

                entity.HasComment("学生订单");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_StudentBill")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.StudentBillUuid)
                    .HasColumnName("StudentBillUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AmountPayable)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))")
                    .HasComment("应付金额");

                entity.Property(e => e.ClassName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("班级");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdcardNum)
                    .HasColumnName("IDCardNum")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("身份证号");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否删除");

                entity.Property(e => e.OederTime).HasComment("支付时间");

                entity.Property(e => e.OrderMoney)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))")
                    .HasComment("已付金额");

                entity.Property(e => e.OrderName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("支付项目");

                entity.Property(e => e.OrderNum).HasComment("订单编号");

                entity.Property(e => e.ParentsPhone)
                    .HasMaxLength(100)
                    .HasComment("支付人手机号");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校uuid");

                entity.Property(e => e.SerialNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasComment("流水号");

                entity.Property(e => e.Sex).HasComment("性别  1男   2女");

                entity.Property(e => e.StudentName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("姓名");

                entity.Property(e => e.StudentNum)
                    .HasMaxLength(50)
                    .HasComment("学号");

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .HasComment("支付人uuid");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.StudentBill)
                    .HasForeignKey(d => d.SchoolUuid)
                    .HasConstraintName("FK_StudentBill_School");
            });

            modelBuilder.Entity<SurverUser>(entity =>
            {
                entity.HasKey(e => e.SurverUserUuid)
                    .IsClustered(false);

                entity.HasComment("调查人员");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SurverUser")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SurverUserUuid)
                    .HasColumnName("SurverUserUUID")
                    .HasComment("调查人员UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Age)
                    .HasMaxLength(255)
                    .HasComment("年纪");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.Major)
                    .HasMaxLength(255)
                    .HasComment("专业");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasComment("姓名");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .HasComment("电话");

                entity.Property(e => e.Remark)
                    .HasMaxLength(255)
                    .HasComment("备注");

                entity.Property(e => e.StudentNum)
                    .HasMaxLength(255)
                    .HasComment("学号");

                entity.Property(e => e.SurverUuid)
                    .HasColumnName("SurverUUID")
                    .HasComment("调查问卷UUID");

                entity.Property(e => e.Year)
                    .HasMaxLength(255)
                    .HasComment("年龄");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.HasKey(e => e.SurveyUuid)
                    .IsClustered(false);

                entity.HasComment("问卷调查列表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_Survey")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SurveyUuid)
                    .HasColumnName("SurveyUUID")
                    .HasComment("问卷调查UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("添加时间");

                entity.Property(e => e.BeginTime)
                    .HasColumnType("datetime")
                    .HasComment("开始时间");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasComment("结束时间");

                entity.Property(e => e.Headline)
                    .HasMaxLength(255)
                    .HasComment("标题");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasComment("是否删除");

                entity.Property(e => e.Number).HasComment("已答人数");

                entity.Property(e => e.ProceedState).HasComment("进行状态");

                entity.Property(e => e.ProductState).HasComment("生产状态");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasComment("类型");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.Survey)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Survey_School");
            });

            modelBuilder.Entity<SurveyAnswer>(entity =>
            {
                entity.HasKey(e => e.SurveyAnswerUuid)
                    .HasName("PK__SurveyAn__D16F394224439A4B")
                    .IsClustered(false);

                entity.HasComment("问卷填写详情");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SurveyAnswer")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SurveyAnswerUuid)
                    .HasColumnName("SurveyAnswerUUID")
                    .HasComment("问卷反馈UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("添加时间");

                entity.Property(e => e.AnswerStr).HasComment("回答字符串 ,用||分隔，多选的题目用逗号分隔");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("标识ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasDefaultValueSql("((1))")
                    .HasComment("标记删除 0.未删除  1.已删除");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.SurveyUuid)
                    .HasColumnName("SurveyUUID")
                    .HasComment("问卷UUID");
            });

            modelBuilder.Entity<SurveyQuestions>(entity =>
            {
                entity.HasKey(e => e.SurveyQuestionsUuid)
                    .HasName("PK__SurveyQu__C4E3EFF5B319C01E")
                    .IsClustered(false);

                entity.HasComment("问卷题目");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SurveyQuestions")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SurveyQuestionsUuid)
                    .HasColumnName("SurveyQuestionsUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("添加时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("标识ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasDefaultValueSql("((1))")
                    .HasComment("标记删除 0.未删除  1.已删除");

                entity.Property(e => e.IsMuti)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否允许多选（0否，1是）");

                entity.Property(e => e.QuestionTitle)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("题目名称");

                entity.Property(e => e.QuestionType).HasComment("题目类型（0客观题，1主观题）");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.SurveyUuid)
                    .HasColumnName("SurveyUUID")
                    .HasComment("调查反馈UUID");
            });

            modelBuilder.Entity<SurveyQuestionsItems>(entity =>
            {
                entity.HasKey(e => e.SurveyQuestionsItemsUuid)
                    .HasName("PK__SurveyQu__6CCFC5CA01B59254")
                    .IsClustered(false);

                entity.HasComment("问卷选项");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SurveyQuestionsItems")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SurveyQuestionsItemsUuid)
                    .HasColumnName("SurveyQuestionsItemsUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPerson)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime2(1)")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("添加时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasComment("标识ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否删除0.未删1.已删");

                entity.Property(e => e.Optionts)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("选项序号");

                entity.Property(e => e.QuestionStr)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("选项内容");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.SurveyQuestionsUuid)
                    .HasColumnName("SurveyQuestionsUUID")
                    .HasComment("调查反馈问题UUID");
            });

            modelBuilder.Entity<SystemIcon>(entity =>
            {
                entity.HasKey(e => e.SystemIconUuid)
                    .HasName("PK__SystemIc__540CED9D42DF8109")
                    .IsClustered(false);

                entity.HasComment("图标");

                entity.HasIndex(e => e.SystemIconUuid)
                    .HasName("IX_SystemIcon")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SystemIconUuid)
                    .HasColumnName("SystemIconUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Custom).HasMaxLength(60);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Size).HasMaxLength(20);
            });

            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.HasKey(e => e.SystemLogUuid)
                    .HasName("PK__SystemLo__65A475E79A921FFD")
                    .IsClustered(false);

                entity.HasComment("系统日志表");

                entity.HasIndex(e => e.SystemLogUuid)
                    .HasName("IX_SystemLog")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SystemLogUuid)
                    .HasColumnName("SystemLogUUID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasColumnType("datetime")
                    .HasComment("添加时间");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Ipaddress)
                    .HasColumnName("IPAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("IP地址");

                entity.Property(e => e.IsDelete).HasComment("标记删除1，未删除2，已删除");

                entity.Property(e => e.OperationContent)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasComment("操作内容");

                entity.Property(e => e.OperationTime)
                    .HasColumnType("datetime")
                    .HasComment("操作时间");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("操作类型(增删改查)");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("操作用户ID");

                entity.Property(e => e.UserIdtype)
                    .HasColumnName("UserIDType")
                    .HasComment("用户名和类型");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("操作用户用户名");
            });

            modelBuilder.Entity<SystemMenu>(entity =>
            {
                entity.HasKey(e => e.SystemMenuUuid)
                    .HasName("PK__DncMenu__A2B5777CA1511602")
                    .IsClustered(false);

                entity.HasComment("菜单表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SystemMenu")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SystemMenuUuid)
                    .HasColumnName("SystemMenuUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.BeforeCloseFun).HasMaxLength(255);

                entity.Property(e => e.Component).HasMaxLength(255);

                entity.Property(e => e.CreatedByUserName).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedByUserName).HasMaxLength(255);

                entity.Property(e => e.ModifiedOn).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ParentName).HasMaxLength(255);

                entity.Property(e => e.Url).HasMaxLength(255);
            });

            modelBuilder.Entity<SystemPermission>(entity =>
            {
                entity.HasKey(e => e.SystemPermissionUuid)
                    .HasName("PK__DncPermi__18DD8CCDCC3FD718")
                    .IsClustered(false);

                entity.HasComment("权限表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SystemPermission")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SystemPermissionUuid)
                    .HasColumnName("SystemPermissionUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActionCode).HasMaxLength(255);

                entity.Property(e => e.CaPower).HasMaxLength(255);

                entity.Property(e => e.CreatedByUserName).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ModifiedByUserName).HasMaxLength(255);

                entity.Property(e => e.ModifiedOn).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SystemMenuUuid).HasColumnName("SystemMenuUUID");

                entity.HasOne(d => d.SystemMenuUu)
                    .WithMany(p => p.SystemPermission)
                    .HasForeignKey(d => d.SystemMenuUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SystemPermission_SystemMenu");
            });

            modelBuilder.Entity<SystemRole>(entity =>
            {
                entity.HasKey(e => e.SystemRoleUuid)
                    .HasName("PK__DncRole__DF75FB283AD1E2C6")
                    .IsClustered(false);

                entity.HasComment("角色表");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SystemRole")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SystemRoleUuid)
                    .HasColumnName("SystemRoleUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople).HasMaxLength(255);

                entity.Property(e => e.AddTime).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsCreEdu)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否由管理者创建");

                entity.Property(e => e.IsFixation)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否内置  0否,1是");

                entity.Property(e => e.RoleName).HasMaxLength(255);

                entity.Property(e => e.SchoolUuid).HasColumnName("SchoolUUID");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.SystemRole)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SystemRole_School");
            });

            modelBuilder.Entity<SystemRolePermissionMapping>(entity =>
            {
                entity.HasKey(e => new { e.SystemRoleUuid, e.SystemPermissionUuid })
                    .IsClustered(false);

                entity.HasComment("角色权限关系");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SystemRolePermissionMapping")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");

                entity.Property(e => e.SystemPermissionUuid).HasColumnName("SystemPermissionUUID");

                entity.Property(e => e.AddPeople).HasMaxLength(255);

                entity.Property(e => e.AddTime).HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SystemUser>(entity =>
            {
                entity.HasKey(e => e.SystemUserUuid)
                    .HasName("PK__DncUser__A2B5777C0780DFF0")
                    .IsClustered(false);

                entity.HasComment("系统用户");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SystemUser")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SystemUserUuid)
                    .HasColumnName("SystemUserUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("注册时间");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("地址");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("邮箱");

                entity.Property(e => e.HealthCertificate).HasComment("是否有健康证    0无   1有");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.InTime)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("入职时间");

                entity.Property(e => e.IsAddPrecord)
                    .HasColumnName("IsAddPRecord")
                    .HasComment("微信用户是否添加采购记录  0否 1是");

                entity.Property(e => e.IsCreEdu)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否由管理级别创建  0否   1是");

                entity.Property(e => e.IsDeleted).HasComment("0正常 1删除");

                entity.Property(e => e.IsFlow).HasComment("微信用户是否可以上传成菜流程图片  0否  1是");

                entity.Property(e => e.IsStaff)
                    .HasDefaultValueSql("((0))")
                    .HasComment("是否为食堂工作人员 0否   1是");

                entity.Property(e => e.IsUploadPicture).HasComment("微信用户是否上传图片  0否  1是");

                entity.Property(e => e.IsUploadVideo).HasComment("微信用户是否可以上传视频文件  0否  1是");

                entity.Property(e => e.Job)
                    .HasMaxLength(255)
                    .HasComment("职务");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(255)
                    .HasComment("登录名");

                entity.Property(e => e.Main)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ManageDepartment).HasColumnType("text");

                entity.Property(e => e.PassWord)
                    .HasMaxLength(255)
                    .HasComment("密码");

                entity.Property(e => e.Phone)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("电话");

                entity.Property(e => e.RealName)
                    .HasMaxLength(255)
                    .HasComment("真实姓名");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("备注");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.Sex)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("性别");

                entity.Property(e => e.StaffNum)
                    .HasMaxLength(255)
                    .HasComment("职工号");

                entity.Property(e => e.SystemRoleUuid)
                    .HasColumnName("SystemRoleUUid")
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("角色id,用逗号分隔");

                entity.Property(e => e.Topic)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("会议主题");

                entity.Property(e => e.Train)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Types)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("部门");

                entity.Property(e => e.UserIdCard)
                    .HasMaxLength(255)
                    .HasComment("身份证");

                entity.Property(e => e.UserType).HasComment("1 超管 2其他");

                entity.Property(e => e.Wechat)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("微信");

                entity.Property(e => e.ZaiGang)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasComment("状态");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.SystemUser)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SystemUser_School");
            });

            modelBuilder.Entity<SystemUserRoleMapping>(entity =>
            {
                entity.HasKey(e => new { e.SystemUserUuid, e.SystemRoleUuid })
                    .HasName("PK__DncUserR__328A96E56EE320C2")
                    .IsClustered(false);

                entity.HasComment("用户角色关系");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_SystemUserRoleMapping")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.SystemUserUuid).HasColumnName("SystemUserUUID");

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");

                entity.Property(e => e.AddPeople).HasMaxLength(255);

                entity.Property(e => e.AddTime)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Train>(entity =>
            {
                entity.HasKey(e => e.TrainUuid)
                    .IsClustered(false);

                entity.HasComment("培训信息");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_Train")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.TrainUuid)
                    .HasColumnName("TrainUUID")
                    .HasComment("培训信息UUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddPeople)
                    .HasMaxLength(255)
                    .HasComment("添加人");

                entity.Property(e => e.AddTime)
                    .HasMaxLength(255)
                    .HasComment("添加时间");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasComment("培训地址");

                entity.Property(e => e.Anum)
                    .HasMaxLength(255)
                    .HasComment("下午培训人数");

                entity.Property(e => e.Content).HasComment("培训内容");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(255)
                    .HasComment("是否删除");

                entity.Property(e => e.Mnum)
                    .HasMaxLength(255)
                    .HasComment("上午培训人数");

                entity.Property(e => e.SchoolUuid)
                    .HasColumnName("SchoolUUID")
                    .HasComment("学校UUID");

                entity.Property(e => e.Speaker)
                    .HasMaxLength(255)
                    .HasComment("授课人（多选）");

                entity.Property(e => e.Theme)
                    .HasMaxLength(255)
                    .HasComment("主题");

                entity.Property(e => e.TrainLession).HasComment("培训课时");

                entity.Property(e => e.TrainName)
                    .HasMaxLength(255)
                    .HasComment("培训人员（多选）");

                entity.Property(e => e.TrainTime)
                    .HasMaxLength(255)
                    .HasComment("培训时间");

                entity.HasOne(d => d.SchoolUu)
                    .WithMany(p => p.Train)
                    .HasForeignKey(d => d.SchoolUuid)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Train_School");
            });

            modelBuilder.Entity<ViewMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_Menu");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.BeforeCloseFun).HasMaxLength(255);

                entity.Property(e => e.Component).HasMaxLength(255);

                entity.Property(e => e.CreatedByUserName).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasMaxLength(255);

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.ModifiedByUserName).HasMaxLength(255);

                entity.Property(e => e.ModifiedOn).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ParentName).HasMaxLength(255);

                entity.Property(e => e.Pd).HasColumnName("pd");

                entity.Property(e => e.Ps).HasColumnName("ps");

                entity.Property(e => e.Pt).HasColumnName("pt");

                entity.Property(e => e.SystemMenuUuid).HasColumnName("SystemMenuUUID");

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");

                entity.Property(e => e.Url).HasMaxLength(255);
            });

            modelBuilder.Entity<ViewSystemPermissionWithMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_SystemPermissionWithMenu");

                entity.Property(e => e.MenuAlias).HasMaxLength(255);

                entity.Property(e => e.MenuName).HasMaxLength(255);

                entity.Property(e => e.Pd).HasColumnName("pd");

                entity.Property(e => e.PermissionActionCode).HasMaxLength(255);

                entity.Property(e => e.PermissionName).HasMaxLength(255);

                entity.Property(e => e.Ps).HasColumnName("ps");

                entity.Property(e => e.SystemRoleUuid).HasColumnName("SystemRoleUUID");
            });

            modelBuilder.Entity<ViewSystemPermissionWithMenu2>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_SystemPermissionWithMenu2");

                entity.Property(e => e.MenuAlias).HasMaxLength(255);

                entity.Property(e => e.MenuName).HasMaxLength(255);

                entity.Property(e => e.Pd).HasColumnName("pd");

                entity.Property(e => e.PermissionActionCode).HasMaxLength(255);

                entity.Property(e => e.PermissionName).HasMaxLength(255);

                entity.Property(e => e.Ps).HasColumnName("ps");
            });

            modelBuilder.Entity<Wxmenu>(entity =>
            {
                entity.HasKey(e => e.WxmenuUuid)
                    .IsClustered(false);

                entity.ToTable("WXMenu");

                entity.HasIndex(e => e.Id)
                    .HasName("IX_WXMenu")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.WxmenuUuid)
                    .HasColumnName("WXMenuUUID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IsDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.MenuName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
