namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemUser")]
    public partial class SystemUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SystemUser()
        {
            MessageBoard = new HashSet<MessageBoard>();
            Order = new HashSet<Order>();
            Report = new HashSet<Report>();
        }

        [Key]
        public Guid SystemUserUUID { get; set; }

        [StringLength(255)]
        public string LoginName { get; set; }

        [StringLength(255)]
        public string RealName { get; set; }

        [StringLength(255)]
        public string UserIdCard { get; set; }

        [StringLength(255)]
        public string PassWord { get; set; }

        public int? UserType { get; set; }

        public Guid? SchoolUUID { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDeleted { get; set; }

        [Column(TypeName = "text")]
        public string ManageDepartment { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(255)]
        public string ZaiGang { get; set; }

        [StringLength(255)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Sex { get; set; }

        [StringLength(20)]
        public string InTime { get; set; }

        [StringLength(255)]
        public string Types { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string SystemRoleUUid { get; set; }

        [StringLength(255)]
        public string Remarks { get; set; }

        [StringLength(255)]
        public string StaffNum { get; set; }

        [StringLength(255)]
        public string Wechat { get; set; }

        [StringLength(255)]
        public string Topic { get; set; }

        [StringLength(255)]
        public string Content { get; set; }

        [StringLength(255)]
        public string Train { get; set; }

        [StringLength(255)]
        public string Main { get; set; }

        [StringLength(255)]
        public string Job { get; set; }

        public int? HealthCertificate { get; set; }

        public int? IsStaff { get; set; }

        public int? IsCreEdu { get; set; }

        public bool IsUploadVideo { get; set; }

        public bool IsUploadPicture { get; set; }

        public bool IsAddPRecord { get; set; }

        public bool IsFlow { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MessageBoard> MessageBoard { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report> Report { get; set; }

        public virtual School School { get; set; }
    }
}
