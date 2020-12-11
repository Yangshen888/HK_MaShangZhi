using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;

namespace HaiKanStudentDiningManagementSystem.Api.RequestPayload.Rbac.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRequestPayload : RequestPayload
    {
        /// <summary>
        /// 是否已被删除
        /// </summary>
        public CommonEnum.IsDeleted IsDeleted { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
       // public UserStatus Status { get; set; }

        public int UserType { get; set; }
        public string Phone { get; set; }
    }
}
