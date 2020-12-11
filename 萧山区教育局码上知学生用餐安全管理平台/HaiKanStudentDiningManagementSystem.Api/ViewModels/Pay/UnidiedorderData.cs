using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.Pay
{
    public class UnidiedorderData
    {
        public Guid Guid { get; set; }
        public string Body { get; set; } 
        public int Totalfee { get; set; }
        public string Openid { get; set; }
        //public string Idcard { get; set; }
        public Guid BillGuid { get; set; }
    }
}
