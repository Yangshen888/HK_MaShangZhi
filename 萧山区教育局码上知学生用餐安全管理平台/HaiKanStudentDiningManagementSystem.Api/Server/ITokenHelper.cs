using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HaiKanStudentDiningManagementSystem.Api.Entities.User;

namespace HaiKanStudentDiningManagementSystem.Api.Server
{
    public interface ITokenHelper
    {
        ComplexToken CreateToken(UserData user);
        ComplexToken CreateToken(Claim[] claims);
        Token RefreshToken(ClaimsPrincipal claimsPrincipal);
    }
}
