namespace HaiKanTaskHousekeeperCore.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MSZDJ : DbContext
    {
        public MSZDJ()
            : base("name=MSZDJ")
        {
        }

        public virtual DbSet<departments> departments { get; set; }
        public virtual DbSet<disinfection_records> disinfection_records { get; set; }
        public virtual DbSet<disinfections> disinfections { get; set; }
        public virtual DbSet<inspection_information> inspection_information { get; set; }
        public virtual DbSet<inspections> inspections { get; set; }
        public virtual DbSet<logs> logs { get; set; }
        public virtual DbSet<migrations> migrations { get; set; }
        public virtual DbSet<monitors> monitors { get; set; }
        public virtual DbSet<orgs> orgs { get; set; }
        public virtual DbSet<pesticides> pesticides { get; set; }
        public virtual DbSet<purchases> purchases { get; set; }
        public virtual DbSet<samples> samples { get; set; }
        public virtual DbSet<screenshots> screenshots { get; set; }
        public virtual DbSet<world_barn_buyers> world_barn_buyers { get; set; }
        public virtual DbSet<world_barn_goods> world_barn_goods { get; set; }
        public virtual DbSet<world_barn_goods_categories> world_barn_goods_categories { get; set; }
        public virtual DbSet<world_barn_order_items> world_barn_order_items { get; set; }
        public virtual DbSet<world_barn_users> world_barn_users { get; set; }
        public virtual DbSet<world_barn_orders> world_barn_orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<departments>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<disinfection_records>()
                .Property(e => e.type_name)
                .IsUnicode(false);

            modelBuilder.Entity<disinfection_records>()
                .Property(e => e.method)
                .IsUnicode(false);

            modelBuilder.Entity<disinfection_records>()
                .Property(e => e.room_name)
                .IsUnicode(false);

            modelBuilder.Entity<disinfection_records>()
                .Property(e => e.tool_name)
                .IsUnicode(false);

            modelBuilder.Entity<disinfections>()
                .Property(e => e.method)
                .IsUnicode(false);

            modelBuilder.Entity<disinfections>()
                .Property(e => e.disinfection)
                .IsUnicode(false);

            modelBuilder.Entity<disinfections>()
                .Property(e => e.type_name)
                .IsUnicode(false);

            modelBuilder.Entity<disinfections>()
                .Property(e => e.img)
                .IsUnicode(false);

            modelBuilder.Entity<disinfections>()
                .Property(e => e.img_urls)
                .IsUnicode(false);

            modelBuilder.Entity<disinfections>()
                .Property(e => e.disinfection_user_name)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.user_name)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.card_number)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.duty)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.check_status)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.attendance)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.attendance_time)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.prosess_info)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.img_url)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.ai_face_img)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.temperature_img)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.hand_img)
                .IsUnicode(false);

            modelBuilder.Entity<inspection_information>()
                .Property(e => e.temperature)
                .IsUnicode(false);

            modelBuilder.Entity<inspections>()
                .Property(e => e.organization_name)
                .IsUnicode(false);

            modelBuilder.Entity<inspections>()
                .Property(e => e.should_count)
                .IsUnicode(false);

            modelBuilder.Entity<inspections>()
                .Property(e => e.actual_count)
                .IsUnicode(false);

            modelBuilder.Entity<inspections>()
                .Property(e => e.unqualified_count)
                .IsUnicode(false);

            modelBuilder.Entity<inspections>()
                .Property(e => e.inspector)
                .IsUnicode(false);

            modelBuilder.Entity<logs>()
                .Property(e => e.object_type)
                .IsUnicode(false);

            modelBuilder.Entity<logs>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<migrations>()
                .Property(e => e.migration)
                .IsUnicode(false);

            modelBuilder.Entity<monitors>()
                .Property(e => e.full_name)
                .IsUnicode(false);

            modelBuilder.Entity<monitors>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<monitors>()
                .Property(e => e.monitor_name)
                .IsUnicode(false);

            modelBuilder.Entity<monitors>()
                .Property(e => e.monitor_type)
                .IsUnicode(false);

            modelBuilder.Entity<monitors>()
                .Property(e => e.hls_url)
                .IsUnicode(false);

            modelBuilder.Entity<orgs>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<orgs>()
                .Property(e => e.school_name)
                .IsUnicode(false);

            modelBuilder.Entity<orgs>()
                .Property(e => e.detail_address)
                .IsUnicode(false);

            modelBuilder.Entity<orgs>()
                .Property(e => e.full_address)
                .IsUnicode(false);

            modelBuilder.Entity<orgs>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<orgs>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<pesticides>()
                .Property(e => e.user_name)
                .IsUnicode(false);

            modelBuilder.Entity<pesticides>()
                .Property(e => e.inspector)
                .IsUnicode(false);

            modelBuilder.Entity<pesticides>()
                .Property(e => e.test_paper)
                .IsUnicode(false);

            modelBuilder.Entity<pesticides>()
                .Property(e => e.vegetables_name)
                .IsUnicode(false);

            modelBuilder.Entity<pesticides>()
                .Property(e => e.vegetables)
                .IsUnicode(false);

            modelBuilder.Entity<pesticides>()
                .Property(e => e.inspection_orders)
                .IsUnicode(false);

            modelBuilder.Entity<pesticides>()
                .Property(e => e.inspection_results)
                .IsUnicode(false);

            modelBuilder.Entity<pesticides>()
                .Property(e => e.handle_measures)
                .IsUnicode(false);

            modelBuilder.Entity<pesticides>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<purchases>()
                .Property(e => e.organization_name)
                .IsUnicode(false);

            modelBuilder.Entity<purchases>()
                .Property(e => e.register)
                .IsUnicode(false);

            modelBuilder.Entity<purchases>()
                .Property(e => e.register_date)
                .IsUnicode(false);

            modelBuilder.Entity<purchases>()
                .Property(e => e.purchase_user)
                .IsUnicode(false);

            modelBuilder.Entity<purchases>()
                .Property(e => e.purchase_date)
                .IsUnicode(false);

            modelBuilder.Entity<purchases>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<purchases>()
                .Property(e => e.supplier)
                .IsUnicode(false);

            modelBuilder.Entity<purchases>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<purchases>()
                .Property(e => e.ticket_imgs)
                .IsUnicode(false);

            modelBuilder.Entity<purchases>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<samples>()
                .Property(e => e.food_ids)
                .IsUnicode(false);

            modelBuilder.Entity<samples>()
                .Property(e => e.food_name)
                .IsUnicode(false);

            modelBuilder.Entity<samples>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<samples>()
                .Property(e => e.img)
                .IsUnicode(false);

            modelBuilder.Entity<samples>()
                .Property(e => e.meal_time_name)
                .IsUnicode(false);

            modelBuilder.Entity<samples>()
                .Property(e => e.eliminate_name)
                .IsUnicode(false);

            modelBuilder.Entity<samples>()
                .Property(e => e.auditor_name)
                .IsUnicode(false);

            modelBuilder.Entity<samples>()
                .Property(e => e.sample_name)
                .IsUnicode(false);

            modelBuilder.Entity<screenshots>()
                .Property(e => e.path)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_buyers>()
                .Property(e => e.buyer_name)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_buyers>()
                .Property(e => e.buyer_order_shortname)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_buyers>()
                .Property(e => e.buyer_shortname)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_goods>()
                .Property(e => e.goods_name)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_goods>()
                .Property(e => e.goods_alias)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_goods>()
                .Property(e => e.goods_code)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_goods>()
                .Property(e => e.goods_desc)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_goods>()
                .Property(e => e.unit_code)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_goods>()
                .Property(e => e.spec)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_goods>()
                .Property(e => e.rs_memo)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_goods_categories>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_order_items>()
                .Property(e => e.order_id)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_order_items>()
                .Property(e => e.goods_name)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_order_items>()
                .Property(e => e.unit)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_order_items>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_order_items>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_order_items>()
                .Property(e => e.supplier_name)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_order_items>()
                .Property(e => e.compare_status)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_order_items>()
                .Property(e => e.last_order_price)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_users>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_users>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_users>()
                .Property(e => e.school_name)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.order_id)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.buyer_code)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.buyer_ids)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.buyer_info)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.buyer_name)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.buyer_names)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.chinese_total_amount)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.contact_person)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.cur_customer_name)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.customer_delivery_time_value)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.goods_class_list)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.last_modify_username)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.line_name)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.mobile)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.order_cancel_reason)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.order_detail)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.print_code)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.qr_code)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.rel_order_id)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.rel_order_type)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.remark)
                .IsUnicode(false);

            modelBuilder.Entity<world_barn_orders>()
                .Property(e => e.service_license_no)
                .IsUnicode(false);
        }
    }
}
