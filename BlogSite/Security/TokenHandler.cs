using BlogSite.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlogSite.Security
{
    public static class TokenHandler
    {
        public static Token CreateToken(IConfiguration configuration, User user, IList<string> roles) // user ve roles parametreleri eklendi
        {
            Token token = new();
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"]));

            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            token.Expiration = DateTime.Now.AddMinutes(Convert.ToInt16(configuration["Token:Expiration"]));

            var claims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name, user.UserName),
                     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                     new Claim(ClaimTypes.Role, "Manager") // Rol claim'i ekleniyor
                 };


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            JwtSecurityToken jwtSecurityToken = new(
                issuer: configuration["Token:Issuer"],
                audience: configuration["Token:Audience"],
                claims: claims, // Claims ekleniyor
                expires: token.Expiration,
                notBefore: DateTime.Now,
                signingCredentials: credentials
            );

            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);
            byte[] numbers = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(numbers);
            token.RefreshToken = Convert.ToBase64String(numbers);
            token.RefreshTokenExpiration = DateTime.Now.AddHours(Convert.ToInt16(configuration["Token:RefreshTokenExpiration"]));
            return token;
        }
    }
}
