using EMS.DAL.EF.Jwt;
using Microsoft.AspNetCore.Identity;

namespace EMS.DAL.EF.Entities;

public class User : IdentityUser
{
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}