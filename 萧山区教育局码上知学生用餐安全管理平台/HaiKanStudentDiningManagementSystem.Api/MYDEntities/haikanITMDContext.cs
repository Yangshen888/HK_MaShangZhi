using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HaiKanStudentDiningManagementSystem.Api.MYDEntities
{
    public partial class haikanITMDContext : DbContext
    {
        public haikanITMDContext()
        {
        }

        public haikanITMDContext(DbContextOptions<haikanITMDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Articles> Articles { get; set; }
        public virtual DbSet<OfficialAccounts> OfficialAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseMySql("server=192.168.0.174;userid=root;pwd=root;port=3306;database=dining-ent;sslmode=none", x => x.ServerVersion("8.0.18-mysql"));
                optionsBuilder.UseMySql("server=10.0.2.3;user id=iot_tool_msa_ro;password=AQkf_wxFruNRDN3i;persistsecurityinfo=True;database=iot_tool_msa", x => x.ServerVersion("8.0.18-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articles>(entity =>
            {
                entity.ToTable("articles");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("articles_category_id_index");

                entity.HasIndex(e => e.Hidden)
                    .HasName("articles_hidden_index");

                entity.HasIndex(e => e.OfficialAccountId)
                    .HasName("articles_official_account_id_index");

                entity.HasIndex(e => e.OrgId)
                    .HasName("articles_org_id_index");

                entity.HasIndex(e => e.OriginalUrl)
                    .HasName("articles_original_url_index");

                entity.HasIndex(e => e.SiteId)
                    .HasName("articles_site_id_index");

                entity.HasIndex(e => e.UserId)
                    .HasName("articles_user_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("分类ID");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasColumnType("mediumtext")
                    .HasComment("文章内容")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Cover)
                    .HasColumnName("cover")
                    .HasColumnType("varchar(255)")
                    .HasComment("封面")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp")
                    .HasComment("创建时间");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(255)")
                    .HasComment("描述")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Hidden)
                    .HasColumnName("hidden")
                    .HasComment("是否隐藏");

                entity.Property(e => e.OfficialAccountId)
                    .HasColumnName("official_account_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("公众号id");

                entity.Property(e => e.OrgId)
                    .HasColumnName("org_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("组织ID");

                entity.Property(e => e.OriginalUrl)
                    .HasColumnName("original_url")
                    .HasColumnType("varchar(255)")
                    .HasComment("原地址")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.PublishedAt)
                    .HasColumnName("published_at")
                    .HasColumnType("timestamp")
                    .HasComment("发布时间");

                entity.Property(e => e.SiteId)
                    .HasColumnName("site_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("场地ID");

                entity.Property(e => e.Subject)
                    .HasColumnName("subject")
                    .HasColumnType("varchar(255)")
                    .HasComment("主题")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("用户ID");
            });

            modelBuilder.Entity<OfficialAccounts>(entity =>
            {
                entity.ToTable("official_accounts");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("official_accounts_category_id_index");

                entity.HasIndex(e => e.SchoolName)
                    .HasName("school_name");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Biz)
                    .HasColumnName("biz")
                    .HasColumnType("varchar(255)")
                    .HasComment("公众号biz")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasColumnType("int(10) unsigned")
                    .HasComment("分类id");

                entity.Property(e => e.LastFetchedAt)
                    .HasColumnName("last_fetched_at")
                    .HasColumnType("timestamp")
                    .HasComment("最后采集时间");

                entity.Property(e => e.MpId)
                    .HasColumnName("mp_id")
                    .HasColumnType("varchar(255)")
                    .HasComment("mp_id")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasComment("公众号名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.SchoolName)
                    .HasColumnName("school_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("学校名称")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
