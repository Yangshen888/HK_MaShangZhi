using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class haikanITMContext : DbContext
    {
        public haikanITMContext()
        {
        }

        public haikanITMContext(DbContextOptions<haikanITMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<DisinfectionRecords> DisinfectionRecords { get; set; }
        public virtual DbSet<Disinfections> Disinfections { get; set; }
        public virtual DbSet<Duties> Duties { get; set; }
        public virtual DbSet<Goods> Goods { get; set; }
        public virtual DbSet<InspectionInformation> InspectionInformation { get; set; }
        public virtual DbSet<Inspections> Inspections { get; set; }
        public virtual DbSet<Lights> Lights { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Migrations> Migrations { get; set; }
        public virtual DbSet<Monitors> Monitors { get; set; }
        public virtual DbSet<Orgs> Orgs { get; set; }
        public virtual DbSet<Partners> Partners { get; set; }
        public virtual DbSet<Pesticides> Pesticides { get; set; }
        public virtual DbSet<PurchaseInformation> PurchaseInformation { get; set; }
        public virtual DbSet<Purchases> Purchases { get; set; }
        public virtual DbSet<Samples> Samples { get; set; }
        public virtual DbSet<Screenshots> Screenshots { get; set; }
        public virtual DbSet<Synthesizes> Synthesizes { get; set; }
        public virtual DbSet<Types> Types { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Vegetables> Vegetables { get; set; }
        public virtual DbSet<Wastes> Wastes { get; set; }
        public virtual DbSet<WorldBarnBuyers> WorldBarnBuyers { get; set; }
        public virtual DbSet<WorldBarnGoods> WorldBarnGoods { get; set; }
        public virtual DbSet<WorldBarnGoodsCategories> WorldBarnGoodsCategories { get; set; }
        public virtual DbSet<WorldBarnOrderItems> WorldBarnOrderItems { get; set; }
        public virtual DbSet<WorldBarnOrders> WorldBarnOrders { get; set; }
        public virtual DbSet<WorldBarnUsers> WorldBarnUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseMySql("server=192.168.0.174;userid=root;pwd=root;port=3306;database=iot_tool_msa;sslmode=none", x => x.ServerVersion("8.0.18-mysql"));
                optionsBuilder.UseMySql("server=10.0.2.3;user id=iot_tool_msa_ro;password=AQkf_wxFruNRDN3i;persistsecurityinfo=True;database=iot_tool_msa", x => x.ServerVersion("8.0.18-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departments>(entity =>
            {
                entity.ToTable("departments");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("departments_organization_id_index");

                entity.HasIndex(e => e.Status)
                    .HasName("departments_status_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasComment("部门名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(11)")
                    .HasComment("组织id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)")
                    .HasComment("状态");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<DisinfectionRecords>(entity =>
            {
                entity.ToTable("disinfection_records");

                entity.HasIndex(e => e.DisinfectionId)
                    .HasName("records_disinfection_id_index");

                entity.HasIndex(e => e.DisinfectionRecordId)
                    .HasName("disinfection_records_disinfection_record_id_index");

                entity.HasIndex(e => e.MethodId)
                    .HasName("records_method_id_index");

                entity.HasIndex(e => e.RoomId)
                    .HasName("records_room_id_index");

                entity.HasIndex(e => e.ToolId)
                    .HasName("records_tool_id_index");

                entity.HasIndex(e => e.ToolName)
                    .HasName("records_tool_name_index");

                entity.HasIndex(e => e.Type)
                    .HasName("records_type_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("create_user_id")
                    .HasColumnType("int(11)")
                    .HasComment("创建用户id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DisinfectionId)
                    .HasColumnName("disinfection_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("消毒id");

                entity.Property(e => e.DisinfectionRecordId)
                    .HasColumnName("disinfection_record_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安消毒记录id");

                entity.Property(e => e.Method)
                    .HasColumnName("method")
                    .HasColumnType("varchar(255)")
                    .HasComment("消毒方式")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.MethodId)
                    .HasColumnName("method_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("消毒方式id");

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("消毒数量/时长");

                entity.Property(e => e.OwnerDisinfectionId)
                    .HasColumnName("owner_disinfection_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("消毒日志表(disinfection)中的id");

                entity.Property(e => e.RoomId)
                    .HasColumnName("room_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("消毒间id");

                entity.Property(e => e.RoomName)
                    .HasColumnName("room_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("消毒间名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ToolId)
                    .HasColumnName("tool_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("工具id");

                entity.Property(e => e.ToolName)
                    .HasColumnName("tool_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("工具名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("int(11)")
                    .HasComment("消毒对象类型");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasColumnName("type_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("消毒对象类型名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<Disinfections>(entity =>
            {
                entity.ToTable("disinfections");

                entity.HasIndex(e => e.DisinfectionId)
                    .HasName("disinfection_disinfection_id_index");

                entity.HasIndex(e => e.IsClean)
                    .HasName("disinfections_is_clean_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("disinfections_organization_id_index");

                entity.HasIndex(e => e.Sync)
                    .HasName("disinfections_sync_index");

                entity.HasIndex(e => e.Type)
                    .HasName("disinfections_type_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("create_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("创建人id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("date")
                    .HasComment("创建日期");

                entity.Property(e => e.Disinfection)
                    .HasColumnName("disinfection")
                    .HasColumnType("varchar(255)")
                    .HasComment("消毒对象")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.DisinfectionId)
                    .HasColumnName("disinfection_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安消毒日志id");

                entity.Property(e => e.DisinfectionUserId)
                    .HasColumnName("disinfection_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("消毒人员id");

                entity.Property(e => e.DisinfectionUserName)
                    .HasColumnName("disinfection_user_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("消毒人员")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Img)
                    .HasColumnName("img")
                    .HasColumnType("varchar(255)")
                    .HasComment("消毒封面图")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ImgUrls)
                    .IsRequired()
                    .HasColumnName("img_urls")
                    .HasColumnType("text")
                    .HasComment("众食安消毒图片集合")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.IsClean).HasColumnName("is_clean");

                entity.Property(e => e.Method)
                    .HasColumnName("method")
                    .HasColumnType("varchar(255)")
                    .HasComment("消毒方式")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("组织id");

                entity.Property(e => e.Sync)
                    .HasColumnName("sync")
                    .HasColumnType("tinyint(3) unsigned")
                    .HasComment("是否同步");

                entity.Property(e => e.SyncAt)
                    .HasColumnName("sync_at")
                    .HasColumnType("timestamp")
                    .HasComment("同步时间");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("消毒餐次类型");

                entity.Property(e => e.TypeName)
                    .HasColumnName("type_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("消毒餐次名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<Duties>(entity =>
            {
                entity.ToTable("duties");

                entity.HasIndex(e => e.DutyId)
                    .HasName("duties_duty_id_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("duties_organization_id_index");

                entity.HasIndex(e => e.Status)
                    .HasName("duties_status_index");

                entity.HasIndex(e => e.Sync)
                    .HasName("duties_sync_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DutyId)
                    .HasColumnName("duty_id")
                    .HasColumnType("int(11)")
                    .HasComment("众食安职位id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(191)")
                    .HasComment("名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(11)")
                    .HasComment("组织id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(4)")
                    .HasComment("是否显示？");

                entity.Property(e => e.Sync)
                    .HasColumnName("sync")
                    .HasComment("是否同步");

                entity.Property(e => e.SyncAt)
                    .HasColumnName("sync_at")
                    .HasColumnType("timestamp")
                    .HasComment("同步时间");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<Goods>(entity =>
            {
                entity.ToTable("goods");

                entity.HasIndex(e => e.GoodsId)
                    .HasName("goods_goods_id_index");

                entity.HasIndex(e => e.MealTime)
                    .HasName("goods_meal_time_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("goods_organization_id_index");

                entity.HasIndex(e => e.Type)
                    .HasName("goods_type_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Explains)
                    .HasColumnName("explains")
                    .HasColumnType("varchar(191)")
                    .HasComment("说明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.GoodsId)
                    .HasColumnName("goods_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安goods_id");

                entity.Property(e => e.Img)
                    .HasColumnName("img")
                    .HasColumnType("varchar(191)")
                    .HasComment("商品图片")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.MealTime)
                    .HasColumnName("meal_time")
                    .HasColumnType("int(11)")
                    .HasComment("进食次数");

                entity.Property(e => e.MealTimeName)
                    .HasColumnName("meal_time_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("餐次名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(191)")
                    .HasComment("商品名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安组织id");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(8,2) unsigned")
                    .HasComment("价格");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("int(11)")
                    .HasComment("类型");

                entity.Property(e => e.TypeName)
                    .HasColumnName("type_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("类型名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<InspectionInformation>(entity =>
            {
                entity.ToTable("inspection_information");

                entity.HasIndex(e => e.InspectionId)
                    .HasName("inspection_information_inspection_id_index");

                entity.HasIndex(e => e.InspectionInformationId)
                    .HasName("inspections_inspection_information_id_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("inspection_information_organization_id_index");

                entity.HasIndex(e => e.UserId)
                    .HasName("inspection_information_user_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.AiFaceImg)
                    .HasColumnName("ai_face_img")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Attendance)
                    .HasColumnName("attendance")
                    .HasColumnType("varchar(255)")
                    .HasComment("考勤;0-到岗,1-事假，2-病假，3-其他")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.AttendanceTime)
                    .HasColumnName("attendance_time")
                    .HasColumnType("varchar(255)")
                    .HasComment("考勤时长类别;0-全天，1-上午，2-下午")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CardNumber)
                    .HasColumnName("card_number")
                    .HasColumnType("varchar(255)")
                    .HasComment("证件号")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CheckStatus)
                    .HasColumnName("check_status")
                    .HasColumnType("varchar(255)")
                    .HasComment("检查状态")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("create_user_id")
                    .HasColumnType("int(11)")
                    .HasComment("创建用户ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Duty)
                    .HasColumnName("duty")
                    .HasColumnType("varchar(255)")
                    .HasComment("职位")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.HandImg)
                    .HasColumnName("hand_img")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ImgUrl)
                    .HasColumnName("img_url")
                    .HasColumnType("varchar(255)")
                    .HasComment("img_url")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.InspectionId)
                    .HasColumnName("inspection_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安晨检ID");

                entity.Property(e => e.InspectionInformationId)
                    .HasColumnName("inspection_information_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安详情晨检ID");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("varchar(255)")
                    .HasComment("情况说明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("组织id");

                entity.Property(e => e.OwnerInspectionId)
                    .HasColumnName("owner_inspection_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("晨检表(inspections)中的id");

                entity.Property(e => e.PeopleStatus)
                    .HasColumnName("people_status")
                    .HasColumnType("varchar(255)")
                    .HasComment("晨检状态；众食安晨检id 以\",\"连接")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.PeopleStatusName)
                    .HasColumnName("people_status_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("晨检状态名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ProsessInfo)
                    .HasColumnName("prosess_info")
                    .HasColumnType("varchar(255)")
                    .HasComment("处理意见;0-正常上岗，1-离岗休息, 2-整改到岗")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Temperature)
                    .HasColumnName("temperature")
                    .HasColumnType("varchar(255)")
                    .HasComment("体温")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.TemperatureImg)
                    .HasColumnName("temperature_img")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasDefaultValueSql("'0'")
                    .HasComment("用户ID");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("用户名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Inspections>(entity =>
            {
                entity.ToTable("inspections");

                entity.HasIndex(e => e.CreatedUser)
                    .HasName("inspections_created_user_index");

                entity.HasIndex(e => e.DepartmentId)
                    .HasName("inspections_department_id_index");

                entity.HasIndex(e => e.InspectionId)
                    .HasName("inspections_inspection_id_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("inspections_organization_id_index");

                entity.HasIndex(e => e.Sync)
                    .HasName("inspections_sync_index");

                entity.HasIndex(e => e.TeamGroupId)
                    .HasName("inspections_team_group_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ActualCount)
                    .HasColumnName("actual_count")
                    .HasColumnType("varchar(191)")
                    .HasComment("实际人数")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("date")
                    .HasComment("创建日期");

                entity.Property(e => e.CreatedUser)
                    .HasColumnName("created_user")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("创建用户id");

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("department_id")
                    .HasColumnType("int(11)")
                    .HasComment("部门id");

                entity.Property(e => e.DepartmentName)
                    .HasColumnName("department_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("部门名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.InspectionId)
                    .HasColumnName("inspection_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安晨检日志id");

                entity.Property(e => e.Inspector)
                    .HasColumnName("inspector")
                    .HasColumnType("varchar(191)")
                    .HasComment("检查员")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.IsDifferent)
                    .IsRequired()
                    .HasColumnName("is_different")
                    .HasDefaultValueSql("'1'")
                    .HasComment("是否不同");

                entity.Property(e => e.IsSupply)
                    .HasColumnName("is_supply")
                    .HasComment("是否供应?");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("组织id");

                entity.Property(e => e.OrganizationName)
                    .HasColumnName("organization_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("组织名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ShouldCount)
                    .HasColumnName("should_count")
                    .HasColumnType("varchar(191)")
                    .HasComment("应检人数")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Sync)
                    .HasColumnName("sync")
                    .HasColumnType("tinyint(3) unsigned")
                    .HasComment("是否同步");

                entity.Property(e => e.SyncAt)
                    .HasColumnName("sync_at")
                    .HasColumnType("timestamp")
                    .HasComment("同步时间");

                entity.Property(e => e.TeamGroupId)
                    .HasColumnName("team_group_id")
                    .HasColumnType("int(11)")
                    .HasComment("班组id");

                entity.Property(e => e.TeamGroupName)
                    .HasColumnName("team_group_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("班组名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("tinyint(4)")
                    .HasComment("类型");

                entity.Property(e => e.UnqualifiedCount)
                    .HasColumnName("unqualified_count")
                    .HasColumnType("varchar(191)")
                    .HasComment("不合格数")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<Lights>(entity =>
            {
                entity.ToTable("lights");

                entity.HasIndex(e => e.CreateUserId)
                    .HasName("lights_create_user_id_index");

                entity.HasIndex(e => e.Del)
                    .HasName("lights_del_index");

                entity.HasIndex(e => e.LightId)
                    .HasName("lights_light_id_index");

                entity.HasIndex(e => e.Type)
                    .HasName("lights_type_index");

                entity.HasIndex(e => e.UpdateUserId)
                    .HasName("lights_update_user_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("create_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("创建用户");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Del)
                    .IsRequired()
                    .HasColumnName("del")
                    .HasDefaultValueSql("'1'")
                    .HasComment("软删除;");

                entity.Property(e => e.LightId)
                    .HasColumnName("light_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安消毒灯id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(191)")
                    .HasComment("消毒专间或消毒灯名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(11)")
                    .HasComment("众食安组织id");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("tinyint(4)")
                    .HasComment("类型;0-紫外线灯,1-消毒专间");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnName("update_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("更新用户");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.ToTable("logs");

                entity.HasIndex(e => e.CreatedAt)
                    .HasName("logs_created_at_index");

                entity.HasIndex(e => new { e.ObjectType, e.ObjectId })
                    .HasName("logs_object_type_object_id_unique")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp")
                    .HasComment("创建时间");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(191)")
                    .HasComment("日志名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("object_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ObjectType)
                    .IsRequired()
                    .HasColumnName("object_type")
                    .HasColumnType("varchar(191)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("'233259'");
            });

            modelBuilder.Entity<Migrations>(entity =>
            {
                entity.ToTable("migrations");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Batch)
                    .HasColumnName("batch")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Migration)
                    .IsRequired()
                    .HasColumnName("migration")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Monitors>(entity =>
            {
                entity.ToTable("monitors");

                entity.HasIndex(e => e.MonitorBranchId)
                    .HasName("monitors_monitor_branch_id_index");

                entity.HasIndex(e => e.MonitorNodeId)
                    .HasName("monitors_monitor_node_id_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("monitors_organization_id_index");

                entity.HasIndex(e => new { e.MonitorBranchId, e.MonitorNodeId })
                    .HasName("monitors_monitor_branch_id_monitor_node_id_unique")
                    .IsUnique();

                entity.HasIndex(e => new { e.SchoolName, e.Procedure })
                    .HasName("monitors_school_name_procedure_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.FullName)
                    .HasColumnName("full_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("组织名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.HlsUrl)
                    .HasColumnName("hls_url")
                    .HasColumnType("varchar(255)")
                    .HasComment("直播数据源地址")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.MonitorBranchId)
                    .HasColumnName("monitor_branch_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("分支id");

                entity.Property(e => e.MonitorName)
                    .HasColumnName("monitor_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("阳光云视名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.MonitorNodeId)
                    .HasColumnName("monitor_node_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("节点id");

                entity.Property(e => e.MonitorType)
                    .HasColumnName("monitor_type")
                    .HasColumnType("varchar(255)")
                    .HasComment("阳光云视类型")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("组织id");

                entity.Property(e => e.Procedure)
                    .HasColumnName("procedure")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.SchoolName)
                    .HasColumnName("school_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("varchar(255)")
                    .HasComment("摄像头名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<Orgs>(entity =>
            {
                entity.ToTable("orgs");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("orgs_organization_id_unique")
                    .IsUnique();

                entity.HasIndex(e => e.SchoolName)
                    .HasName("orgs_school_name_index");

                entity.HasIndex(e => e.UserId)
                    .HasName("orgs_user_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.DetailAddress)
                    .HasColumnName("detail_address")
                    .HasColumnType("varchar(255)")
                    .HasComment("详细地址")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.FullAddress)
                    .HasColumnName("full_address")
                    .HasColumnType("varchar(255)")
                    .HasComment("地址")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.IsSchool)
                    .HasColumnName("is_school")
                    .HasComment("是否为学校");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasComment("组织名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("组织id");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("varchar(255)")
                    .HasComment("密码")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Realname)
                    .HasColumnName("realname")
                    .HasColumnType("varchar(255)")
                    .HasComment("众食安用户姓名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.SchoolName)
                    .HasColumnName("school_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("学校名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安用户id");

                entity.Property(e => e.UserOrganizationId)
                    .HasColumnName("user_organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安该组织下用户id");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(255)")
                    .HasComment("用户名(登录账号)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Partners>(entity =>
            {
                entity.ToTable("partners");

                entity.HasIndex(e => e.AreaId)
                    .HasName("partners_area_id_index");

                entity.HasIndex(e => e.CityId)
                    .HasName("partners_city_id_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("partners_organization_id_index");

                entity.HasIndex(e => e.PartnerId)
                    .HasName("partners_partner_id_index");

                entity.HasIndex(e => e.ProvideId)
                    .HasName("partners_organization_provide_id_index");

                entity.HasIndex(e => e.ProvinceId)
                    .HasName("partners_province_id_index");

                entity.HasIndex(e => e.StreetId)
                    .HasName("partners_street_id_index");

                entity.HasIndex(e => e.TypeId)
                    .HasName("partners_type_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.AreaId)
                    .HasColumnName("area_id")
                    .HasColumnType("int(11)")
                    .HasComment("地区id");

                entity.Property(e => e.AreaName)
                    .HasColumnName("area_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("地区名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.BeginTime)
                    .HasColumnName("begin_time")
                    .HasColumnType("timestamp")
                    .HasComment("开始时间");

                entity.Property(e => e.CityId)
                    .HasColumnName("city_id")
                    .HasColumnType("int(11)")
                    .HasComment("城市id");

                entity.Property(e => e.CityName)
                    .HasColumnName("city_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("城市名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.DetailAddress)
                    .HasColumnName("detail_address")
                    .HasColumnType("varchar(191)")
                    .HasComment("详情地址")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.EndTime)
                    .HasColumnName("end_time")
                    .HasColumnType("timestamp")
                    .HasComment("结束时间");

                entity.Property(e => e.EnterpriseTypeName)
                    .HasColumnName("enterprise_type_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("企业类型名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.FullName)
                    .HasColumnName("full_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("接收单位名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Idcard)
                    .HasColumnName("idcard")
                    .HasColumnType("varchar(191)")
                    .HasComment("证件号")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.LicenseImg)
                    .HasColumnName("license_img")
                    .HasColumnType("varchar(191)")
                    .HasComment("经营证件照")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.LicenseNo)
                    .HasColumnName("license_no")
                    .HasColumnType("varchar(191)")
                    .HasComment("经营证件号")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.LicensePeriod)
                    .HasColumnName("license_period")
                    .HasColumnType("varchar(191)")
                    .HasComment("有效时间")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.MajorGoods)
                    .HasColumnName("major_goods")
                    .HasColumnType("varchar(191)")
                    .HasComment("主要负责对象")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.MajorGoodsIds)
                    .HasColumnName("major_goods_ids")
                    .HasColumnType("varchar(191)")
                    .HasComment("major_goods_id; 以逗号连接")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("varchar(191)")
                    .HasComment("说明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安组织id");

                entity.Property(e => e.PartnerId)
                    .HasColumnName("partner_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安接收单位id");

                entity.Property(e => e.PermitNumberNo)
                    .HasColumnName("permit_number_no")
                    .HasColumnType("varchar(191)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ProductionLicence)
                    .HasColumnName("production_licence")
                    .HasColumnType("varchar(191)")
                    .HasComment("产品经营许可证号")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ProvideId)
                    .HasColumnName("provide_id")
                    .HasColumnType("int(11)")
                    .HasComment("接收单位id");

                entity.Property(e => e.ProvideIdCode)
                    .HasColumnName("provide_id_code")
                    .HasColumnType("varchar(191)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ProvideIdCodeImg)
                    .HasColumnName("provide_id_code_img")
                    .HasColumnType("varchar(191)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ProvideName)
                    .HasColumnName("provide_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("供应商联系人名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ProvidePhone)
                    .HasColumnName("provide_phone")
                    .HasColumnType("varchar(191)")
                    .HasComment("供应商手机号")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ProvinceId)
                    .HasColumnName("province_id")
                    .HasColumnType("int(11)")
                    .HasComment("省份id");

                entity.Property(e => e.ProvinceName)
                    .HasColumnName("province_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("省份名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.StreetId)
                    .HasColumnName("street_id")
                    .HasColumnType("int(11)")
                    .HasComment("街道id？");

                entity.Property(e => e.StreetName)
                    .HasColumnName("street_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("街道名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Tel)
                    .HasColumnName("tel")
                    .HasColumnType("varchar(191)")
                    .HasComment("联系方式")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.TypeId)
                    .HasColumnName("type_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("类型id");

                entity.Property(e => e.TypeName)
                    .HasColumnName("type_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("单位类型")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Pesticides>(entity =>
            {
                entity.ToTable("pesticides");

                entity.HasIndex(e => e.CreateUserId)
                    .HasName("pesticides_create_user_id_index");

                entity.HasIndex(e => e.HandleMeasure)
                    .HasName("pesticides_handle_measure_index");

                entity.HasIndex(e => e.InspectionOrder)
                    .HasName("pesticides_inspection_order_index");

                entity.HasIndex(e => e.InspectionResult)
                    .HasName("pesticides_inspection_result_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("pesticides_organization_id_index");

                entity.HasIndex(e => e.PesticideId)
                    .HasName("pesticides_pesticide_id_index");

                entity.HasIndex(e => e.Sync)
                    .HasName("pesticides_sync_index");

                entity.HasIndex(e => e.UserOrganizationId)
                    .HasName("pesticides_user_organization_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CheckedAt)
                    .HasColumnName("checked_at")
                    .HasColumnType("timestamp")
                    .HasComment("检测时间");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("create_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("创建用户id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.HandleMeasure)
                    .HasColumnName("handle_measure")
                    .HasColumnType("tinyint(4)")
                    .HasComment("处理结果;0-正常使用,1-停用销毁");

                entity.Property(e => e.HandleMeasures)
                    .HasColumnName("handle_measures")
                    .HasColumnType("varchar(255)")
                    .HasComment("处理结果详细说明?")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.InspectionOrder)
                    .HasColumnName("inspection_order")
                    .HasColumnType("tinyint(4)")
                    .HasComment("检测次序;0-清洗前,1-清洗后一测,2-清洗后二测");

                entity.Property(e => e.InspectionOrders)
                    .HasColumnName("inspection_orders")
                    .HasColumnType("varchar(255)")
                    .HasComment("检测次序详细说明?")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.InspectionResult)
                    .HasColumnName("inspection_result")
                    .HasColumnType("tinyint(4)")
                    .HasComment("检测结果;0-阴性,1-阳性");

                entity.Property(e => e.InspectionResults)
                    .HasColumnName("inspection_results")
                    .HasColumnType("varchar(255)")
                    .HasComment("检测结果详细说明?")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Inspector)
                    .IsRequired()
                    .HasColumnName("inspector")
                    .HasColumnType("varchar(255)")
                    .HasComment("检测人员")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("text")
                    .HasComment("备注")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("组织id");

                entity.Property(e => e.PesticideId)
                    .HasColumnName("pesticide_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安农药残留日志id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)")
                    .HasComment("检测结果");

                entity.Property(e => e.Sync)
                    .HasColumnName("sync")
                    .HasColumnType("tinyint(3) unsigned")
                    .HasComment("是否同步");

                entity.Property(e => e.SyncAt)
                    .HasColumnName("sync_at")
                    .HasColumnType("timestamp")
                    .HasComment("同步时间");

                entity.Property(e => e.TestPaper)
                    .HasColumnName("test_paper")
                    .HasColumnType("text")
                    .HasComment("众食安检测结果图片")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("登记人员")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UserOrganizationId)
                    .HasColumnName("user_organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("登记人员组织id");

                entity.Property(e => e.Vegetables)
                    .HasColumnName("vegetables")
                    .HasColumnType("varchar(255)")
                    .HasComment("果蔬品名id")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.VegetablesName)
                    .HasColumnName("vegetables_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("果蔬品名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<PurchaseInformation>(entity =>
            {
                entity.ToTable("purchase_information");

                entity.HasIndex(e => e.CreateUserId)
                    .HasName("purcharse_information_create_user_id_index");

                entity.HasIndex(e => e.ExpireDate)
                    .HasName("purchase_information_guarantee_date_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("purcharse_information_organization_id_index");

                entity.HasIndex(e => e.OwnerPurchaseId)
                    .HasName("purcharse_information_owner_purchase_id_index");

                entity.HasIndex(e => e.ProductionDate)
                    .HasName("purchase_information_manufactured_date_index");

                entity.HasIndex(e => e.PurchaseId)
                    .HasName("purcharse_information_purchase_id_index");

                entity.HasIndex(e => e.PurchaseInformationId)
                    .HasName("purcharse_information_purchase_information_id_index");

                entity.HasIndex(e => e.Status)
                    .HasName("purchase_information_status_index");

                entity.HasIndex(e => e.SupplierId)
                    .HasName("purcharse_information_supplier_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("create_user_id")
                    .HasColumnType("int(11)")
                    .HasComment("创建用户");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp")
                    .HasComment("创建时间");

                entity.Property(e => e.ExpireDate)
                    .HasColumnName("expire_date")
                    .HasColumnType("date")
                    .HasComment("保质期");

                entity.Property(e => e.FoodName)
                    .HasColumnName("food_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("货品名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ModelName)
                    .HasColumnName("model_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("规格名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasColumnType("varchar(191)")
                    .HasComment("采购数量")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("组织id");

                entity.Property(e => e.OwnerPurchaseId)
                    .HasColumnName("owner_purchase_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("采购日志ID");

                entity.Property(e => e.ProductionDate)
                    .HasColumnName("production_date")
                    .HasColumnType("date")
                    .HasComment("生产日期");

                entity.Property(e => e.PurchaseId)
                    .HasColumnName("purchase_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安采购日志ID");

                entity.Property(e => e.PurchaseInformationId)
                    .HasColumnName("purchase_information_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安采购明细ID");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(4)")
                    .HasComment("状态");

                entity.Property(e => e.Supplier)
                    .HasColumnName("supplier")
                    .HasColumnType("varchar(191)")
                    .HasComment("供应商")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("supplier_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("供应商id");

                entity.Property(e => e.UnitName)
                    .HasColumnName("unit_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("单位名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Purchases>(entity =>
            {
                entity.ToTable("purchases");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("purchases_organization_id_index");

                entity.HasIndex(e => e.PurchaseDate)
                    .HasName("purchases_purchase_date_index");

                entity.HasIndex(e => e.PurchaseId)
                    .HasName("purchases_purchase_id_index");

                entity.HasIndex(e => e.RegisterDate)
                    .HasName("purchases_register_date_index");

                entity.HasIndex(e => e.Sync)
                    .HasName("purchases_sync_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("varchar(255)")
                    .HasComment("备注")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(11)")
                    .HasComment("组织id");

                entity.Property(e => e.OrganizationName)
                    .HasColumnName("organization_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("组织名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.PurchaseDate)
                    .HasColumnName("purchase_date")
                    .HasColumnType("varchar(255)")
                    .HasComment("采购日期")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.PurchaseId)
                    .HasColumnName("purchase_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安采购日志id");

                entity.Property(e => e.PurchaseUser)
                    .HasColumnName("purchase_user")
                    .HasColumnType("varchar(255)")
                    .HasComment("采购人员")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Register)
                    .HasColumnName("register")
                    .HasColumnType("varchar(255)")
                    .HasComment("登记人员")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.RegisterDate)
                    .HasColumnName("register_date")
                    .HasColumnType("varchar(255)")
                    .HasComment("登记日期")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.RegisterUserId)
                    .HasColumnName("register_user_id")
                    .HasColumnType("int(11)")
                    .HasComment("登记人员用户id;");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("varchar(255)")
                    .HasComment("状态")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Supplier)
                    .HasColumnName("supplier")
                    .HasColumnType("varchar(255)")
                    .HasComment("供应商")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Sync)
                    .HasColumnName("sync")
                    .HasColumnType("tinyint(3) unsigned")
                    .HasComment("是否同步");

                entity.Property(e => e.SyncAt)
                    .HasColumnName("sync_at")
                    .HasColumnType("timestamp")
                    .HasComment("同步时间");

                entity.Property(e => e.TicketImgs)
                    .IsRequired()
                    .HasColumnName("ticket_imgs")
                    .HasColumnType("text")
                    .HasComment("众食安采购票证")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("varchar(255)")
                    .HasComment("品种")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Types)
                    .HasColumnName("types")
                    .HasColumnType("varchar(255)")
                    .HasComment("品种ids,(以','连接) 同步到众食安须填写")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Samples>(entity =>
            {
                entity.ToTable("samples");

                entity.HasIndex(e => e.AuditorId)
                    .HasName("sample_auditor_id_index");

                entity.HasIndex(e => e.EliminateId)
                    .HasName("sample_eliminate_id_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("sample_organization_id_index");

                entity.HasIndex(e => e.SampleId)
                    .HasName("samples_sample_id_index");

                entity.HasIndex(e => e.Sync)
                    .HasName("samples_sync_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.AuditorId)
                    .HasColumnName("auditor_id")
                    .HasColumnType("int(11)")
                    .HasComment("审核员id");

                entity.Property(e => e.AuditorName)
                    .HasColumnName("auditor_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("审核员名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("create_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("创建用户id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Del)
                    .HasColumnName("del")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("是否销样");

                entity.Property(e => e.EliminateId)
                    .HasColumnName("eliminate_id")
                    .HasColumnType("int(11)")
                    .HasComment("销除留样方式");

                entity.Property(e => e.EliminateName)
                    .HasColumnName("eliminate_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("销除留样方式名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.EliminatedAt)
                    .HasColumnName("eliminated_at")
                    .HasColumnType("timestamp")
                    .HasComment("销样时间");

                entity.Property(e => e.FoodIds)
                    .HasColumnName("food_ids")
                    .HasColumnType("varchar(255)")
                    .HasComment("食品ids")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.FoodName)
                    .HasColumnName("food_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("食品名称集合")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Hours)
                    .HasColumnName("hours")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("留样冷藏小时");

                entity.Property(e => e.Img)
                    .HasColumnName("img")
                    .HasColumnType("varchar(255)")
                    .HasComment("留样照片")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.MaturedAt)
                    .HasColumnName("matured_at")
                    .HasColumnType("timestamp")
                    .HasComment("到期时间");

                entity.Property(e => e.MealTime)
                    .HasColumnName("meal_time")
                    .HasColumnType("int(11)")
                    .HasComment("留样餐次");

                entity.Property(e => e.MealTimeName)
                    .HasColumnName("meal_time_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("留样餐次名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("varchar(255)")
                    .HasComment("备注")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("组织id");

                entity.Property(e => e.SampleId)
                    .HasColumnName("sample_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安留样日志id");

                entity.Property(e => e.SampleName)
                    .HasColumnName("sample_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("留样人员")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.SampleUserId)
                    .HasColumnName("sample_user_id")
                    .HasColumnType("int(11)")
                    .HasComment("留样人员id");

                entity.Property(e => e.SampledAt)
                    .HasColumnName("sampled_at")
                    .HasColumnType("timestamp")
                    .HasComment("留样时间");

                entity.Property(e => e.Sync)
                    .HasColumnName("sync")
                    .HasColumnType("tinyint(3) unsigned")
                    .HasComment("是否同步");

                entity.Property(e => e.SyncAt)
                    .HasColumnName("sync_at")
                    .HasColumnType("timestamp")
                    .HasComment("同步时间");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Weight)
                    .HasColumnName("weight")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("留样重量/数量");
            });

            modelBuilder.Entity<Screenshots>(entity =>
            {
                entity.ToTable("screenshots");

                entity.HasIndex(e => e.CreatedAt)
                    .HasName("screenshots_created_at_index");

                entity.HasIndex(e => e.MonitorId)
                    .HasName("screenshots_monitor_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp")
                    .HasComment("创建时间");

                entity.Property(e => e.MonitorId)
                    .HasColumnName("monitor_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("摄像头id");

                entity.Property(e => e.Path)
                    .HasColumnName("path")
                    .HasColumnType("varchar(191)")
                    .HasComment("路径")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Synthesizes>(entity =>
            {
                entity.ToTable("synthesizes");

                entity.HasIndex(e => e.CreateUserId)
                    .HasName("synthesizes_create_user_id_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("synthesizes_organization_id_index");

                entity.HasIndex(e => e.RepersonId)
                    .HasName("synthesizes_reperson_id_index");

                entity.HasIndex(e => e.Status)
                    .HasName("synthesizes_status_index");

                entity.HasIndex(e => e.SubjectId)
                    .HasName("synthesizes_subject_id_index");

                entity.HasIndex(e => e.SynthesizeId)
                    .HasName("synthesizes_synthesize_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.ContinueTime)
                    .HasColumnName("continue_time")
                    .HasColumnType("int(11)")
                    .HasComment("持续时间");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("date");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("create_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("创建用户id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp")
                    .HasComment("创建时间");

                entity.Property(e => e.Department)
                    .HasColumnName("department")
                    .HasColumnType("varchar(191)")
                    .HasComment("参与部门id, 以逗号连接")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.DepartmentName)
                    .HasColumnName("department_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("参与部门名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Imgs)
                    .HasColumnName("imgs")
                    .HasColumnType("text")
                    .HasComment("活动照片,多张以逗号连接")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Introduce)
                    .HasColumnName("introduce")
                    .HasColumnType("varchar(191)")
                    .HasComment("活动介绍")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .HasColumnType("int(11)")
                    .HasComment("参与人数");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安组织id");

                entity.Property(e => e.Reperson)
                    .HasColumnName("reperson")
                    .HasColumnType("varchar(191)")
                    .HasComment("负责人名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.RepersonId)
                    .HasColumnName("reperson_id")
                    .HasColumnType("varchar(191)")
                    .HasComment("负责人用户id")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Result)
                    .HasColumnName("result")
                    .HasColumnType("varchar(255)")
                    .HasComment("备注说明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("subject_id")
                    .HasColumnType("varchar(191)")
                    .HasComment("主题id;types表中type为zhtzzt的type_id")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.SubjectName)
                    .HasColumnName("subject_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("主题名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Sync)
                    .HasColumnName("sync")
                    .HasColumnType("tinyint(3) unsigned")
                    .HasComment("是否启动");

                entity.Property(e => e.SyncAt)
                    .HasColumnName("sync_at")
                    .HasColumnType("timestamp")
                    .HasComment("同步时间");

                entity.Property(e => e.SynthesizeId)
                    .HasColumnName("synthesize_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安综合台账id");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnName("update_user_id")
                    .HasColumnType("int(11)")
                    .HasComment("更新用户");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp")
                    .HasComment("更新时间");
            });

            modelBuilder.Entity<Types>(entity =>
            {
                entity.ToTable("types");

                entity.HasIndex(e => e.Status)
                    .HasName("goods_types_status_index");

                entity.HasIndex(e => e.Type)
                    .HasName("types_type_index");

                entity.HasIndex(e => e.TypeId)
                    .HasName("goods_types_goods_type_id_unique")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(191)")
                    .HasComment("描述")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.KeyName)
                    .HasColumnName("key_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.KeyValue)
                    .HasColumnName("key_value")
                    .HasColumnType("varchar(191)")
                    .HasComment("value值")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("状态;1-开启？");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("varchar(191)")
                    .HasComment("父级拼音首字母")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.TypeId)
                    .HasColumnName("type_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安货品分类id");

                entity.Property(e => e.TypeName)
                    .HasColumnName("type_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("父级描述")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.DepartmentId)
                    .HasName("users_department_id_index");

                entity.HasIndex(e => e.DutyId)
                    .HasName("users_duty_id_index");

                entity.HasIndex(e => e.GroupId)
                    .HasName("users_group_id_index");

                entity.HasIndex(e => e.Nbunber)
                    .HasName("users_nbunber_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("users_organization_id_index");

                entity.HasIndex(e => e.ParentId)
                    .HasName("users_parent_id_index");

                entity.HasIndex(e => e.Seniority)
                    .HasName("users_seniority_index");

                entity.HasIndex(e => e.Sex)
                    .HasName("users_sex_index");

                entity.HasIndex(e => e.Sync)
                    .HasName("users_sync_index");

                entity.HasIndex(e => e.SysUserId)
                    .HasName("users_sys_user_id_index");

                entity.HasIndex(e => e.TeamGroupId)
                    .HasName("users_team_group_id_index");

                entity.HasIndex(e => e.UserId)
                    .HasName("users_user_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Activation)
                    .HasColumnName("activation")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("是否激活");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("varchar(191)")
                    .HasComment("出生日期")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ChargeArea)
                    .HasColumnName("charge_area")
                    .HasColumnType("varchar(191)")
                    .HasComment("收费地区?")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("varchar(191)")
                    .HasComment("未知")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("create_user_id")
                    .HasColumnType("int(11)")
                    .HasComment("创建人id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("department_id")
                    .HasColumnType("int(11)")
                    .HasComment("部门id");

                entity.Property(e => e.DutyId)
                    .HasColumnName("duty_id")
                    .HasColumnType("int(11)")
                    .HasComment("职位id");

                entity.Property(e => e.DutyName)
                    .HasColumnName("duty_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("职位名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.EducationId)
                    .HasColumnName("education_id")
                    .HasColumnType("int(11)")
                    .HasComment("文化程度");

                entity.Property(e => e.ElecHealthCertificateCount)
                    .HasColumnName("elec_health_certificate_count")
                    .HasColumnType("int(11)")
                    .HasComment("电子健康证明数量");

                entity.Property(e => e.EntryDate)
                    .HasColumnName("entry_date")
                    .HasColumnType("date")
                    .HasComment("录入时间");

                entity.Property(e => e.ExaminationAt)
                    .HasColumnName("examination_at")
                    .HasColumnType("timestamp")
                    .HasComment("检查时间");

                entity.Property(e => e.GroupId)
                    .HasColumnName("group_id")
                    .HasColumnType("int(11)")
                    .HasComment("班组id");

                entity.Property(e => e.HashValue)
                    .HasColumnName("hash_value")
                    .HasColumnType("char(32)")
                    .HasDefaultValueSql("''")
                    .HasComment("用来判断数据是否更新")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.HealthBeginDate)
                    .HasColumnName("health_begin_date")
                    .HasColumnType("date")
                    .HasComment("健康证有效范围开始日期");

                entity.Property(e => e.HealthCodeAt)
                    .HasColumnName("health_code_at")
                    .HasColumnType("timestamp")
                    .HasComment("未知");

                entity.Property(e => e.HealthEndDate)
                    .HasColumnName("health_end_date")
                    .HasColumnType("date")
                    .HasComment("健康证有效范围结束日期");

                entity.Property(e => e.HealthFrom)
                    .HasColumnName("health_from")
                    .HasColumnType("varchar(191)")
                    .HasComment("发证机构")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.HealthImg)
                    .HasColumnName("health_img")
                    .HasColumnType("varchar(191)")
                    .HasComment("健康证照片")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.HealthNum)
                    .HasColumnName("health_num")
                    .HasColumnType("varchar(191)")
                    .HasComment("健康证编号")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.HealthOverdue)
                    .HasColumnName("health_overdue")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("是否过期");

                entity.Property(e => e.HealthStatus)
                    .HasColumnName("health_status")
                    .HasColumnType("tinyint(4)")
                    .HasComment("健康证状态");

                entity.Property(e => e.HealthTypeId)
                    .HasColumnName("health_type_id")
                    .HasColumnType("int(11)")
                    .HasComment("健康证类型");

                entity.Property(e => e.Icons)
                    .HasColumnName("icons")
                    .HasColumnType("varchar(191)")
                    .HasComment("图标")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Idcard)
                    .HasColumnName("idcard")
                    .HasColumnType("varchar(191)")
                    .HasComment("身份证")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.IdcardImg)
                    .HasColumnName("idcard_img")
                    .HasColumnType("varchar(191)")
                    .HasComment("身份证照片")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.IsFinal)
                    .HasColumnName("is_final")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("是否完成?");

                entity.Property(e => e.LoginName)
                    .HasColumnName("login_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("登录账号?")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Mdz)
                    .HasColumnName("mdz")
                    .HasColumnType("varchar(191)")
                    .HasComment("未知")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Mzt)
                    .HasColumnName("mzt")
                    .HasColumnType("varchar(191)")
                    .HasComment("健康码状态")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(191)")
                    .HasComment("姓名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Nbunber)
                    .HasColumnName("nbunber")
                    .HasColumnType("int(11)")
                    .HasComment("未知");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("varchar(191)")
                    .HasComment("备注")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OcrResult)
                    .HasColumnName("ocr_result")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安组织id");

                entity.Property(e => e.OrganizationType)
                    .HasColumnName("organization_type")
                    .HasColumnType("varchar(191)")
                    .HasComment("组织类型?")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OverdueNum)
                    .HasColumnName("overdue_num")
                    .HasColumnType("int(11)")
                    .HasComment("逾期数");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parent_id")
                    .HasColumnType("int(11)")
                    .HasComment("父级id");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(191)")
                    .HasComment("联系方式")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Rank)
                    .HasColumnName("rank")
                    .HasColumnType("int(11)")
                    .HasComment("未知");

                entity.Property(e => e.RegionId)
                    .HasColumnName("region_id")
                    .HasColumnType("int(11)")
                    .HasComment("地区id");

                entity.Property(e => e.Seniority)
                    .HasColumnName("seniority")
                    .HasColumnType("int(11)")
                    .HasComment("排序,从高到底");

                entity.Property(e => e.Sex)
                    .HasColumnName("sex")
                    .HasColumnType("tinyint(4)")
                    .HasComment("性别");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'")
                    .HasComment("是否显示?");

                entity.Property(e => e.SuperviseOrganization)
                    .HasColumnName("supervise_organization")
                    .HasColumnType("varchar(191)")
                    .HasComment("监督组织?")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Sync)
                    .HasColumnName("sync")
                    .HasComment("是否同步");

                entity.Property(e => e.SyncAt)
                    .HasColumnName("sync_at")
                    .HasColumnType("timestamp")
                    .HasComment("同步时间");

                entity.Property(e => e.SysUserId)
                    .HasColumnName("sys_user_id")
                    .HasColumnType("int(11)")
                    .HasComment("系统下用户id");

                entity.Property(e => e.TeamGroupId)
                    .HasColumnName("team_group_id")
                    .HasColumnType("int(11)")
                    .HasComment("班组id");

                entity.Property(e => e.TeamGroupName)
                    .HasColumnName("team_group_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("班组名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.TrainAt)
                    .HasColumnName("train_at")
                    .HasColumnType("timestamp")
                    .HasComment("培训时间");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("该组织下用户id");

                entity.Property(e => e.WarnAt)
                    .HasColumnName("warn_at")
                    .HasColumnType("timestamp")
                    .HasComment("警告时间?");

                entity.Property(e => e.WarnStatus)
                    .HasColumnName("warn_status")
                    .HasColumnType("tinyint(4)")
                    .HasComment("警告状态?");

                entity.Property(e => e.Work)
                    .HasColumnName("work")
                    .HasColumnType("tinyint(4)")
                    .HasComment("是否在岗");
            });

            modelBuilder.Entity<Vegetables>(entity =>
            {
                entity.ToTable("vegetables");

                entity.HasIndex(e => e.CreateUserId)
                    .HasName("vegetables_create_user_id_index");

                entity.HasIndex(e => e.Del)
                    .HasName("vegetables_del_index");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("vegetables_organization_id_index");

                entity.HasIndex(e => e.TypeId)
                    .HasName("vegetables_goods_type_id_index");

                entity.HasIndex(e => e.UpdateUserId)
                    .HasName("vegetables_update_user_id_index");

                entity.HasIndex(e => e.VegetableId)
                    .HasName("vegetables_vegetable_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("create_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安创建人用户id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Del)
                    .IsRequired()
                    .HasColumnName("del")
                    .HasDefaultValueSql("'1'")
                    .HasComment("是否软删;0-软删，1-正常");

                entity.Property(e => e.Explain)
                    .HasColumnName("explain")
                    .HasColumnType("varchar(191)")
                    .HasComment("说明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(191)")
                    .HasComment("蔬菜明")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安组织id");

                entity.Property(e => e.TypeId)
                    .HasColumnName("type_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安分类id");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnName("update_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安创建人用户id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.VegetableId)
                    .HasColumnName("vegetable_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("众食安蔬菜列表");
            });

            modelBuilder.Entity<Wastes>(entity =>
            {
                entity.ToTable("wastes");

                entity.HasIndex(e => e.OrganizationId)
                    .HasName("wastes_organization_id_index");

                entity.HasIndex(e => e.Status)
                    .HasName("wastes_status_index");

                entity.HasIndex(e => e.Sync)
                    .HasName("wastes_sync_index");

                entity.HasIndex(e => e.WasteId)
                    .HasName("wastes_waste_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("timestamp")
                    .HasComment("创建时间");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("create_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("创建人id");

                entity.Property(e => e.FullName)
                    .HasColumnName("full_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("接收单位")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.HandleDate)
                    .HasColumnName("handle_date")
                    .HasColumnType("timestamp")
                    .HasComment("处置日期");

                entity.Property(e => e.HandlerId)
                    .HasColumnName("handler_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasDefaultValueSql("'0'")
                    .HasComment("处置id");

                entity.Property(e => e.HandlerName)
                    .HasColumnName("handler_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("处置人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Imgs)
                    .HasColumnName("imgs")
                    .HasColumnType("varchar(191)")
                    .HasComment("处理场景图")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.IsSupply)
                    .IsRequired()
                    .HasColumnName("is_supply")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.NewHandlerName)
                    .HasColumnName("new_handler_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("交接负责人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("varchar(191)")
                    .HasComment("备注")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("organization_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("组织id");

                entity.Property(e => e.OtherWasteNumber)
                    .HasColumnName("other_waste_number")
                    .HasColumnType("decimal(8,2) unsigned")
                    .HasComment("其他废弃物数量");

                entity.Property(e => e.Receiver)
                    .HasColumnName("receiver")
                    .HasColumnType("varchar(255)")
                    .HasComment("接收人员")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ReceiverCompanyId)
                    .HasColumnName("receiver_company_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("接收单位id");

                entity.Property(e => e.ReceiverCompanyName)
                    .HasColumnName("receiver_company_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("接收名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ReceiverIdentityCard)
                    .HasColumnName("receiver_identity_card")
                    .HasColumnType("varchar(191)")
                    .HasComment("接收人身份证")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)")
                    .HasComment("废弃物状态");

                entity.Property(e => e.SwillNumber)
                    .HasColumnName("swill_number")
                    .HasColumnType("decimal(8,2) unsigned")
                    .HasComment("泔水数量");

                entity.Property(e => e.Sync)
                    .HasColumnName("sync")
                    .HasColumnType("tinyint(3) unsigned")
                    .HasComment("是否同步");

                entity.Property(e => e.SyncAt)
                    .HasColumnName("sync_at")
                    .HasColumnType("timestamp")
                    .HasComment("同步时间");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasColumnType("timestamp")
                    .HasComment("更新时间");

                entity.Property(e => e.UpdateUser)
                    .HasColumnName("update_user")
                    .HasColumnType("varchar(191)")
                    .HasComment("更新数据用户")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.WasteId)
                    .HasColumnName("waste_id")
                    .HasColumnType("bigint(20) unsigned")
                    .HasComment("众食安废弃物日志id");

                entity.Property(e => e.WasteoilNumber)
                    .HasColumnName("wasteoil_number")
                    .HasColumnType("decimal(8,2) unsigned")
                    .HasComment("废油数量");
            });

            modelBuilder.Entity<WorldBarnBuyers>(entity =>
            {
                entity.ToTable("world_barn_buyers");

                entity.HasIndex(e => e.EntryId)
                    .HasName("world_barn_buyers_entry_id_index");

                entity.HasIndex(e => e.WorldBarnUserId)
                    .HasName("world_barn_buyers_world_barn_user_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.BuyerName)
                    .IsRequired()
                    .HasColumnName("buyer_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("买方名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.BuyerOrderShortname)
                    .IsRequired()
                    .HasColumnName("buyer_order_shortname")
                    .HasColumnType("varchar(255)")
                    .HasComment("买方简称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.BuyerShortname)
                    .IsRequired()
                    .HasColumnName("buyer_shortname")
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''")
                    .HasComment("买方简称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.DeliveryTime)
                    .HasColumnName("delivery_time")
                    .HasColumnType("int(11)")
                    .HasComment("待交货数?");

                entity.Property(e => e.EntryId)
                    .HasColumnName("entry_id")
                    .HasColumnType("int(11)")
                    .HasComment("条目id");

                entity.Property(e => e.WorldBarnUserId)
                    .HasColumnName("world_barn_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("天下良仓用户id");
            });

            modelBuilder.Entity<WorldBarnGoods>(entity =>
            {
                entity.ToTable("world_barn_goods");

                entity.HasIndex(e => e.BigClassId)
                    .HasName("world_barn_goods_big_class_id_index");

                entity.HasIndex(e => e.RsId)
                    .HasName("world_barn_goods_rs_id_index");

                entity.HasIndex(e => e.SmallClassId)
                    .HasName("world_barn_goods_small_class_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.BigClassId)
                    .HasColumnName("big_class_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("大类");

                entity.Property(e => e.GoodsAlias)
                    .HasColumnName("goods_alias")
                    .HasColumnType("varchar(191)")
                    .HasComment("商品别称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.GoodsCode)
                    .HasColumnName("goods_code")
                    .HasColumnType("varchar(191)")
                    .HasComment("商品编码")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.GoodsDesc)
                    .IsRequired()
                    .HasColumnName("goods_desc")
                    .HasColumnType("text")
                    .HasComment("商品描述")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.GoodsName)
                    .IsRequired()
                    .HasColumnName("goods_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("商品名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.RsId)
                    .HasColumnName("rs_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.RsMemo)
                    .HasColumnName("rs_memo")
                    .HasColumnType("varchar(191)")
                    .HasComment("商品图片")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.SmallClassId)
                    .HasColumnName("small_class_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("小类");

                entity.Property(e => e.Spec)
                    .HasColumnName("spec")
                    .HasColumnType("varchar(191)")
                    .HasComment("规格")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UnitCode)
                    .HasColumnName("unit_code")
                    .HasColumnType("varchar(191)")
                    .HasComment("单位")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<WorldBarnGoodsCategories>(entity =>
            {
                entity.ToTable("world_barn_goods_categories");

                entity.HasIndex(e => e.ParentId)
                    .HasName("world_barn_goods_categories_parent_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(191)")
                    .HasComment("分类名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parent_id")
                    .HasColumnType("int(11)")
                    .HasComment("父级id");
            });

            modelBuilder.Entity<WorldBarnOrderItems>(entity =>
            {
                entity.ToTable("world_barn_order_items");

                entity.HasIndex(e => e.GoodsId)
                    .HasName("world_barn_order_items_goods_id_index");

                entity.HasIndex(e => e.OrderId)
                    .HasName("world_barn_order_items_order_id_index");

                entity.HasIndex(e => e.OrderType)
                    .HasName("world_barn_order_items_order_type_index");

                entity.HasIndex(e => e.SupplierId)
                    .HasName("world_barn_order_items_supplier_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CompareStatus)
                    .HasColumnName("compare_status")
                    .HasColumnType("varchar(191)")
                    .HasComment("价格对比")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ConfirmGoodsDate)
                    .HasColumnName("confirm_goods_date")
                    .HasColumnType("timestamp")
                    .HasComment("确认商品时间");

                entity.Property(e => e.DemandedQuantity)
                    .HasColumnName("demanded_quantity")
                    .HasColumnType("decimal(10,2) unsigned")
                    .HasComment("需求数量");

                entity.Property(e => e.GoodsId)
                    .HasColumnName("goods_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("商品id");

                entity.Property(e => e.GoodsName)
                    .IsRequired()
                    .HasColumnName("goods_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("商品名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.LastOrderPrice)
                    .HasColumnName("last_order_price")
                    .HasColumnType("varchar(191)")
                    .HasComment("上期价格")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.LastOrderPriceDate)
                    .HasColumnName("last_order_price_date")
                    .HasColumnType("date")
                    .HasComment("上期价格时间");

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasColumnType("varchar(191)")
                    .HasComment("模式")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("varchar(191)")
                    .HasComment("备注")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasColumnName("order_id")
                    .HasColumnType("varchar(191)")
                    .HasComment("订单id")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrderType)
                    .HasColumnName("order_type")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("订单类型");

                entity.Property(e => e.RealPrice)
                    .HasColumnName("real_price")
                    .HasColumnType("decimal(10,2) unsigned")
                    .HasComment("最终价");

                entity.Property(e => e.RealQuantity)
                    .HasColumnName("real_quantity")
                    .HasColumnType("decimal(10,2) unsigned")
                    .HasComment("实收数量");

                entity.Property(e => e.Subtotal)
                    .HasColumnName("subtotal")
                    .HasColumnType("decimal(10,2) unsigned")
                    .HasComment("小计");

                entity.Property(e => e.SupplierId)
                    .HasColumnName("supplier_id")
                    .HasColumnType("int(10) unsigned")
                    .HasDefaultValueSql("'0'")
                    .HasComment("供应商id");

                entity.Property(e => e.SupplierName)
                    .IsRequired()
                    .HasColumnName("supplier_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("供应商名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasColumnName("unit")
                    .HasColumnType("varchar(191)")
                    .HasComment("单位")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("unit_price")
                    .HasColumnType("decimal(8,2) unsigned")
                    .HasComment("数量");
            });

            modelBuilder.Entity<WorldBarnOrders>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("world_barn_orders");

                entity.HasIndex(e => e.BuyerCode)
                    .HasName("world_barn_orders_buyer_code_index");

                entity.HasIndex(e => e.BuyerId)
                    .HasName("world_barn_orders_buyer_id_index");

                entity.HasIndex(e => e.CompanyId)
                    .HasName("world_barn_orders_company_id_index");

                entity.HasIndex(e => e.CurCustomerId)
                    .HasName("world_barn_orders_cur_customer_id_index");

                entity.HasIndex(e => e.CurOrderStatus)
                    .HasName("world_barn_orders_cur_order_status_index");

                entity.HasIndex(e => e.EntryId)
                    .HasName("world_barn_orders_entry_id_index");

                entity.HasIndex(e => e.IsEvaluation)
                    .HasName("world_barn_orders_is_evaluation_index");

                entity.HasIndex(e => e.LastModifyUserId)
                    .HasName("world_barn_orders_last_modify_user_id_index");

                entity.HasIndex(e => e.LineId)
                    .HasName("world_barn_orders_line_id_index");

                entity.HasIndex(e => e.ModifyBelongType)
                    .HasName("world_barn_orders_modify_belong_type_index");

                entity.HasIndex(e => e.OrderId)
                    .HasName("world_barn_orders_order_id_unique")
                    .IsUnique();

                entity.HasIndex(e => e.PaymentStatus)
                    .HasName("world_barn_orders_payment_status_index");

                entity.HasIndex(e => e.PrintId)
                    .HasName("world_barn_orders_print_id_index");

                entity.HasIndex(e => e.Status)
                    .HasName("world_barn_orders_status_index");

                entity.Property(e => e.BuyerCode)
                    .HasColumnName("buyer_code")
                    .HasColumnType("varchar(191)")
                    .HasComment("买方code")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.BuyerId)
                    .HasColumnName("buyer_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("买方id");

                entity.Property(e => e.BuyerIds)
                    .HasColumnName("buyer_ids")
                    .HasColumnType("varchar(191)")
                    .HasComment("买方id集合?")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.BuyerInfo)
                    .HasColumnName("buyer_info")
                    .HasColumnType("varchar(191)")
                    .HasComment("买方信息集合")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.BuyerName)
                    .HasColumnName("buyer_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("买方名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.BuyerNames)
                    .HasColumnName("buyer_names")
                    .HasColumnType("varchar(191)")
                    .HasComment("买方名称集合")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ChineseTotalAmount)
                    .HasColumnName("chinese_total_amount")
                    .HasColumnType("varchar(191)")
                    .HasComment("中文大写金额")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("company_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("公司id");

                entity.Property(e => e.ContactPerson)
                    .HasColumnName("contact_person")
                    .HasColumnType("varchar(191)")
                    .HasComment("联系人")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("timestamp")
                    .HasComment("创建时间");

                entity.Property(e => e.CurCustomerId)
                    .HasColumnName("cur_customer_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("当前顾客id");

                entity.Property(e => e.CurCustomerName)
                    .HasColumnName("cur_customer_name")
                    .HasColumnType("varchar(191)")
                    .HasComment("当前顾客名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CurOrderStatus)
                    .HasColumnName("cur_order_status")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'")
                    .HasComment("当前订单状态");

                entity.Property(e => e.CustomerDeliveryDate)
                    .HasColumnName("customer_delivery_date")
                    .HasColumnType("timestamp")
                    .HasComment("收货时间");

                entity.Property(e => e.CustomerDeliveryTime)
                    .HasColumnName("customer_delivery_time")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("收货次数?");

                entity.Property(e => e.CustomerDeliveryTimeValue)
                    .HasColumnName("customer_delivery_time_value")
                    .HasColumnType("varchar(191)")
                    .HasComment("收货次数值?")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.DayOrderNum)
                    .HasColumnName("day_order_num")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("天订单数?");

                entity.Property(e => e.DeliveryDate)
                    .HasColumnName("delivery_date")
                    .HasColumnType("timestamp")
                    .HasComment("发货日期");

                entity.Property(e => e.EntryId)
                    .HasColumnName("entry_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("条目id");

                entity.Property(e => e.GoodsClassList)
                    .HasColumnName("goods_class_list")
                    .HasColumnType("text")
                    .HasComment("商品类目集合")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.GoodsNum)
                    .HasColumnName("goods_num")
                    .HasColumnType("int(11)")
                    .HasComment("商品数");

                entity.Property(e => e.IsEvaluation)
                    .HasColumnName("is_evaluation")
                    .HasComment("是否评价");

                entity.Property(e => e.LastModifyDate)
                    .HasColumnName("last_modify_date")
                    .HasColumnType("timestamp")
                    .HasComment("最后修改时间");

                entity.Property(e => e.LastModifyUserId)
                    .HasColumnName("last_modify_user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("最后修改订单用户id");

                entity.Property(e => e.LastModifyUsername)
                    .HasColumnName("last_modify_username")
                    .HasColumnType("varchar(191)")
                    .HasComment("最后修改订单用户名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.LineId)
                    .HasColumnName("line_id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.LineName)
                    .HasColumnName("line_name")
                    .HasColumnType("varchar(191)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Mobile)
                    .HasColumnName("mobile")
                    .HasColumnType("varchar(191)")
                    .HasDefaultValueSql("''")
                    .HasComment("手机号")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ModifyBelongType)
                    .HasColumnName("modify_belong_type")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("修改人类型");

                entity.Property(e => e.OldRealAmount)
                    .HasColumnName("old_real_amount")
                    .HasColumnType("decimal(10,4)")
                    .HasComment("旧真实金额");

                entity.Property(e => e.OrderCancelReason)
                    .IsRequired()
                    .HasColumnName("order_cancel_reason")
                    .HasColumnType("varchar(191)")
                    .HasDefaultValueSql("''")
                    .HasComment("订单取消理由")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrderDetail)
                    .HasColumnName("order_detail")
                    .HasColumnType("text")
                    .HasComment("订单详情")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasColumnName("order_id")
                    .HasColumnType("varchar(191)")
                    .HasComment("订单id")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.OrderPrintDate)
                    .HasColumnName("order_print_date")
                    .HasColumnType("timestamp")
                    .HasComment("订单打印时间");

                entity.Property(e => e.PaymentStatus)
                    .HasColumnName("payment_status")
                    .HasColumnType("tinyint(4)")
                    .HasComment("订单付款状态");

                entity.Property(e => e.PrintCode)
                    .IsRequired()
                    .HasColumnName("print_code")
                    .HasColumnType("varchar(191)")
                    .HasDefaultValueSql("''")
                    .HasComment("打印code")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.PrintId)
                    .HasColumnName("print_id")
                    .HasColumnType("int(11)")
                    .HasComment("打印id");

                entity.Property(e => e.QrCode)
                    .IsRequired()
                    .HasColumnName("qr_code")
                    .HasColumnType("varchar(191)")
                    .HasDefaultValueSql("''")
                    .HasComment("二维码")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.RealAmount)
                    .HasColumnName("real_amount")
                    .HasColumnType("decimal(10,4) unsigned")
                    .HasComment("真实金额");

                entity.Property(e => e.RelOrderId)
                    .IsRequired()
                    .HasColumnName("rel_order_id")
                    .HasColumnType("varchar(191)")
                    .HasDefaultValueSql("''")
                    .HasComment("相关订单id")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.RelOrderType)
                    .HasColumnName("rel_order_type")
                    .HasColumnType("varchar(191)")
                    .HasComment("相关订单类型")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Remark)
                    .IsRequired()
                    .HasColumnName("remark")
                    .HasColumnType("varchar(191)")
                    .HasDefaultValueSql("''")
                    .HasComment("备注")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ServiceLicenseNo)
                    .HasColumnName("service_license_no")
                    .HasColumnType("varchar(191)")
                    .HasComment("服务商经营许可证件号")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Source)
                    .HasColumnName("source")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(11)")
                    .HasComment("-1临时保存,0已提交,7待录价,8交易完成,9取消");

                entity.Property(e => e.TotalAmount)
                    .HasColumnName("total_amount")
                    .HasColumnType("decimal(10,4)")
                    .HasComment("订单总金额");
            });

            modelBuilder.Entity<WorldBarnUsers>(entity =>
            {
                entity.ToTable("world_barn_users");

                entity.HasIndex(e => e.SchoolName)
                    .HasName("world_barn_users_school_name_index");

                entity.HasIndex(e => e.Username)
                    .HasName("world_barn_users_username_unique")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(255)")
                    .HasComment("密码")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.SchoolName)
                    .HasColumnName("school_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("学校名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(255)")
                    .HasComment("用户名")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
