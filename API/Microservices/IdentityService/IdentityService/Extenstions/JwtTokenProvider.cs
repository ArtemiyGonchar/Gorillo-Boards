using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
namespace PresentationLayer.Extenstions
{
    public class JwtTokenProvider
    {
        private readonly IConfiguration _configuration;

        public JwtTokenProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(Guid Id, string username, string role)
        {
            string secretKey = _configuration["Jwt:Secret"]; //in user secrets
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); //TODO add null check

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, role)
                    ]),
                Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
