using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiDemo.Helpers
{
    public class TokenHelper
    {
        private readonly string _secretKey;

        public TokenHelper(IConfiguration configuration)
        {

            _secretKey = configuration["Jwt:Key"];
        }

        public string GenerateToken(string username)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_secretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {

                    new Claim("UserName", username),
                }),
                Expires = DateTime.UtcNow.AddDays(7),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
