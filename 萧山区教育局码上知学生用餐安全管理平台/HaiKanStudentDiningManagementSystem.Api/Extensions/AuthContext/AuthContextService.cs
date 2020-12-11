using System;
using System.Security.Claims;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using Microsoft.AspNetCore.Http;

namespace HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext
{
    public static class AuthContextService
    {
        private static IHttpContextAccessor _context;
        private static HttpContext context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor;
        }

        public static void Configure(HttpContext httpContext)
        {
            context = httpContext;
        }
        /// <summary>
        /// 
        /// </summary>
        public static HttpContext Current => _context?.HttpContext??context;
        /// <summary>
        /// 
        /// </summary>
        public static AuthContextUser CurrentUser
        {
            get
            {
                var user = new AuthContextUser
                {
                    LoginName = Current.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    DisplayName = Current.User.FindFirstValue("displayName"),
                    EmailAddress = Current.User.FindFirstValue("emailAddress"),
                    UserType = Current.User.FindFirstValue("userType").ToString(),
                    Avator = Current.User.FindFirstValue("avator"),
                    Guid = new Guid(Current.User.FindFirstValue("guid")),
                    Roleid = Current.User.FindFirstValue("roleid"),
                    RoleName = Current.User.FindFirstValue("roleName"),
                    SchoolGuid = CheckGuid(Current.User.FindFirstValue("schoolguid")),
                    SchoolName= Current.User.FindFirstValue("schoolName"),
                };
                return user;
            }
        }

        public static Guid? CheckGuid(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }
            else
            {
                return Guid.Parse(guid);
            }
        }

        /// <summary>
        /// 是否已授权
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return Current.User.Identity.IsAuthenticated;
            }
        }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public static bool IsSupperAdministator
        {
            get
            {
                return (Current.User.FindFirstValue("userType").ToString()== "1");
            }
        }
    }
}

