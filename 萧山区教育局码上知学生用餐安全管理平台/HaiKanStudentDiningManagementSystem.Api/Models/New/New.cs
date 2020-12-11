using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.Models.New
{
    public class New
    {
        public int ID { get; set; }
        public string Headline { get; set; }
        public string Uuid { get; set; }
        public string AddTime { get; set; }
        public string Accessory { get; set; }
        public string Type { get; set; }
        public string Tag { get; set; }
        public string Digest { get; set; }
        public string InOut { get; set; }
    }
}
