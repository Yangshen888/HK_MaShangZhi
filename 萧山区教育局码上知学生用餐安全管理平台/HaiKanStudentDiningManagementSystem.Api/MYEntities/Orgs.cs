using System;
using System.Collections.Generic;

namespace HaiKanStudentDiningManagementSystem.Api.MYEntities
{
    public partial class Orgs
    {
        public ulong Id { get; set; }
        public uint OrganizationId { get; set; }
        public string Name { get; set; }
        public string SchoolName { get; set; }
        public string DetailAddress { get; set; }
        public string FullAddress { get; set; }
        public bool IsSchool { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public uint UserId { get; set; }
        public string Realname { get; set; }
        public uint UserOrganizationId { get; set; }
    }
}
