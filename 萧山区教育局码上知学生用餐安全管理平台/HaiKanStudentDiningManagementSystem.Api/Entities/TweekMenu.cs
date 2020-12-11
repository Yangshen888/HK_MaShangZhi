using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.Entities
{
    public partial class TweekMenu
    {
        public int Id { get; set; }
        public Guid TweekMenuUuid { get; set; }
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
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDelete { get; set; }
        public Guid? SchoolUuid { get; set; }

        public virtual School SchoolUu { get; set; }
    }
}
