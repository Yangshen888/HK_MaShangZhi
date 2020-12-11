using System;
using System.Collections.Generic;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;

namespace HaiKanStudentDiningManagementSystem.Api.ViewModels.Rbac.SystemUser
{
    /// <summary>
    /// 
    /// </summary>
    public class UserEditViewModel
    {
        public Guid SystemUserUuid { get; set; }
        public string LoginName { get; set; }
        public string RealName { get; set; }
        public string UserIdCard { get; set; }
        public string PassWord { get; set; }
        public int? UserType { get; set; }
        public Guid? SchoolUUID { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public string Phone { get; set; }
        public int? IsDeleted { get; set; }
        public string OldCard { get; set; }
        //public List<string> SystemRoleUuid { get; set; }
        public string SystemRoleUuid { get; set; }
        public string Job { get; set; }
        public int? HealthCertificate { get; set; }
        public Guid? ShopUuid { get; set; }
        public Guid? VillageId { get; set; }
        public int? IsStaff { get; set; }
        public int? IsCreEdu { get; set; }
        public bool IsUploadVideo { get; set; }
        public bool IsUploadPicture { get; set; }
        public bool IsAddPrecord { get; set; }
        public bool IsFlow { get; set; }
    }
}
