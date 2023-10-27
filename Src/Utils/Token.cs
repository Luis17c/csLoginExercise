using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Models;


namespace Utils {
    public class Token {
        public static string Generate(User user) {
            var builder = WebApplication.CreateBuilder();
            var configuration = builder.Configuration;

            var key = Encoding.ASCII.GetBytes(configuration["Token:Key"]);
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