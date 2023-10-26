using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Utils {
    public class Token {
        public static string Generate(User user) {
            var key = Encoding.ASCII.GetBytes("102030405060708090100110120130140150160170180190200");
            var tokenConfig = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new ("userId", user.id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}