using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task4Back.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Task4Back.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateToken(User user);

        public int? ValidateToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration Configuration;

        public JwtUtils(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(Configuration["JWT_SECRET"]);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateToken(string token)
        {
            if (token == null)
            {
                return null;
            }

            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(Configuration["JWT_SECRET"]);
            try
            {
                _ = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;

                return int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            }
            catch
            {
                return null;
            }
        }
    }
}