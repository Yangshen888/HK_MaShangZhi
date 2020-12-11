namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MessageBoard")]
    public partial class MessageBoard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid MessageUUID { get; set; }

        [StringLength(255)]
        public string Content { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        public DateTime? MessageDate { get; set; }

        public Guid? SystemUserUUID { get; set; }

        [StringLength(255)]
        public string Response { get; set; }

        [StringLength(255)]
        public string ResponseType { get; set; }

        public DateTime? ResponseDate { get; set; }

        public int? State { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public virtual School School { get; set; }

        public virtual SystemUser SystemUser { get; set; }
    }
}
