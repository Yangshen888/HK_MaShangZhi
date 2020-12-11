namespace HaiKanTaskHousekeeperCore.mData
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NWeekMenu")]
    public partial class NWeekMenu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public Guid NWeekMenuUUID { get; set; }

        public string MonMon { get; set; }

        public string MonNoon { get; set; }

        public string MonNight { get; set; }

        public string TuesMon { get; set; }

        public string TuesNoon { get; set; }

        public string TuesNight { get; set; }

        public string WedMon { get; set; }

        public string WedNoon { get; set; }

        public string WedNight { get; set; }

        public string ThursMon { get; set; }

        public string ThursNoon { get; set; }

        public string ThursNight { get; set; }

        public string FriMon { get; set; }

        public string FriNoon { get; set; }

        public string FriNight { get; set; }

        public string SatMon { get; set; }

        public string SatNoon { get; set; }

        public string SatNight { get; set; }

        public string SunMon { get; set; }

        public string SunNoon { get; set; }

        public string SunNight { get; set; }

        [StringLength(255)]
        public string AddTime { get; set; }

        [StringLength(255)]
        public string AddPeople { get; set; }

        public int? IsDelete { get; set; }

        public Guid? SchoolUUID { get; set; }

        [StringLength(255)]
        public string Datetimes { get; set; }

        public virtual School School { get; set; }
    }
}
