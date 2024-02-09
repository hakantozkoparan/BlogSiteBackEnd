using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace BlogSite.Security
{
    public static class TokenHandler
    {
        public static Token CreateToken(IConfiguration configuration) // token optionsları tutuluyor iconfiguration
        {
            Token token = new();
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"])); // tokenin güvenliğini sağlayan anahtar

            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256); // tokenin imzalanması için kullanılır
            token.Expiration = DateTime.Now.AddMinutes(Convert.ToInt16(configuration["Token:Expiration"])); // tokenin süresi

            JwtSecurityToken jwtSecurityToken = new(
                               issuer: configuration["Token:Issuer"],
                                              audience: configuration["Token:Audience"],
                                                             expires: token.Expiration,
                                                                    notBefore: DateTime.Now,
                                                                            signingCredentials: credentials
                                                                                       );
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken); // token oluşturuluyor
            byte[] numbers = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(numbers);
            token.RefreshToken = Convert.ToBase64String(numbers); // tokenin süresi dolduğunda yeniden token almak için kullanılır
            token.RefreshTokenExpiration = DateTime.Now.AddHours(Convert.ToInt16(configuration["Token:RefreshTokenExpiration"])); // tokenin süresi
                return token;
        }
    }
}
